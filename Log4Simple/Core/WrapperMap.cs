using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Log4Simple.Core
{
    public delegate ILoggerWrapper WrapperCreationHandler(ILogger logger);

    public class WrapperMap
    {
        private readonly Hashtable m_Repositories = new Hashtable();
        private readonly WrapperCreationHandler m_createWrapperHandler;

        public WrapperMap(WrapperCreationHandler createWrapperHandler)
        {
            m_createWrapperHandler = createWrapperHandler;
        }

        virtual protected ILoggerWrapper CreateNewWrapperObject(ILogger logger)
        {
            if (m_createWrapperHandler != null)
            {
                return m_createWrapperHandler(logger);
            }
            return null;
        }
        /// <summary>
        /// The wrapper map to use to hold the <see cref="LogImpl"/> objects. 
        /// </summary>
        /// <param name="logger"></param>
        /// <returns></returns>
        virtual public ILoggerWrapper GetWrapper(ILogger logger)
        {
            if (logger == null)
                return null;

            lock (this)
            {
                Hashtable wrappermap = (Hashtable)m_Repositories[logger.Repository];
                if (wrappermap == null)
                {
                    wrappermap = new Hashtable();
                    m_Repositories[logger.Repository] = wrappermap;
                }
                ILoggerWrapper wrapperobject = wrappermap[logger] as ILoggerWrapper;
                if (wrapperobject == null)
                {
                    wrapperobject = CreateNewWrapperObject(logger);
                    wrappermap[logger] = wrapperobject;

                }

                return wrapperobject;
            }
        }
    }
}
