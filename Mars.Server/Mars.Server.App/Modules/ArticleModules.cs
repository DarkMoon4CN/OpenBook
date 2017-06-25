using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.App.Modules
{
    public class ArticleModules : ModuleBase
    {
        
        public ArticleModules()
            : base("/Article")
        {
            #region  添加评论
            Post["AddReviews"] = _ =>
            {
                try
                {
                    EventItemCommentEntity entity=new EventItemCommentEntity();
                    dynamic data = FetchFormData();
                    string contents = data.Contents;
                    int eventItemID = data.EventItemID;
                    bool isAnon = data.IsAnon;//是否匿名

                    entity.CommentContent = contents;
                    entity.EventItemID = eventItemID;
                    entity.IsAnonymous = isAnon;
                    entity.UserID = CurrentUser.UserID;
                    entity.ViewStateID = 1;

                    if (contents == null)
                    {
                        return JsonObj<JsonMessageBase<ReviewCommonEntity>>.ToJson(new JsonMessageBase<ReviewCommonEntity>()
                        {
                            Status = (int)OperationResultType.Error,
                            Msg ="评论内容不能为空！",
                            Value = new ReviewCommonEntity()
                        });
                    }
                    else if (contents.Length > 1000) 
                    {
                        return JsonObj<JsonMessageBase<ReviewCommonEntity>>.ToJson(new JsonMessageBase<ReviewCommonEntity>()
                        {
                            Status = (int)OperationResultType.Error,
                            Msg = "评论内容过长！",
                            Value = new ReviewCommonEntity()
                        });
                    }

                    OperationResult<ReviewCommonEntity> result = BCtrl_Article.Instance.ArticleComments_Insert(entity);
                    return JsonObj<JsonMessageBase<ReviewCommonEntity>>.ToJson(new JsonMessageBase<ReviewCommonEntity>() 
                                                {  Status = (int)result.ResultType, 
                                                   Msg = result.Message,
                                                   Value =result.AppendData });
                }
                catch (Exception ex) 
                {
                     LogUtil.WriteLog(ex);
                     return JsonObj<JsonMessageBase<ReviewCommonEntity>>.ToJson(new JsonMessageBase<ReviewCommonEntity>() 
                                                { Status = 0,
                                                  Msg = "数据提交不完全",
                                                  Value = new ReviewCommonEntity() });
                }
            };
            #endregion 

            #region  给文章点赞
            Get["ClickLikeArticle"] = _ =>
            {
                try
                {
                    EventItemLikeEntity entity = new EventItemLikeEntity();
                    dynamic data = FecthQueryData();
                    int eventItemID = data.EventItemID;
                    entity.EventItemID = eventItemID;
                    entity.UserID = CurrentUser.UserID;
                    entity.LikeCnt = 1;
                    OperationResult<ReviewCommonEntity> result = BCtrl_Article.Instance.ArticleLike_Insert(entity);

                    return JsonObj<JsonMessageBase<ReviewCommonEntity>>.ToJson(new JsonMessageBase<ReviewCommonEntity>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.Message,
                        Value = result.AppendData
                    });
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<ReviewCommonEntity>>.ToJson(new JsonMessageBase<ReviewCommonEntity>()
                    {
                        Status = 0,
                        Msg = "数据提交不完全",
                        Value = new ReviewCommonEntity()
                    });
                }

            };
            #endregion

            #region 回复列表
            Get["ReplyList"] = _ =>
            {
                try
                {
                    dynamic data = FecthQueryData();
                    int  reviewsId = data.ReviewsId;
                    int  replyId = data.ReplyId;
                    bool isHeader = data.IsHeader;
                    int userid = CurrentUser.UserID;
                    int pageSize = data.PageSize == null ? 20 : data.PageSize;
                    int pageIndex = data.PageIndex == null ? 1 : data.PageIndex;
                    if (pageSize > 50)
                    {
                        pageSize = 50;
                    }
                    OperationResult<ReplyHeaderEntity> harder = null;
                    
                    if (isHeader)//是否返回回复头信息
                    {
                        harder = BCtrl_Article.Instance.ArticleReplyHeader_Get(reviewsId,userid);
                    }
                    
                    //回复列表
                    OperationResult<List<EventItemReplyEntity>> result = BCtrl_Article.Instance.ArticleReply_GetList(reviewsId, replyId, pageIndex, pageSize);
                    if (result.ResultType == OperationResultType.Success)
                    {
                        var jsonBase = new JsonMessageBase<ReplyHeaderEntity, List<EventItemReplyEntity>>();
                        jsonBase.Status = 1;
                        jsonBase.Msg = "回复抓取完成！";
                        jsonBase.Value = harder==null ? null : harder.AppendData;
                        jsonBase.Value2 =result==null ?null : result.AppendData;
                        return JsonObj<JsonMessageBase<ReplyHeaderEntity, List<EventItemReplyEntity>>>.ToJson(jsonBase);
                    }
                    else 
                    {
                        var jsonBase = new JsonMessageBase<ReplyHeaderEntity, List<EventItemReplyEntity>>();
                        jsonBase.Status = 0;
                        jsonBase.Msg = "回复数据抓取异常！";
                        jsonBase.Value = harder == null ? null : harder.AppendData;
                        jsonBase.Value2 = result == null ? null : result.AppendData;
                        return JsonObj<JsonMessageBase<ReplyHeaderEntity, List<EventItemReplyEntity>>>.ToJson(jsonBase);
                    }
                }
                catch (Exception ex) 
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "数据提交不完全" });
                }
            };

            #endregion

            #region  添加回复
            Post["AddReply"] = _ =>
            {
                try
                {
                    EventItemReplyEntity entity = new EventItemReplyEntity();
                    dynamic data = FetchFormData();
                    string contents = data.Contents;
                    int rid = data.ReviewsId;
                    int toReplyID = data.ToReplyID;
                    bool isAnon = data.IsAnon;

                    entity.ReplyContent = contents;
                    entity.CommentID = rid;
                    entity.ReplyParentID = toReplyID;
                    entity.IsAnonymous = isAnon;
                    entity.ViewStateID = 1;
                    entity.UserID = CurrentUser.UserID;
                    if (contents == null)
                    {
                        return JsonObj<JsonMessageBase<AddReplyCommonEntity>>.ToJson(new JsonMessageBase<AddReplyCommonEntity>()
                        {
                            Status = (int)OperationResultType.Error,
                            Msg = "评论内容不能为空！",
                            Value = new AddReplyCommonEntity()
                        });
                    }
                    else if (contents.Length > 1000)
                    {
                        return JsonObj<JsonMessageBase<AddReplyCommonEntity>>.ToJson(new JsonMessageBase<AddReplyCommonEntity>()
                        {
                            Status = (int)OperationResultType.Error,
                            Msg = "评论内容过长！",
                            Value = new AddReplyCommonEntity()
                        });
                    }

                    OperationResult<AddReplyCommonEntity> result = BCtrl_Article.Instance.ArticleReply_Insert(entity);
                    return JsonObj<JsonMessageBase<AddReplyCommonEntity>>.ToJson(new JsonMessageBase<AddReplyCommonEntity>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.Message,
                        Value = result.AppendData
                    });
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<AddReplyCommonEntity>>.ToJson(new JsonMessageBase<AddReplyCommonEntity>()
                    {
                        Status = 0,
                        Msg = "数据提交不完全",
                        Value = new AddReplyCommonEntity()
                    });
                }
            };
            #endregion

            #region 给评论点赞
            Get["ClickLikeReview"] = _ =>
            {
                try
                {
                    CommentLikeEntity entity = new CommentLikeEntity();
                    dynamic data = FecthQueryData();
                    int rid = data.ReviewsId;
                    entity.CommentID = rid;
                    entity.UserID = CurrentUser.UserID;
                    entity.LikeCnt = 1;
                    OperationResult<ReplyCommonEntity> result = BCtrl_Article.Instance.CommentsLike_Insert(entity);
                    return JsonObj<JsonMessageBase<ReplyCommonEntity>>.ToJson(new JsonMessageBase<ReplyCommonEntity>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.Message,
                        Value = result.AppendData
                    });
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<ReviewCommonEntity>>.ToJson(new JsonMessageBase<ReviewCommonEntity>()
                    {
                        Status = 0,
                        Msg = "数据提交不完全",
                        Value = new ReviewCommonEntity()
                    });
                }
            };
            #endregion

            #region 无用户下的 文章点赞数及评论数
            Get["ArticleCommon"] = _ =>
            {
                try
                {
                    EventItemCommentEntity entity = new EventItemCommentEntity();
                    dynamic data = FecthQueryData();
                    int eid = data.EventItemID;
                    entity.EventItemID = eid;
                    entity.UserID = CurrentUser==null ? 0: CurrentUser.UserID;
                    OperationResult<ReviewCommonEntity> result = BCtrl_Article.Instance.ReviewCommon_Get(entity);
                    return JsonObj<JsonMessageBase<ReviewCommonEntity>>.ToJson(new JsonMessageBase<ReviewCommonEntity>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.Message,
                        Value = result.AppendData
                    });
                }
                catch (Exception ex) 
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<ReviewCommonEntity>>.ToJson(new JsonMessageBase<ReviewCommonEntity>()
                    {
                        Status = 0,
                        Msg = "数据提交不完全",
                        Value = new ReviewCommonEntity()
                    });
                }
            };
            #endregion 

            #region 有用户下的 文章点赞数及评论数
            Get["UserArticleCommon"] = _ =>
            {
                try
                { 
                    EventItemCommentEntity entity = new EventItemCommentEntity();
                    dynamic data = FecthQueryData();
                    int eid = data.EventItemID;
                    entity.EventItemID = eid;
                    entity.UserID = CurrentUser == null ? 0 : CurrentUser.UserID;
                    OperationResult<ReviewCommonEntity> result = BCtrl_Article.Instance.ReviewCommon_Get(entity);
                    return JsonObj<JsonMessageBase<ReviewCommonEntity>>.ToJson(new JsonMessageBase<ReviewCommonEntity>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.Message,
                        Value = result.AppendData
                    });
                }
                catch (Exception ex)
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<ReviewCommonEntity>>.ToJson(new JsonMessageBase<ReviewCommonEntity>()
                    {
                        Status = 0,
                        Msg = "数据提交不完全",
                        Value = new ReviewCommonEntity()
                    });
                }
            };
            #endregion 

            #region  2016-01-05 重构文章列表
            Post["List"] = _ =>
            {
                try
                {
                    dynamic data = FetchFormData();
                    int aid = data.ArticleId;
                    int pagesize = 10;
                    int pageno = 1;
                    if (data != null)
                    {
                        pagesize = data.pagesize == null ? 10 : data.pagesize;
                        pageno = data.pageno == null ? 1 : data.pageno;
                    }
                    if (pagesize > 50)
                    {
                        pagesize = 50;
                    }
                    BCtrl_EventItem eventitemobj = new BCtrl_EventItem();
                    List<EventItemGroupEntity> lists = eventitemobj.QueryAllEventItemGroups(pageno, pagesize);
                    OperationResult<dynamic> result = new OperationResult<dynamic>(OperationResultType.Success, "OK");
                    return JsonObj<JsonMessageBase<object>>.ToJson(new JsonMessageBase<object>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.Message,
                        Value = new PictureServerEntity()
                    });
                }
                catch (Exception ex) 
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "数据提交不完全" });
                }
            };


            #endregion 

            #region  评论举报
            Post["Report"] = _ =>
            {
                try
                {
                    dynamic data = FetchFormData();
                    int reportInfoTypeID = data.TagType;
                    int reportInfoID = data.Tag;
                    int userId = CurrentUser.UserID;
                    ReportEntity entity = new ReportEntity();
                    entity.ReportInfoTypeID = reportInfoTypeID;
                    entity.ReportInfoID = reportInfoID;
                    entity.FromUserID = userId;
                    entity.ReportTypeID = 1; //举报类型
                    OperationResult<bool> result = BCtrl_Article.Instance.Report_Insert(entity);
                    return JsonObj<JsonMessageBase<bool>>.ToJson(new JsonMessageBase<bool>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.Message,
                        Value = result.AppendData
                    });
                }
                catch (Exception ex) 
                {
                    LogUtil.WriteLog(ex);
                    return JsonObj<JsonMessageBase<bool>>.ToJson(new JsonMessageBase<bool>()
                    {
                        Status = 0,
                        Msg = "数据提交不完全",
                        Value = false
                    });
                }
            };
            #endregion 

        }
    }
}
