using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class FunctionEntity
    {
        private int _function_id;
        private string _function_name;
        private string _function_url;
        private int _function_parentid;
        private string _function_order;
        private int _function_isvalid;
        private int _function_Level;
        private string _function_url_new;

        public FunctionEntity()
        {
            ChildFunctionList = new List<FunctionEntity>();
        }

        /// <summary>
        /// 
        /// </summary>
        public int Function_ID
        {
            set { _function_id = value; }
            get { return _function_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Function_Name
        {
            set { _function_name = value; }
            get { return _function_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Function_URL
        {
            set { _function_url = value; }
            get { return _function_url; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Function_URL_New
        {
            set { _function_url_new = value; }
            get { return _function_url_new; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Function_ParentID
        {
            set { _function_parentid = value; }
            get { return _function_parentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Function_Order
        {
            set { _function_order = value; }
            get { return _function_order; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Function_isValid
        {
            set { _function_isvalid = value; }
            get { return _function_isvalid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Function_Level
        {
            get { return _function_Level; }
            set { _function_Level = value; }
        }

        private int _function_IsNew;
        /// <summary>
        /// 
        /// </summary>
        public int Function_IsNew
        {
            set { _function_IsNew = value; }
            get { return _function_IsNew; }
        }

        public DateTime CreateDate { get; set; }

        public List<FunctionEntity> ChildFunctionList { get; set; }
    }

    public class FunctionSearchEntity : EntityBase
    {
        public int Function_ID { get; set; }

        public string Function_Name { get; set; }

        public int Function_IsNew { get; set; }

        public int Function_isValid { get; set; }
    }
}
