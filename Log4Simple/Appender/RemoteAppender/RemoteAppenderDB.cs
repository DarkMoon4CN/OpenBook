using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Log4Simple.Util;
using Log4Simple.Core;
using Log4Simple.Layout;
using System.Collections;

namespace Log4Simple.Appender.RemoteAppender
{
    
    /// <summary>
    /// 把日志数据插入到目标数据库中
    /// CREATE TABLE [dbo].[Log_NewBookFlat] ( 
     //  [ID] [int] IDENTITY (1, 1) NOT NULL Primary Key,
     //  [AppNo] [int] NOT NULL,
     //  [OccurTime] [datetime] NOT NULL ,       
     //  [Level] [int] NOT NULL ,
     //  [LogName] [varchar] (255) NOT NULL ,
     //  [Message] [varchar] (4000) NOT NULL 
     //) 
    /// 
    /// </summary>
    /// <author>liuww</author>    
    public class RemoteAppenderDB :RemoteAppenderSkeleton
    {


        

        /// <summary>
        /// 数据库连接
        /// </summary>
        public RemoteAppenderDB()
        {
            m_connectionType = "System.Data.OleDb.OleDbConnection";
            m_useTransactions = true;
            m_commandType = System.Data.CommandType.Text;
            m_parameters = new ArrayList();
            

            
        }

      
      
        #region 数据入库

      


        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="loggingEvents"></param>
        public override void DoAppend(LoggingData[] loggingEvents)
        {

            if (m_dbConnection == null || m_dbConnection.State != ConnectionState.Open || m_ErrorCount >= m_ErrorCountAfterRetry)
            {
                LogLog.Debug(declaringType, "Attempting to reconnect to database. Current Connection State: " + ((m_dbConnection == null) ? SystemInfo.NullText : m_dbConnection.State.ToString()));
                m_ErrorCount = 0;
                InitDbConnection();
                InitDbCommand();
                              
            }

            if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
            {
                if (m_useTransactions)
                {
                    //日志写入目标数据库中
                    IDbTransaction trans = null;
                    try
                    {
                        trans = m_dbConnection.BeginTransaction();
                        DoAppend(trans, loggingEvents);
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            trans.Rollback();
                        }
                        catch { }
                        m_ErrorCount++;

                        throw;
                    }
                    finally
                    {

                        if (trans != null)
                        {
                            trans.Dispose();
                        }
                    }
                }
                else
                    DoAppend(null, loggingEvents);
            }           
       
           
        }


        /// <summary>
        /// Writes the events to the database using the transaction specified.
        /// </summary>
        /// <param name="dbTran">The transaction that the events will be executed under.</param>
        /// <param name="events">The array of events to insert into the database.</param>
        /// <remarks>
        /// <para>
        /// The transaction argument can be <c>null</c> if the appender has been
        /// configured not to use transactions. See <see cref="UseTransactions"/>
        /// property for more information.
        /// </para>
        /// </remarks>
        private void DoAppend(IDbTransaction dbTran, LoggingData[] events)
        {           
            // Send buffer using the prepared command object
            if (m_dbCommand != null)
            {
                if (dbTran != null)
                {
                    m_dbCommand.Transaction = dbTran;
                }

                // run for all events
                foreach (LoggingData e in events)
                {
                    if (!FilterEvent(e))
                        continue;
                    // Set the parameter values
                    foreach (DBAppenderParameter param in m_parameters)
                    {
                        param.FormatValue(m_dbCommand, e);
                    }

                    // Execute the query
                    m_dbCommand.ExecuteNonQuery();                        
                }                 
            }
           
        }


        #region Public Instance Methods

        /// <summary>
        /// Adds a parameter to the command.
        /// </summary>
        /// <param name="parameter">The parameter to add to the command.</param>
        /// <remarks>
        /// <para>
        /// Adds a parameter to the ordered list of command parameters.
        /// </para>
        /// </remarks>
        public void AddParameter(DBAppenderParameter parameter)
        {
            m_parameters.Add(parameter);
        }


        #endregion // Public Instance Methods

       
      
       
        
        /// <summary>
        /// 初始化Command
        /// </summary>
        private void InitDbCommand()
        {
            if (m_dbConnection != null)
            {
                try
                {
                    DisposeCommand();

                    // Create the command object
                    m_dbCommand = m_dbConnection.CreateCommand();

                    // Set the command string
                    m_dbCommand.CommandText = m_commandText;

                    // Set the command type
                    m_dbCommand.CommandType = m_commandType;
                }
                catch (Exception e)
                {                    
                    DisposeCommand();
                    throw e;
                }

                if (m_dbCommand != null)
                {
                    try
                    {
                        foreach (DBAppenderParameter param in m_parameters)
                        {
                           
                            param.Prepare(m_dbCommand);                           
                        }
                    }
                    catch(Exception ex)
                    {
                        DisposeCommand();
                        throw;
                    }
                }

                if (m_dbCommand != null)
                {
                    try
                    {
                        // Prepare the command statement.
                        m_dbCommand.Prepare();
                    }
                    catch (Exception e)
                    {                        
                        DisposeCommand();
                        throw;
                    }
                }
            }
        }                         


        #endregion

       

        #region 准备资源


        /// <summary>
        /// Connects to the database.
        /// </summary>		
        private void InitDbConnection()
        {
         
 

            try
            {
                DisposeCommand();
                DiposeConnection();

                

                m_dbConnection = CreateConnection(m_connectionType, m_connectionString);
                //m_dbConnection.ConnectionTimeout = 30;
                m_dbConnection.Open();                
            }
            catch (Exception e)
            {                              
                if (m_dbConnection != null)
                {
                    m_dbConnection.Close();
                    m_dbConnection.Dispose();
                    m_dbConnection = null;
                }

                throw;
            }
        }

        
        /// <summary>
        /// Creates an <see cref="IDbConnection"/> instance used to connect to the database.
        /// </summary>
        /// <remarks>
        /// This method is called whenever a new IDbConnection is needed (i.e. when a reconnect is necessary).
        /// </remarks>
        /// <param name="connectionType">The <see cref="Type"/> of the <see cref="IDbConnection"/> object.</param>
        /// <param name="connectionString">The connectionString output from the ResolveConnectionString method.</param>
        /// <returns>An <see cref="IDbConnection"/> instance with a valid connection string.</returns>
        virtual protected IDbConnection CreateConnection(string connectionType, string connectionString)
        {
           
            IDbConnection connection = (IDbConnection)Activator.CreateInstance(SystemInfo.GetTypeFromString(connectionType, true, false));
            connection.ConnectionString = connectionString;
            return connection;
        }


        /// <summary>
        /// Cleanup the existing command.
        /// </summary>
        /// <param name="ignoreException">
        /// If true, a message will be written using LogLog.Warn if an exception is encountered when calling Dispose.
        /// </param>
        private void DisposeCommand()
        {
            // Cleanup any existing command or connection
            if (m_dbCommand != null)
            {
                try
                {
                    m_dbCommand.Dispose();
                }
                catch (Exception ex)
                {
                   
                    Error("Exception while disposing cached command object", ex);                    
                }
                m_dbCommand = null;
            }
        }


        /// <summary>
        /// Cleanup the existing connection.
        /// </summary>
        /// <remarks>
        /// Calls the IDbConnection's <see cref="IDbConnection.Close"/> method.
        /// </remarks>
        private void DiposeConnection()
        {
            if (m_dbConnection != null)
            {
                try
                {
                    try
                    {
                        m_dbConnection.Close();
                    }
                    catch { }
                    m_dbConnection.Dispose();
                }
                catch (Exception ex)
                {
                    Error("Exception while disposing cached connection object", ex);
                }
                m_dbConnection = null;
            }
        }


        #endregion

        #region 关闭

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Close()
        {

            DisposeCommand();
            DiposeConnection();

            base.Close();
        }

        #endregion


        #region 变量定义

        /// <summary>
        /// The list of <see cref="AdoNetAppenderParameter"/> objects.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The list of <see cref="AdoNetAppenderParameter"/> objects.
        /// </para>
        /// </remarks>
        private ArrayList m_parameters;


      

        /// <summary>
        /// 数据库连接
        /// </summary>
        private IDbConnection m_dbConnection;

        /// <summary>
        /// 数据库处理类
        /// </summary>
        private IDbCommand m_dbCommand;

        /// <summary>
        /// 连接数据库串
        /// </summary>
        private string m_connectionString;

        /// <summary>
        /// 数据库连接类型
        /// </summary>
        private string m_connectionType;

        /// <summary>
        /// Indicates whether to use transactions when writing to the database.
        /// </summary>
        private bool m_useTransactions;

        /// <summary>
        /// SQL Script
        /// </summary>
        private string m_commandText;

        /// <summary>
        /// The command type.
        /// </summary>
        private CommandType m_commandType;


        private int m_ErrorCount = 0;
        
        /// <summary>
        /// 错误多少次后重试
        /// </summary>
        private int m_ErrorCountAfterRetry = 5;
     

        /// <summary>
        /// 执行超时时间
        /// </summary>
        private int m_ExecuteTimeOut = 60*6;

        /// <summary>
        /// 连接超时时间
        /// </summary>
        private int m_ConnectionTimeOut = 60;

     

        /// <summary>
        /// The fully qualified type of the RemoteAppenderDB class.
        /// </summary>
        /// <remarks>
        /// Used by the internal logger to record the Type of the
        /// log message.
        /// </remarks>
        private readonly static Type declaringType = typeof(RemoteAppenderDB);

        #endregion



        #region 属性定义


       

        /// <summary>
        /// 连接数据库串
        /// </summary>
        public string ConnectionString
        {
            get { return m_connectionString; }
            set { m_connectionString = value; }
        }

        /// <summary>
        /// 连接类型
        /// </summary>
        public string ConnectionType
        {
            get { return m_connectionType; }
            set { m_connectionType = value; }
        }

        /// <summary>
        /// Gets or sets the command text that is used to insert logging events
        /// into the database.
        /// </summary>
        /// <value>
        /// The command text used to insert logging events into the database.
        /// </value>
        /// <remarks>
        /// <para>
        /// Either the text of the prepared statement or the
        /// name of the stored procedure to execute to write into
        /// the database.
        /// </para>
        /// <para>
        /// The <see cref="CommandType"/> property determines if
        /// this text is a prepared statement or a stored procedure.
        /// </para>
        /// </remarks>
        public string CommandText
        {
            get { return m_commandText; }
            set { m_commandText = value; }
        }


        /// <summary>
		/// Should transactions be used to insert logging events in the database.
		/// </summary>
		/// <value>
		/// <c>true</c> if transactions should be used to insert logging events in
		/// the database, otherwise <c>false</c>. The default value is <c>true</c>.
		/// </value>
		/// <remarks>
		/// <para>
		/// Gets or sets a value that indicates whether transactions should be used
		/// to insert logging events in the database.
		/// </para>
		/// <para>
		/// When set a single transaction will be used to insert the buffered events
		/// into the database. Otherwise each event will be inserted without using
		/// an explicit transaction.
		/// </para>
		/// </remarks>
		public bool UseTransactions
		{
			get { return m_useTransactions; }
			set { m_useTransactions = value; }
		}


        public int ErrorCountAfterRetry
        {
            get { return m_ErrorCountAfterRetry; }
            set { m_ErrorCountAfterRetry = value; }
        }
        /// <summary>
        /// 执行超时时间
        /// </summary>
        public int ExecuteTimeOut
        {            
            set { m_ExecuteTimeOut = value; }
            get { return m_ExecuteTimeOut; }
        }

        /// <summary>
        /// 连接超时时间
        /// </summary>
        public int ConnectionTimeout
        {
            set { m_ConnectionTimeOut = value; }
            get { return m_ConnectionTimeOut; }
        }

        #endregion


    }

    /// <summary>
    /// Parameter type used by the <see cref="DBAppenderParameter"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class provides the basic database parameter properties
    /// as defined by the <see cref="System.Data.IDbDataParameter"/> interface.
    /// </para>
    /// <para>This type can be subclassed to provide database specific
    /// functionality. The two methods that are called externally are
    /// <see cref="Prepare"/> and <see cref="FormatValue"/>.
    /// </para>
    /// </remarks>
    public class DBAppenderParameter
    {
        #region Public Instance Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AdoNetAppenderParameter" /> class.
		/// </summary>
		/// <remarks>
		/// Default constructor for the AdoNetAppenderParameter class.
		/// </remarks>
        public DBAppenderParameter()
		{
			m_precision = 0;
			m_scale = 0;
			m_size = 0;
		}

		#endregion // Public Instance Constructors

		#region Public Instance Properties

		/// <summary>
		/// Gets or sets the name of this parameter.
		/// </summary>
		/// <value>
		/// The name of this parameter.
		/// </value>
		/// <remarks>
		/// <para>
		/// The name of this parameter. The parameter name
		/// must match up to a named parameter to the SQL stored procedure
		/// or prepared statement.
		/// </para>
		/// </remarks>
		public string ParameterName
		{
			get { return m_parameterName; }
			set { m_parameterName = value; }
		}

		/// <summary>
		/// Gets or sets the database type for this parameter.
		/// </summary>
		/// <value>
		/// The database type for this parameter.
		/// </value>
		/// <remarks>
		/// <para>
		/// The database type for this parameter. This property should
		/// be set to the database type from the <see cref="DbType"/>
		/// enumeration. See <see cref="IDataParameter.DbType"/>.
		/// </para>
		/// <para>
		/// This property is optional. If not specified the ADO.NET provider 
		/// will attempt to infer the type from the value.
		/// </para>
		/// </remarks>
		/// <seealso cref="IDataParameter.DbType" />
		public DbType DbType
		{
			get { return m_dbType; }
			set 
			{ 
				m_dbType = value; 
				m_inferType = false;
			}
		}

		/// <summary>
		/// Gets or sets the precision for this parameter.
		/// </summary>
		/// <value>
		/// The precision for this parameter.
		/// </value>
		/// <remarks>
		/// <para>
		/// The maximum number of digits used to represent the Value.
		/// </para>
		/// <para>
		/// This property is optional. If not specified the ADO.NET provider 
		/// will attempt to infer the precision from the value.
		/// </para>
		/// </remarks>
		/// <seealso cref="IDbDataParameter.Precision" />
		public byte Precision 
		{
			get { return m_precision; } 
			set { m_precision = value; }
		}

		/// <summary>
		/// Gets or sets the scale for this parameter.
		/// </summary>
		/// <value>
		/// The scale for this parameter.
		/// </value>
		/// <remarks>
		/// <para>
		/// The number of decimal places to which Value is resolved.
		/// </para>
		/// <para>
		/// This property is optional. If not specified the ADO.NET provider 
		/// will attempt to infer the scale from the value.
		/// </para>
		/// </remarks>
		/// <seealso cref="IDbDataParameter.Scale" />
		public byte Scale 
		{
			get { return m_scale; }
			set { m_scale = value; }
		}

		/// <summary>
		/// Gets or sets the size for this parameter.
		/// </summary>
		/// <value>
		/// The size for this parameter.
		/// </value>
		/// <remarks>
		/// <para>
		/// The maximum size, in bytes, of the data within the column.
		/// </para>
		/// <para>
		/// This property is optional. If not specified the ADO.NET provider 
		/// will attempt to infer the size from the value.
		/// </para>
        /// <para>
        /// For BLOB data types like VARCHAR(max) it may be impossible to infer the value automatically, use -1 as the size in this case.
        /// </para>
		/// </remarks>
		/// <seealso cref="IDbDataParameter.Size" />
		public int Size 
		{
			get { return m_size; }
			set { m_size = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="IRawLayout"/> to use to 
		/// render the logging event into an object for this 
		/// parameter.
		/// </summary>
		/// <value>
		/// The <see cref="IRawLayout"/> used to render the
		/// logging event into an object for this parameter.
		/// </value>
		/// <remarks>
		/// <para>
		/// The <see cref="IRawLayout"/> that renders the value for this
		/// parameter.
		/// </para>
		/// <para>
		/// The <see cref="RawLayoutConverter"/> can be used to adapt
		/// any <see cref="ILayout"/> into a <see cref="IRawLayout"/>
		/// for use in the property.
		/// </para>
		/// </remarks>
		public IRawLayout Layout
		{
			get { return m_layout; }
			set { m_layout = value; }
		}

		#endregion // Public Instance Properties

		#region Public Instance Methods

		/// <summary>
		/// Prepare the specified database command object.
		/// </summary>
		/// <param name="command">The command to prepare.</param>
		/// <remarks>
		/// <para>
		/// Prepares the database command object by adding
		/// this parameter to its collection of parameters.
		/// </para>
		/// </remarks>
		virtual public void Prepare(IDbCommand command)
		{
			// Create a new parameter
			IDbDataParameter param = command.CreateParameter();

			// Set the parameter properties
			param.ParameterName = m_parameterName;

			if (!m_inferType)
			{
				param.DbType = m_dbType;
			}
			if (m_precision != 0)
			{
				param.Precision = m_precision;
			}
			if (m_scale != 0)
			{
				param.Scale = m_scale;
			}
			if (m_size != 0)
			{
				param.Size = m_size;
			}

			// Add the parameter to the collection of params
			command.Parameters.Add(param);
		}

		/// <summary>
		/// Renders the logging event and set the parameter value in the command.
		/// </summary>
		/// <param name="command">The command containing the parameter.</param>
		/// <param name="loggingEvent">The event to be rendered.</param>
		/// <remarks>
		/// <para>
		/// Renders the logging event using this parameters layout
		/// object. Sets the value of the parameter on the command object.
		/// </para>
		/// </remarks>
		virtual public void FormatValue(IDbCommand command, LoggingData loggingEvent)
		{
			// Lookup the parameter
			IDbDataParameter param = (IDbDataParameter)command.Parameters[m_parameterName];

			// Format the value
			object formattedValue = Layout.Format(loggingEvent);

			// If the value is null then convert to a DBNull
			if (formattedValue == null)
			{
				formattedValue = DBNull.Value;
			}

			param.Value = formattedValue;
		}

		#endregion // Public Instance Methods

		#region Private Instance Fields

		/// <summary>
		/// The name of this parameter.
		/// </summary>
		private string m_parameterName;

		/// <summary>
		/// The database type for this parameter.
		/// </summary>
		private DbType m_dbType;

		/// <summary>
		/// Flag to infer type rather than use the DbType
		/// </summary>
		private bool m_inferType = true;

		/// <summary>
		/// The precision for this parameter.
		/// </summary>
		private byte m_precision;

		/// <summary>
		/// The scale for this parameter.
		/// </summary>
		private byte m_scale;

		/// <summary>
		/// The size for this parameter.
		/// </summary>
		private int m_size;

		/// <summary>
		/// The <see cref="IRawLayout"/> to use to render the
		/// logging event into an object for this parameter.
		/// </summary>
		private IRawLayout m_layout;

		#endregion // Private Instance Fields 
    }
}
