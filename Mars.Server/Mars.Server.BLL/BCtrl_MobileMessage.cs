using Mars.Server.DAO;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL
{
    public class BCtrl_MobileMessage
    {
        private BCtrl_MobileMessage() { }
        private static BCtrl_MobileMessage _instance;
        public static BCtrl_MobileMessage Instance { get { return _instance ?? (_instance = new BCtrl_MobileMessage()); } }
        private static readonly MobileMessageDAO dao = new MobileMessageDAO();

        public OperationResult<int> MobileMessage_Total(string strWhere = null)
        {
            return dao.MobileMessage_Total(strWhere);
        }

        public OperationResult<int> MobileMessage_Total(int userId,int messageType)
        {
            string strWhere = string.Empty;
            if (userId != 0)
            {
                strWhere += " AND ToUserID={0} ";
                strWhere = string.Format(strWhere, userId);
            }
            if (messageType == 1)
            {
                strWhere += " AND  MessageType IN(1,2)   ";
            }
            strWhere += " AND IsRead =0 ";
            return dao.MobileMessage_Total(strWhere);
        }

        public OperationResult<List<RelateEntity>> Relate_GetList(int userId, int messageID, int pageIndex, int pageSize)
        {
            dao.Relate_UpdateRead(userId);
            var result = dao.Relate_GetList(userId, messageID, pageIndex, pageSize);
            foreach (var item in result.AppendData)
            {
                if(item.FromUserIsAnonymous==true) 
                {
                    item.FromUserNickName = "匿名";
                    item.FromUserPicture=AppUtil.UserAnonymousHeader + AppUtil.ConvertJpg;
                }
                if (string.IsNullOrEmpty(item.FromUserPicture)==false)
                {
                    item.FromUserPicture = item.FromUserPictureDomain + item.FromUserPicture + AppUtil.ConvertJpg;
                }
                else if (item.FromUserThirdPictureUrl != null)
                {
                    item.FromUserPicture = item.FromUserThirdPictureUrl;
                }
                else 
                {
                    //如果没有头像就使用默认用户头像
                    item.FromUserPicture = AppUtil.UserDefaultHeader + AppUtil.ConvertJpg;
                }
                item.EventItemPicture = item.EventItemPictureDomain + item.EventItemPicture + AppUtil.ConvertJpg;
            }
            return result;
        }

        /// <summary>
        /// 删除与我相关的消息,在删除评论之前删除
        /// </summary>
        /// <param name="commmentID">评论ID</param>
        public void Relate_Delete(int commmentID) 
        {
            dao.Relate_Delete(commmentID);
        }
    }
}
