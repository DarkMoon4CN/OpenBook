using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mars.Server.Utils
{
    /// <summary>
    ///  当使用增删改时调用此业务类
    /// </summary>
    public class OperationResult<T>
    {
        public OperationResult(OperationResultType resultType) 
        {
            ResultType = resultType;
        }

        public OperationResult(OperationResultType resultType, string message)
            : this(resultType)
           
        {
            Message = message;
        }

        public OperationResult(OperationResultType resultType, string message, string logMessage) 
            :this(resultType,message)
        {
            LogMessage = LogMessage;
        }

        public OperationResult(OperationResultType resultType, string message, T appendData)
            : this(resultType, message)
        {
            AppendData = appendData;
        }


        public string GetEnumDescription(OperationResultType obj)
        {
            var type = obj.GetType();
            FieldInfo field = type.GetField(Enum.GetName(type, obj));
            DescriptionAttribute descAttr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (descAttr == null)
            {
                return string.Empty;
            }
            return descAttr.Description;
        }
        
        /// <summary>
        /// 获取或设置 追加的附加参数
        /// </summary>
        public T AppendData { get; set; }

        /// <summary>
        /// 获取或设置 状态枚举
        /// </summary>
        public OperationResultType ResultType { get; set; }
        
        /// <summary>
        /// 获取或设置 日志消息
        /// </summary>
        public string LogMessage { get; set; }
        
        /// <summary>
        /// 获取或设置 操作返回信息
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 业务枚举类描述
    /// </summary>
    public class Description 
    {
        public  static string EnumDescription(OperationResultType obj)
        {
            var type = obj.GetType();
            FieldInfo field = type.GetField(Enum.GetName(type, obj));
            DescriptionAttribute descAttr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (descAttr == null)
            {
                return string.Empty;
            }
            return descAttr.Description;
        }
    }

    /// <summary>
    /// 业务枚举类
    /// </summary>
    [Description("业务操作结果的枚举")]
    public enum OperationResultType
    {
        [Description("操作成功。")]
        Success=1,
        [Description("操作没有引发任何变化，提交取消。")]
        NoChanged,
        [Description("参数错误。")]
        ParamError,
        [Description("指定参数的数据不存在。")]
        QueryNull,
        [Description("当前用户权限不足，不能继续操作。")]
        PurviewLack,
        [Description("非法操作。")]
        IllegalOperation,
        [Description("警告")]
        Warning,
        [Description("操作引发错误。")]
        Error,
        [Description("无法连接至服务器！")]
        NoConnection,
    }

    [Serializable]
    public abstract class Entity 
    {
        protected Entity() 
        {

        }

        public bool IsDeleted { get; set; }

        public DateTime AddTime { get; set; }

        public byte[] TimeStamp { get; set; }
    }

}
