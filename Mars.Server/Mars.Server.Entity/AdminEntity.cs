using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    /// <summary>
    /// 系统用户实体
    /// </summary>
   public class AdminEntity
    {
       public int User_ID { get; set; }

       public string TrueName { get; set; }

       /// <summary>
       /// 登录名
       /// </summary>
       public string User_Name { get; set; }

       public string User_Pwd { get; set; }

       public int User_Sex { get; set; }

       /// <summary>
       /// 分机号
       /// </summary>
       public string User_Tel { get; set; }

       /// <summary>
       /// 电话
       /// </summary>
       public string User_Tel_Private { get; set; }

       public string User_Mobile { get; set; }

       public string User_Mail { get; set; }

       public string User_PhotoPath { get; set; }

       public int User_DeptID { get; set; }      

       public string User_PositionID { get; set; }

       public DateTime RegisterDate { get; set; }

       /// <summary>
       /// 是否禁用
       /// </summary>
       public bool IsValid { get; set; }
      
    }
}
