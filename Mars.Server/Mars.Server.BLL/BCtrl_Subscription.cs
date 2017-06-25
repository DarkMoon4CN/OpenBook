using Mars.Server.DAO;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL
{
    public class BCtrl_Subscription
    {
        private BCtrl_Subscription() { }
        private static BCtrl_Subscription _instance;
        public static BCtrl_Subscription Instance { get { return _instance ?? (_instance = new BCtrl_Subscription()); } }
        private static readonly SubscriptionDAO dao = new SubscriptionDAO();


        /// <summary>
        ///  查询订阅结果集
        /// </summary>
        /// <param name="userID">用户id</param>
        /// <param name="subID">末尾订阅ID</param>
        /// <param name="strWhere">查询条件 用于查询是否订阅条件</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">起始页 默认为1</param>
        /// <param name="type">YES 已订阅  NO 未订阅</param>
        /// <param name="header">已订阅的</param>
        /// <param name="footer">未订阅的</param>
        /// <returns></returns>
        public  void Subscription_GetList (bool isHeader, int headerID, int headerSize,int headerIndex
                                         , bool isFooter, int footerID, int footerSize,int footerIndex
                                         , int userID,string keyword
                                         , out List<SubscriptionEntity> header,out List<SubscriptionEntity> footer)
        {
            header = new List<SubscriptionEntity>();
            footer = new List<SubscriptionEntity>();
            string strWhere = string.Empty;
            if (!string.IsNullOrEmpty(keyword) && keyword != "") //关键字查询
            {
                strWhere = " AND (s.SubName LIKE '%{0}%'  OR  s.PinYin LIKE '%{1}%' OR s.SubShortName  LIKE '%{2}%' )  ";
                strWhere = string.Format(strWhere,keyword,keyword,keyword);
            }
            if (userID > 0)
            {
                if (isHeader)
                {
                    var hResult = dao.Subscription_GetList(userID, headerID, headerSize, strWhere, headerIndex, SubQueryType.YES);
                   header = hResult.AppendData;
                }
                if (isFooter)
                {
                    var fResult = dao.Subscription_GetList(userID, footerID, footerSize, strWhere, footerIndex, SubQueryType.NO);
                   footer = fResult.AppendData;
                }
            }
            else 
            {
                var fResult = dao.Subscription_GetList(-1, footerID, footerSize, strWhere, footerIndex, SubQueryType.NO);
               footer = fResult.AppendData;
            }
        }


        public OperationResult<bool> Subscription_Insert(int userID, List<int> subIDs) 
        {
            return dao.Subscription_Insert(userID, subIDs);
        }

        public OperationResult<bool> Subscription_Del(int userID, List<int> subIDs)
        {
            return dao.Subscription_Del(userID, subIDs);
        }

        /// <summary>
        /// 获取单条订阅明细
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="subID"></param>
        /// <returns></returns>
        public OperationResult<SubscriptionEntity> Subscription_Get(int userID, int subID)
        {
            return dao.Subscription_Get(userID, subID);
        }
    }
}
