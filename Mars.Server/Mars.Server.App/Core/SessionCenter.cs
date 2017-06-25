using log4net;
using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.App.Core
{
    public class SessionCenter
    {

        static SessionCenter()
        {
            _SessionPools = new ConcurrentDictionary<string, SessionIdentity>();
        }
        private static System.Collections.Concurrent.ConcurrentDictionary<string, SessionIdentity> _SessionPools;
        private static string SessionBagFile = Path.Combine(AppPath.App_Root, "sessionpools.dat");

        private static ILog marsLog = log4net.LogManager.GetLogger(typeof(SessionCenter));
        
        public static bool RefreshSessionIdentity(string sessionid)
        {
            SessionIdentity si = new SessionIdentity();
            if (_SessionPools.TryGetValue(sessionid, out si))
            {
                si.CreatedTime = DateTime.Now;
                return true;
            }
            else
                return false;
        }

        public static SessionIdentity GetIdentity(string sessionid)
        {
            if (sessionid.ToLower() == CommonSessionID)
            {
                return new SessionIdentity() { SessionID=CommonSessionID, UserID=0 };
            }
            SessionIdentity si = new SessionIdentity();
            if (_SessionPools.TryGetValue(sessionid, out si))
            {
                return si;
            }
            else
                return null;
        }


        public static string CommonSessionID = "ee6f1ec59d854b1a9921f35d32f17048";


        public static string AddSessionIdentity(UserSessionEntity user)
        {
            SessionIdentity si = new SessionIdentity();
            si.CreatedTime = DateTime.Now;
            si.SessionID = Guid.NewGuid().ToString("N");
            si.UserID = user.UserID;
            si.ZoneID = user.ZoneID;
            if (_SessionPools.TryAdd(si.SessionID, si))
            {
                return si.SessionID;
            }
            else
                return string.Empty;
        }

        public static bool RemoveSessionIdentity(string sessionid)
        {
            SessionIdentity si = new SessionIdentity();
            return _SessionPools.TryRemove(sessionid, out si);
        }

        private static void RemoveExpiredSession()
        {
            DateTime ep = DateTime.Now.AddMonths(-3); //把3个月前的session 全部干掉。
            List<string> toremove = new List<string>();
            foreach (var key in _SessionPools)
            {
                if (key.Value.CreatedTime <= ep)
                {
                    toremove.Add(key.Key);
                }
            }
            SessionIdentity si = new SessionIdentity();
            foreach (string k in toremove)
            {
                 //log 
                if (_SessionPools[k] == null)
                {
                    marsLog.InfoFormat(string.Format("缓存中不存在此用户信息:SessionId:{0}", k));
                }
                else 
                {
                    _SessionPools.TryRemove(k, out si);
                    marsLog.InfoFormat(string.Format("用户移除统计：SessionId:{0}  UserId:{1}  CreatedTime:{2}",si.SessionID,si.UserID,si.CreatedTime.ToString()));
                }
            }
        }


        private static bool runningstate = false;

        public static void Start()
        {
            new System.Threading.Thread(() =>
            {
                if (File.Exists(SessionBagFile))
                {
                    _SessionPools = SerializeManager<ConcurrentDictionary<string, SessionIdentity>>.Deserialize(SessionBagFile);
                    if (_SessionPools == null)
                    {
                        _SessionPools = new ConcurrentDictionary<string, SessionIdentity>();
                    }
                }
                runningstate = true;
                int tick = 0;
                int tick2 = 0;
                while (runningstate)
                {
                    System.Threading.Thread.Sleep(1000);
                    tick += 1000;
                    tick2 += 1000;
                    if (tick / 1000 >= 5 * 60) //5分钟 写入磁盘一次
                    {
                        SaveSessionToDisk();
                        tick = 0;
                    }

                    //test
                    if (true) 
                    {
                        RemoveExpiredSession();
                    }
                    //end test

                    if (tick2 / 1000 >= 3600*24) //24个小时清理一次过期session
                    {
                        RemoveExpiredSession();
                        tick2 = 0;
                    }
                }
            }).Start();

        }

        private static void SaveSessionToDisk()
        {
            SerializeManager<ConcurrentDictionary<string, SessionIdentity>>.Serialize(SessionBagFile, _SessionPools);
        }

        public static void Stop()
        {
            runningstate = false;
            SaveSessionToDisk();
        }

        /// <summary>
        /// EmailTiming 邮件定时提醒  2016-01-04 08:20:00 发送完以后 销毁
        /// </summary>
        public static void EmailTiming() 
        {
                DateTime? sendTime = DateTime.Parse("2016-01-07 08:20:00");
                new System.Threading.Thread(() =>
                {
                    while (true)
                    {
                        if (DateTime.Now < sendTime)
                        {
                            //5分钟
                            System.Threading.Thread.Sleep(60 * 1000 * 5);
                        }
                        else 
                        {
                            BCtrl_UserMail bllMail = new BCtrl_UserMail();
                            List<CompanyEmailAccountEntity> mailList = bllMail.QueryCoEmailAccountList();
                            if (mailList == null || mailList.Count == 0)
                            {
                                break;
                            }
                            OperationResult<IList<ExhibitionEmailEntity>> result =
                                                    BCtrl_ExhibitionEmail.Instance.ExhibitionEmail_GetWhere(" AND SendTypeID =0 ");
                            if(result.ResultType==OperationResultType.Success)
                            {
                                var entitys=result.AppendData;
                                entitys = entitys.GroupBy(p => new { p.CustomerEmail }).Select(g => g.First()).ToList();

                                for (int i = 0; i < entitys.Count; i++)
                                {
                                    //随机挑出一个服务器地址
                                    Random rd = new Random();
                                    int num = rd.Next(0, mailList.Count - 1);
                                    SendEmailEntity sendEntity = new SendEmailEntity();

                                    #region 发送邮件固定参数
                                    CompanyEmailAccountEntity cea = mailList[num];
                                    sendEntity.FromEmail = cea.EmailName;
                                    sendEntity.FromSendName = "开卷网络";
                                    sendEntity.FromPassword = cea.EmailPassword;

                                    sendEntity.ToSendName = entitys[i].CustomerName;
                                    sendEntity.ToEmail = entitys[i].CustomerEmail;
                                    sendEntity.Subject = "2016年北京图书订货会电子书目——由北京开卷收集整理";
                                    sendEntity.Body = "基于2016年图书订货会，这是开卷搜集并整理订货会参展商的电子书目。<br />";
                                    sendEntity.Body += "下载链接: http://pan.baidu.com/s/1bpjF5c ";
                                    sendEntity.Body += "密码: ijex   <br />";
                                    sendEntity.Body += "开卷日历APP——网络书业人和事。<br />";
                                    sendEntity.Body += "<img src='http://7xkwie.com2.z0.glb.qiniucdn.com/20151201/e3834448-e386-45b8-93cc-01a92744e5e8.jpg' alt='扫一扫' ></img>";
                                    #endregion 

                                    bool state = SendEmail.Send(sendEntity);
                                    if (state)
                                    {
                                        //单条发送时，发送后改变发送状态
                                        entitys[i].SendTypeID = 1;
                                        BCtrl_ExhibitionEmail.Instance.ExhibitionEmail_UpdateSendTypeID(entitys[i].CustomerEmail,entitys[i].SendTypeID);
                                    }
                                   
                                }
                            }
                            
                            break;
                        }
                    }
                }).Start();
           
        }
    }
    [Serializable]
    public class SessionIdentity
    {
        public DateTime CreatedTime { get; set; }
        public int UserID { get; set; }
        public string SessionID { get; set; }
        public string Version { get; set; }
        public int ZoneID { get; set; }
        public string AppKey { get; set; }
    }
}
