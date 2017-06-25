using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mars.Server.BLL;
using Mars.Server.Entity;
using System.Threading;
using Mars.Server.Utils;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mime;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Web;
using System.Web.Caching;

namespace Mars.Server.BLL
{
    /// <summary>
    /// 邮件管理类
    /// </summary>
    public class MailManager : IMailManager
    {
        private static MultiTaskSendMail sendMailObj;
        static MailManager()
        {

        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            if (sendMailObj == null)
            {
                sendMailObj = new MultiTaskSendMail();
            }
            sendMailObj.Start();
        }

        /// <summary>
        /// 终止
        /// </summary>
        public void Stop()
        {
            if (sendMailObj != null)
            {
                sendMailObj.Stop();
            }
        }
    }

    internal class MultiTaskSendMail : IMailManager
    {
        ConcurrentQueue<UserMailViewEntity> mailQueue;
        int successMailNum; //发送邮件数量

        CancellationTokenSource cts;
        static object obj = new object();
        static object cacheobj = new object();
        static object cacheobj2 = new object();
        BCtrl_UserMail bllMail;
        Task[] mailSendThread;
        string sendName;
        string attachDomain;
        int sendIntervalMinute;
        int cacheTimeoutMinute;
        readonly string cacheTimeoutName;
        Regex mailFormatReg = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
        List<CompanyEmailAccountEntity> companyEmailAccountList { get; set; }
        public MultiTaskSendMail()
        {
            successMailNum = 0;
            mailQueue = new ConcurrentQueue<UserMailViewEntity>();
            bllMail = new BCtrl_UserMail();
            sendName = System.Configuration.ConfigurationManager.AppSettings["Mail_SendName"];
            attachDomain = System.Configuration.ConfigurationManager.AppSettings["Mail_AttachPathDir"];            
            sendIntervalMinute = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Mail_SendIntervalMinute"]) * 60 * 1000;
            cacheTimeoutMinute = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Mail_CacheTimeoutMinute"]) * 60;
            cacheTimeoutName = "MarsMailServer";
            InitMailThread();
        }

        #region 邮件发送公开方法
        /// <summary>
        /// 开始发送邮件
        /// </summary>
        public void Start()
        {
            try
            {
                if (mailSendThread == null && cts == null)
                {
                    InitMailThread();
                }

                for (int loop = 0; loop < mailSendThread.Length; loop++)
                {
                    if (mailSendThread[loop].Status == TaskStatus.Created)
                    {
                        mailSendThread[loop].Start();
                        LogUtilExpand.WriteLog("线程" + (loop + 1) + "开始启动");
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtilExpand.WriteLog("邮件服务启动失败（异常信息如下)");
                LogUtilExpand.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// 停止发送邮件
        /// </summary>
        public void Stop()
        {
            int stopThreadNum = 0;

            if (mailSendThread != null && cts != null)
            {
                LogUtilExpand.WriteLog("邮件服务尝试终止...");
                cts.Cancel();
                for (int loop = 0; loop < mailSendThread.Length; loop++)
                {
                    try
                    {
                        mailSendThread[loop].Wait();
                    }
                    catch (AggregateException ex)
                    {
                        ex.Handle(e => e is OperationCanceledException);
                        LogUtilExpand.WriteLog("线程" + (loop + 1) + "成功终止");
                        stopThreadNum++;
                    }
                    catch (Exception ex)
                    {
                        LogUtilExpand.WriteLog("线程" + (loop + 1) + "终止失败(失败原因如下：)");
                        LogUtilExpand.WriteLog(ex);
                    }
                }

                if (successMailNum > 0)
                {
                    LogUtilExpand.WriteLog("邮件服务程序在终止前成功发送" + successMailNum + "封邮件");
                }

                if (stopThreadNum == mailSendThread.Length)
                {
                    mailSendThread = null;
                    cts = null;
                    successMailNum = 0;
                    LogUtilExpand.WriteLog("线程已全部终止");
                }
                else
                {
                    LogUtilExpand.WriteLog("成功终止" + stopThreadNum + "个线程，但有" + (mailSendThread.Length - stopThreadNum) + "个线程终止失败");
                }
            }
        }

        #endregion

        #region 内部逻辑处理方法
        /// <summary>
        /// 初始化邮件发送线程
        /// </summary>
        private void InitMailThread()
        {
            List<CompanyEmailAccountEntity> emailAccountList = this.GetCompanyEmailAccountList();
            companyEmailAccountList = emailAccountList;

            if (emailAccountList != null)
            {
                cts = new CancellationTokenSource();
                int looptimes = emailAccountList.Count;
                mailSendThread = new Task[looptimes];

                for (int loop = 0; loop < looptimes; loop++)
                {
                    mailSendThread[loop] = new Task(() => StartSendEmail(cts.Token, 1), cts.Token);
                }

                LogUtilExpand.WriteLog("从数据库获取" + emailAccountList.Count + "个公司邮箱账号,接下来将开启" + emailAccountList.Count + "个邮件发送线程，" + (sendIntervalMinute / 60 / 1000) + "分钟后将开始发送邮件");
            }
            else
            {
                LogUtilExpand.WriteLog("（异常）：未设置公司邮箱账号");
            }
        }

        /// <summary>
        /// 发送邮件方法
        /// </summary>
        /// <param name="ct"></param>
        /// <param name="threadNum"></param>
        /// <returns></returns>
        private void StartSendEmail(CancellationToken ct, int threadNum)
        {
            UserMailViewEntity mailEntity;
            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    ct.ThrowIfCancellationRequested();
                }
                else
                {
                    //如果邮件队列为空，休眠指定时间，尝试获取邮件列表
                    if (mailQueue.Count == 0)
                    {                        
                        if (successMailNum > 0)
                        {
                            int count = successMailNum;
                            Interlocked.Add(ref successMailNum, -count);
                            LogUtilExpand.WriteLog("邮件服务程序成功发送" + count + "封邮件");
                        }
                        //写这么多，主要是终止服务时，尽可能早的检测到
                        if (ct.IsCancellationRequested)
                        {
                            ct.ThrowIfCancellationRequested();
                        }
                        Thread.Sleep(sendIntervalMinute);
                        if (ct.IsCancellationRequested)
                        {
                            ct.ThrowIfCancellationRequested();
                        }
                        GetWaitSendMail();
                    }
                    if (ct.IsCancellationRequested)
                    {
                        ct.ThrowIfCancellationRequested();
                    }
                    if (mailQueue.TryDequeue(out mailEntity))
                    {
                        SendMailMessage(mailEntity, companyEmailAccountList[threadNum]);
                    }
                }
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="attachmentPath"></param>
        /// <param name="email"></param>
        public void SendMailMessage(UserMailViewEntity mailEntity, CompanyEmailAccountEntity emailAccountEntity)
        {
            string remark = string.Empty;

            #region 发送前错误判断 (提前判断错误，会很大提升性能)
            if (string.IsNullOrEmpty(mailEntity.EMail))
            {
                remark = "当前用户没有填写邮箱";
                bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
                return;
            }

            if (!mailFormatReg.IsMatch(mailEntity.EMail))
            {
                remark = "当前用户邮箱格式错误";
                bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
                return;
            }

            if (string.IsNullOrEmpty(mailEntity.BookListPath))
            {
                remark = "当前所属文章附件没有设置文件路径";
                bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
                return;
            }
            #endregion

            MailMessage message = null;
            SmtpClient client = null;
            Attachment myAttachment = null;
            try
            {
                //设置 收件人和发件人地址
                MailAddress from = new MailAddress(emailAccountEntity.EmailName, sendName);
                MailAddress to = new MailAddress(mailEntity.EMail, mailEntity.LoginName);

                message = new MailMessage(from, to);

                string attachPath = attachDomain + mailEntity.BookListPath;
                if (File.Exists(attachPath))
                {
                    FileInfo fileInfo = new FileInfo(attachPath);
                    myAttachment = new Attachment(attachPath, MediaTypeNames.Application.Octet);
                    //MIME协议下的一个对象，用以设置附件的创建时间，修改时间以及读取时间  
                    System.Net.Mime.ContentDisposition disposition = myAttachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachPath);
                    disposition.ModificationDate = File.GetLastWriteTime(attachPath);
                    disposition.ReadDate = File.GetLastAccessTime(attachPath);
                    disposition.FileName = fileInfo.Name; ;
                }
                else
                {
                    remark = "该附件存在";
                    bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
                    return;
                }

                message.Attachments.Add(myAttachment);
                message.Subject = "开卷网络附带书单";
                message.Body = "书单测试主体";
                message.BodyEncoding = Encoding.GetEncoding("gb2312");
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;

                client = new SmtpClient();
                client.Timeout = 100000;
                client.Host = emailAccountEntity.EmailSMTPHost;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(emailAccountEntity.EmailName, emailAccountEntity.EmailPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(message);

                remark = "发送成功";
                bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendSuccess);
                Interlocked.Increment(ref successMailNum);
            }
            catch (Exception ex)
            {
                remark = "发送失败";
                bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
                LogUtilExpand.WriteLog("邮件ID：" + mailEntity.MailID + "发送失败(异常信息如下：)");
                LogUtilExpand.WriteLog(ex);

                if (message != null)
                    message.Dispose();

                if (client != null)
                    client.Dispose();
            }
        }

        /// <summary>
        /// 获取待发邮件列表
        /// </summary>
        private void GetWaitSendMail()
        {
            List<UserMailViewEntity> mailList;
            if (mailQueue.Count == 0 && !IsExistsMailCacheName()) 
            {
                lock (obj)
                {               
                    if (mailQueue.Count == 0 && !IsExistsMailCacheName())
                    {
                        LogUtilExpand.WriteLog("尝试从数据库提取待发邮件列表...");
                        mailList = bllMail.QueryViewWaitSendMailList();

                        if (mailList != null)
                        {
                            LogUtilExpand.WriteLog("从数据库取出" + mailList.Count + "封发邮件，成功放入邮件发送队列");
                            foreach (UserMailViewEntity entity in mailList)
                            {
                                mailQueue.Enqueue(entity);
                            }
                        }
                        else
                        {
                            LogUtilExpand.WriteLog("从数据库取出0封邮件");
                            SetAbsoluteExpirationCache(cacheTimeoutName, 1, cacheTimeoutMinute);
                        }
                    }                  
                }
            }
        }

        /// <summary>
        /// 获取公司邮箱账号
        /// </summary>
        /// <returns></returns>
        private List<CompanyEmailAccountEntity> GetCompanyEmailAccountList()
        {
            List<CompanyEmailAccountEntity> mailList = bllMail.QueryCoEmailAccountList();           
            return mailList;
        }

         /// <summary>
       /// 设定绝对的过期时间
       /// </summary>
       /// <param name="CacheKey"></param>
       /// <param name="objObject"></param>
       /// <param name="seconds">超过多少秒后过期</param>
       private void SetAbsoluteExpirationCache(string CacheKey, object obj, int seconds) 
       {
           MyCache<string>.Insert(CacheKey, "1", seconds/5);
           //Cache objCache = HttpRuntime.Cache;
           //if (objCache.Get(cacheTimeoutName) == null)
           //{
           //    lock (cacheobj)
           //    {
           //         if (objCache.Get(cacheTimeoutName) == null)
           //         {
           //             objCache.Insert(CacheKey, obj, null, System.DateTime.Now.AddSeconds(Seconds), TimeSpan.Zero);
           //         }
           //    }
           //}     
       }

        /// <summary>
        /// 判断邮件缓存名是否过期
        /// 没过期 邮件线程不操作数据库继续休眠
        /// 过期 邮件线程开始尝试从数据库读取未发邮件
        /// </summary>
        /// <returns></returns>
       private bool IsExistsMailCacheName(bool w=true)
       {      
           return !string.IsNullOrEmpty(MyCache<string>.Get(cacheTimeoutName));
          
           //Cache objCache = HttpRuntime.Cache;
           //if (objCache.Get(cacheTimeoutName) != null)
           //{
           //    lock (cacheobj2)
           //    {
           //          if (objCache.Get(cacheTimeoutName) != null)
           //          {
           //              return true;
           //          }
           //    }
           //}
           //return false;
       }
        #endregion
    }

    public interface IMailManager
    {
        /// <summary>
        /// 开始发送邮件
        /// </summary>
        void Start();

        /// <summary>
        /// 停止邮件发送
        /// </summary>
        void Stop();
    }

    #region 作废
    ///// <summary>
    ///// 邮件管理类
    ///// </summary>
    //public class MailManager
    //{
    //    static Thread threadProd;
    //    static Thread threadConsumer;
    //    static MailFactory mailFactory;

    //    static MailManager()
    //    {
    //        mailFactory = new MailFactory();

    //        threadProd = new Thread(new ThreadStart(MailProducerThread));
    //        threadProd.IsBackground = true;

    //        threadConsumer = new Thread(new ThreadStart(MailConsumerThread));
    //        threadConsumer.IsBackground = true;
    //    }

    //    /// <summary>
    //    /// 开始
    //    /// </summary>
    //    public void Start()
    //    {
    //        threadProd.Start();
    //        threadConsumer.Start();
    //    }

    //    /// <summary>
    //    /// 终止
    //    /// </summary>
    //    public void Stop()
    //    {
    //        if (threadProd != null && (threadProd.ThreadState != ThreadState.Aborted || threadProd.ThreadState != ThreadState.Stopped))
    //        {
    //            threadProd.Abort();
    //            threadProd.Join();

    //            threadProd = new Thread(new ThreadStart(MailProducerThread));
    //            threadProd.IsBackground = true;
    //        }


    //        if (threadConsumer != null && (threadConsumer.ThreadState != ThreadState.Aborted || threadConsumer.ThreadState != ThreadState.Stopped))
    //        {
    //            threadConsumer.Abort();
    //            threadConsumer.Join();

    //            threadConsumer = new Thread(new ThreadStart(MailConsumerThread));
    //            threadConsumer.IsBackground = true;
    //        }
    //    }

    //    /// <summary>
    //    /// 邮件生产线程
    //    /// </summary>
    //    private static void MailProducerThread()
    //    {
    //        while (true)
    //        {
    //            mailFactory.MailProducer();
    //        }
    //    }

    //    /// <summary>
    //    /// 邮件消费线程
    //    /// </summary>
    //    private static void MailConsumerThread()
    //    {
    //        while (true)
    //        {
    //            mailFactory.MailConsumer();
    //        }
    //    }
    //}
    ///// <summary>
    ///// 多线程邮件操作
    ///// </summary>
    //internal class MultiThreadSendMail
    //{
    //    ConcurrentDictionary<int, UserMailViewEntity> mailDic;
    //    static object obj = new object();
    //    BCtrl_UserMail bllMail;
    //    Thread[] mailSendThread;
    //    string sendName;
    //    string attachDomain;
    //    int sendIntervalMinute;
    //    Regex mailFormatReg = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
    //    List<CompanyEmailAccountEntity> companyEmailAccountList { get; set; }

    //    public MultiThreadSendMail()
    //    {
    //        bllMail = new BCtrl_UserMail();
    //        mailDic = new ConcurrentDictionary<int, UserMailViewEntity>();
    //        sendName = System.Configuration.ConfigurationManager.AppSettings["Mail_SendName"];
    //        attachDomain = System.Configuration.ConfigurationManager.AppSettings["Mail_AttachPathDir"];
    //        sendIntervalMinute = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Mail_SendIntervalMinute"]) * 60 * 1000;
    //        InitMailThread();
    //    }

    //    /// <summary>
    //    /// 开始发送邮件
    //    /// </summary>
    //    public void StartSend()
    //    {
    //        foreach (Thread mailThread in mailSendThread)
    //        {
    //            mailThread.Start();
    //        }
    //    }

    //    /// <summary>
    //    /// 停止发送邮件
    //    /// </summary>
    //    public void StopSend()
    //    {
    //        foreach (Thread mailThread in mailSendThread)
    //        {
    //            if (mailThread.ThreadState != ThreadState.Aborted || mailThread.ThreadState != ThreadState.Stopped)
    //            {
    //                mailThread.Abort();
    //                mailThread.Join();

    //                InitMailThread();
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 初始化邮件发送线程
    //    /// </summary>
    //    private void InitMailThread()
    //    {
    //        List<CompanyEmailAccountEntity> emailAccountList = this.GetCompanyEmailAccountList();
    //        companyEmailAccountList = emailAccountList;

    //        if (emailAccountList != null)
    //        {
    //            int looptimes = emailAccountList.Count >= 10 ? 10 : emailAccountList.Count; //到多同时运行10个线程
    //            mailSendThread = new Thread[looptimes];
    //            ThreadStart threadHandler = null;

    //            #region 作废
    //            //for (int loop = 0; loop < looptimes; loop++)
    //            //{
    //            //    switch (loop)
    //            //    {
    //            //        case 0:
    //            //            threadHandler = new ThreadStart(SendMailThread0);
    //            //            break;
    //            //        case 1:
    //            //            threadHandler = new ThreadStart(SendMailThread1);
    //            //            break;
    //            //        case 2:
    //            //            threadHandler = new ThreadStart(SendMailThread2);
    //            //            break;
    //            //        case 3:
    //            //            threadHandler = new ThreadStart(SendMailThread3);
    //            //            break;
    //            //        case 4:
    //            //            threadHandler = new ThreadStart(SendMailThread4);
    //            //            break;
    //            //        case 5:
    //            //            threadHandler = new ThreadStart(SendMailThread5);
    //            //            break;
    //            //        case 6:
    //            //            threadHandler = new ThreadStart(SendMailThread6);
    //            //            break;
    //            //        case 7:
    //            //            threadHandler = new ThreadStart(SendMailThread7);
    //            //            break;
    //            //        case 8:
    //            //            threadHandler = new ThreadStart(SendMailThread8);
    //            //            break;
    //            //        default:
    //            //            threadHandler = new ThreadStart(SendMailThread9);
    //            //            break;
    //            //    }
    //            //    mailSendThread[loop] = new Thread(threadHandler);
    //            //    mailSendThread[loop].IsBackground = true;
    //            //}
    //            #endregion
    //        }
    //    }

    //    /// <summary>
    //    /// 获取待发邮件列表
    //    /// </summary>
    //    private void GetWaitSendMail()
    //    {
    //        List<UserMailViewEntity> mailList;
    //        if (mailDic.Count == 0)
    //        {
    //            lock (obj)
    //            {
    //                if (mailDic.Count == 0)
    //                {
    //                    mailList = bllMail.QueryViewWaitSendMailList();

    //                    if (mailList != null)
    //                    {
    //                        foreach (UserMailViewEntity entity in mailList)
    //                        {
    //                            mailDic.TryAdd(entity.MailID, entity);
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    //#region 10个发送邮件线程
    //    //private void SendMailThread0()
    //    //{
    //    //    if (mailDic.Count == 0)
    //    //    {
    //    //        Thread.Sleep(sendIntervalMinute);
    //    //    }

    //    //}

    //    //private void SendMailThread1()
    //    //{

    //    //}

    //    //private void SendMailThread2()
    //    //{

    //    //}

    //    //private void SendMailThread3()
    //    //{

    //    //}

    //    //private void SendMailThread4()
    //    //{

    //    //}

    //    //private void SendMailThread5()
    //    //{

    //    //}

    //    //private void SendMailThread6()
    //    //{

    //    //}

    //    //private void SendMailThread7()
    //    //{

    //    //}

    //    //private void SendMailThread8()
    //    //{

    //    //}

    //    //private void SendMailThread9()
    //    //{

    //    //}

    //    ///// <summary>
    //    ///// 获取线程处理邮件列表
    //    ///// </summary>
    //    ///// <returns></returns>
    //    //private List<UserMailViewEntity> GetThreadDealMailList(int threadNum, out CompanyEmailAccountEntity coEmailAccountEntity)
    //    //{
    //    //    coEmailAccountEntity = null;
    //    //    List<UserMailViewEntity> mailList = null;

    //    //    if (mailDic.Count > 0)
    //    //    {
    //    //        switch (threadNum)
    //    //        {
    //    //            case 0:
    //    //                coEmailAccountEntity = companyEmailAccountList[0];
    //    //                mailList = mailDic.Where(entity => entity.Key % 10 == 0).Select(entity => entity.Value).ToList();
    //    //                break;
    //    //            case 1:
    //    //                coEmailAccountEntity = companyEmailAccountList[1];
    //    //                mailList = mailDic.Where(entity => entity.Key % 10 == 1).Select(entity => entity.Value).ToList();
    //    //                break;
    //    //            case 2:
    //    //                coEmailAccountEntity = companyEmailAccountList[2];
    //    //                mailList = mailDic.Where(entity => entity.Key % 10 == 2).Select(entity => entity.Value).ToList();
    //    //                break;
    //    //            case 3:
    //    //                coEmailAccountEntity = companyEmailAccountList[3];
    //    //                mailList = mailDic.Where(entity => entity.Key % 10 == 3).Select(entity => entity.Value).ToList();
    //    //                break;
    //    //            case 4:
    //    //                coEmailAccountEntity = companyEmailAccountList[4];
    //    //                mailList = mailDic.Where(entity => entity.Key % 10 == 4).Select(entity => entity.Value).ToList();
    //    //                break;
    //    //            case 5:
    //    //                coEmailAccountEntity = companyEmailAccountList[5];
    //    //                mailList = mailDic.Where(entity => entity.Key % 10 == 5).Select(entity => entity.Value).ToList();
    //    //                break;
    //    //            case 6:
    //    //                coEmailAccountEntity = companyEmailAccountList[6];
    //    //                mailList = mailDic.Where(entity => entity.Key % 10 == 6).Select(entity => entity.Value).ToList();
    //    //                break;
    //    //            case 7:
    //    //                coEmailAccountEntity = companyEmailAccountList[7];
    //    //                mailList = mailDic.Where(entity => entity.Key % 10 == 7).Select(entity => entity.Value).ToList();
    //    //                break;
    //    //            case 8:
    //    //                coEmailAccountEntity = companyEmailAccountList[8];
    //    //                mailList = mailDic.Where(entity => entity.Key % 10 == 8).Select(entity => entity.Value).ToList();
    //    //                break;
    //    //            default:
    //    //                coEmailAccountEntity = companyEmailAccountList[9];
    //    //                mailList = mailDic.Where(entity => entity.Key % 10 == 9).Select(entity => entity.Value).ToList();
    //    //                break;
    //    //        }
    //    //    }

    //    //    return mailList;
    //    //}
    //    //#endregion

    //    /// <summary>
    //    /// 发送邮件
    //    /// </summary>
    //    /// <param name="content"></param>
    //    /// <param name="attachmentPath"></param>
    //    /// <param name="email"></param>
    //    public void SendMail(UserMailViewEntity mailEntity, CompanyEmailAccountEntity emailAccountEntity)
    //    {
    //        string remark = string.Empty;

    //        #region 发送前错误判断 (提前判断错误，会很大提升性能)
    //        if (string.IsNullOrEmpty(mailEntity.EMail))
    //        {
    //            remark = "当前用户没有填写邮箱";
    //            bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
    //            return;
    //        }

    //        if (!mailFormatReg.IsMatch(mailEntity.EMail))
    //        {
    //            remark = "当前用户邮箱格式错误";
    //            bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
    //            return;
    //        }

    //        if (string.IsNullOrEmpty(mailEntity.BookListPath))
    //        {
    //            remark = "当前所属文章附件没有设置文件路径";
    //            bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
    //            return;
    //        }
    //        #endregion

    //        MailMessage message = null;
    //        SmtpClient client = null;
    //        Attachment myAttachment = null;
    //        try
    //        {
    //            //设置 收件人和发件人地址
    //            MailAddress from = new MailAddress(emailAccountEntity.EmailName, sendName);
    //            MailAddress to = new MailAddress(mailEntity.EMail, mailEntity.LoginName);

    //            message = new MailMessage(from, to);

    //            string attachPath = attachDomain + mailEntity.BookListPath;
    //            if (File.Exists(attachPath))
    //            {
    //                FileInfo fileInfo = new FileInfo(attachPath);
    //                myAttachment = new Attachment(attachPath, MediaTypeNames.Application.Octet);
    //                //MIME协议下的一个对象，用以设置附件的创建时间，修改时间以及读取时间  
    //                System.Net.Mime.ContentDisposition disposition = myAttachment.ContentDisposition;
    //                disposition.CreationDate = File.GetCreationTime(attachPath);
    //                disposition.ModificationDate = File.GetLastWriteTime(attachPath);
    //                disposition.ReadDate = File.GetLastAccessTime(attachPath);
    //                disposition.FileName = fileInfo.Name; ;
    //            }
    //            else
    //            {
    //                remark = "该附件存在";
    //                bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
    //                return;
    //            }

    //            message.Attachments.Add(myAttachment);
    //            message.Subject = "开卷网络附带书单";
    //            message.Body = "书单测试主体";
    //            message.BodyEncoding = Encoding.GetEncoding("gb2312");
    //            message.IsBodyHtml = true;
    //            message.Priority = MailPriority.High;

    //            client = new SmtpClient();
    //            client.Timeout = 100000;
    //            client.Host = emailAccountEntity.EmailSMTPHost;
    //            client.UseDefaultCredentials = false;
    //            client.Credentials = new NetworkCredential(emailAccountEntity.EmailName, emailAccountEntity.EmailPassword);
    //            client.DeliveryMethod = SmtpDeliveryMethod.Network;

    //            client.Send(message);

    //            remark = "发送成功";
    //            bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendSuccess);
    //        }
    //        catch (Exception ex)
    //        {
    //            remark = "发送失败";
    //            bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);

    //            if (message != null)
    //                message.Dispose();

    //            if (client != null)
    //                client.Dispose();
    //        }
    //    }

    //    private List<CompanyEmailAccountEntity> GetCompanyEmailAccountList()
    //    {
    //        List<CompanyEmailAccountEntity> mailList = new List<CompanyEmailAccountEntity>();
    //        mailList.Add(new CompanyEmailAccountEntity { CompanyEmailAccountID = 1, EmailName = "admin@openbook.cn", EmailPassword = "human123", EmailSMTPHost = "smtp.qiye.163.com" });
    //        mailList.Add(new CompanyEmailAccountEntity { CompanyEmailAccountID = 2, EmailName = "admin@openbook.cn", EmailPassword = "human123", EmailSMTPHost = "smtp.qiye.163.com" });
    //        return mailList;
    //    }
    //}

    ///// <summary>
    ///// 邮件工厂
    ///// 1从数据库读取待发邮件
    ///// 2当检测到有邮件，执行发送操作
    ///// 3当发送完成后,根据配置文件设置等待时间后，步骤1重新执行
    ///// </summary>
    //internal class MailFactory
    //{

    //    private static List<UserMailViewEntity> mailList = null;
    //    BCtrl_UserMail bllMail = new BCtrl_UserMail();

    //    bool mailFlag = false; //状态标志 true时，可以消费  false时，正在生产
    //    static string smtp;
    //    static string uid;
    //    static string pwd;
    //    static string sendName;
    //    static string attachDomain;
    //    static int sendIntervalMinute;

    //    static MailFactory()
    //    {
    //        smtp = System.Configuration.ConfigurationManager.AppSettings["Mail_SMTP"];
    //        uid = System.Configuration.ConfigurationManager.AppSettings["Mail_UID"];
    //        pwd = System.Configuration.ConfigurationManager.AppSettings["Mail_PWD"];
    //        sendName = System.Configuration.ConfigurationManager.AppSettings["Mail_SendName"];
    //        attachDomain = System.Configuration.ConfigurationManager.AppSettings["Mail_AttachPathDir"];
    //        sendIntervalMinute = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Mail_SendIntervalMinute"]) * 60 * 1000;
    //    }

    //    /// <summary>
    //    /// 邮件生产者
    //    /// </summary>
    //    public void MailProducer()
    //    {
    //        lock (this)
    //        {
    //            #region 状态判断
    //            if (mailFlag)
    //            {
    //                try
    //                {
    //                    Monitor.Wait(this);
    //                }
    //                catch (SynchronizationLockException ex)
    //                {
    //                    //string errMsg = "邮件服务";
    //                    // LogUtil.WriteLog("");
    //                }
    //                catch (ThreadInterruptedException ex)
    //                {

    //                }
    //                catch (Exception ex)
    //                {

    //                }
    //            }
    //            #endregion

    //            //间隔一段指定时间
    //            Thread.Sleep(sendIntervalMinute);

    //            #region 从数据库读取待发邮件
    //            try
    //            {
    //                mailList = bllMail.QueryViewWaitSendMailList();

    //                if (mailList != null)
    //                {
    //                    mailFlag = true;
    //                    Monitor.Pulse(this); //通知消费者
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                //
    //            }
    //            #endregion
    //        }
    //    }

    //    /// <summary>
    //    /// 邮件消费者
    //    /// </summary>
    //    public void MailConsumer()
    //    {
    //        lock (this)
    //        {
    //            #region 状态判断
    //            if (!mailFlag)
    //            {
    //                try
    //                {
    //                    Monitor.Wait(this);
    //                }
    //                catch (SynchronizationLockException ex)
    //                {

    //                }
    //                catch (ThreadInterruptedException ex)
    //                {

    //                }
    //                catch (Exception ex)
    //                {

    //                }
    //            }
    //            #endregion

    //            #region 发送邮件
    //            if (mailList != null)
    //            {
    //                foreach (UserMailViewEntity item in mailList)
    //                {
    //                    SendMail(item);
    //                }
    //            }
    //            #endregion

    //            //测试用暂停10秒
    //            Thread.Sleep(10000);

    //            mailList = null;
    //            mailFlag = false;
    //            Monitor.Pulse(this);
    //        }
    //    }

    //    Regex mailFormatReg = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

    //    /// <summary>
    //    /// 发送邮件
    //    /// </summary>
    //    /// <param name="content"></param>
    //    /// <param name="attachmentPath"></param>
    //    /// <param name="email"></param>
    //    public void SendMail(UserMailViewEntity mailEntity)
    //    {
    //        string remark = string.Empty;

    //        #region 发送前错误判断 (提前判断错误，会很大提升性能)
    //        if (string.IsNullOrEmpty(mailEntity.EMail))
    //        {
    //            remark = "当前用户没有填写邮箱";
    //            bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
    //            return;
    //        }

    //        if (!mailFormatReg.IsMatch(mailEntity.EMail))
    //        {
    //            remark = "当前用户邮箱格式错误";
    //            bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
    //            return;
    //        }

    //        if (string.IsNullOrEmpty(mailEntity.BookListPath))
    //        {
    //            remark = "当前所属文章附件没有设置文件路径";
    //            bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
    //            return;
    //        }
    //        #endregion

    //        MailMessage message = null;
    //        SmtpClient client = null;
    //        Attachment myAttachment = null;
    //        try
    //        {
    //            //设置 收件人和发件人地址
    //            MailAddress from = new MailAddress(uid, sendName);
    //            MailAddress to = new MailAddress(mailEntity.EMail, mailEntity.LoginName);

    //            message = new MailMessage(from, to);

    //            string attachPath = attachDomain + mailEntity.BookListPath;
    //            if (File.Exists(attachPath))
    //            {
    //                FileInfo fileInfo = new FileInfo(attachPath);
    //                myAttachment = new Attachment(attachPath, MediaTypeNames.Application.Octet);
    //                //MIME协议下的一个对象，用以设置附件的创建时间，修改时间以及读取时间  
    //                System.Net.Mime.ContentDisposition disposition = myAttachment.ContentDisposition;
    //                disposition.CreationDate = File.GetCreationTime(attachPath);
    //                disposition.ModificationDate = File.GetLastWriteTime(attachPath);
    //                disposition.ReadDate = File.GetLastAccessTime(attachPath);
    //                disposition.FileName = fileInfo.Name; ;
    //            }
    //            else
    //            {
    //                remark = "该附件存在";
    //                bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);
    //                return;
    //            }

    //            message.Attachments.Add(myAttachment);
    //            message.Subject = "开卷网络附带书单";
    //            message.Body = "书单测试主体";
    //            message.BodyEncoding = Encoding.GetEncoding("gb2312");
    //            message.IsBodyHtml = true;
    //            message.Priority = MailPriority.High;

    //            //try
    //            //{
    //            //    message.To.Add(mailEntity.EMail);
    //            //}
    //            //catch
    //            //{
    //            //    remark = "当前用户邮箱是假邮箱";
    //            //    bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);

    //            //    if (message != null)
    //            //        message.Dispose();
    //            //    return;
    //            //}

    //            client = new SmtpClient();
    //            client.Timeout = 100000;
    //            client.Host = smtp;
    //            client.UseDefaultCredentials = false;
    //            client.Credentials = new NetworkCredential(uid, pwd);
    //            client.DeliveryMethod = SmtpDeliveryMethod.Network;

    //            client.Send(message);

    //            remark = "发送成功";
    //            bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendSuccess);
    //        }
    //        catch (Exception ex)
    //        {
    //            remark = "发送失败";
    //            bllMail.UpdateSendStatus(mailEntity.MailID, remark, UserMailStatus.SendFail);

    //            if (message != null)
    //                message.Dispose();

    //            if (client != null)
    //                client.Dispose();
    //        }
    //    }
    //}
    #endregion
}
