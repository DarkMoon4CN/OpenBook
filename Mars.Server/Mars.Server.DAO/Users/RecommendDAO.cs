using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Mars.Server.DAO.Users
{
    public class RecommendDAO
    {
        public int WriteNumber(int recommendId, string phoneNum)
        {
            int returnValue = 0;

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(@"INSERT INTO dbo.M_User_Recommend_Mobile
                                        ( RecommendID, Mobile )
                                VALUES  ( {0},'{1}');
                                SELECT @@IDENTITY;"
                , recommendId.ToString()
                , phoneNum);

            try
            {
                object obj = SQlHelper.ExecuteNonQuery(SQlHelper.MyConnectStr, CommandType.Text, strSql.ToString(), null);
                if (obj!=null)
                {
                    returnValue = int.Parse(obj.ToString());
                }
            }
            catch (Exception ex)
            {
                LogUtil.WriteLog(ex);
            }
            return returnValue;
        }
    }
}
