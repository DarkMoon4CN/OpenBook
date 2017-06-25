using Mars.Server.DAO;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mars.Server.BLL
{
    public class BCtrl_Article
    {
        private BCtrl_Article() { }
        private static BCtrl_Article _instance;
        public static BCtrl_Article Instance { get { return _instance ?? (_instance = new BCtrl_Article()); } }
        private static readonly ArticleDAO dao = new ArticleDAO();

        public OperationResult<ReviewCommonEntity> ArticleComments_Insert(EventItemCommentEntity entity)
        {
            return dao.ArticleComments_Insert(entity);
        }
        public OperationResult<ReviewCommonEntity> ArticleLike_Insert(EventItemLikeEntity entity) 
        {
            return dao.ArticleLike_Insert(entity);
        }

        public OperationResult<ReplyHeaderEntity> ArticleReplyHeader_Get(int reviewsId,int userId)
        {
            var result= dao.ArticleReplyHeader_Get(reviewsId,userId);
            
            if (string.IsNullOrEmpty(result.AppendData.UserPicture) == false)
            {
                result.AppendData.UserPicture = result.AppendData.UserPictureDomain + result.AppendData.UserPicture + AppUtil.ConvertJpg;
            }
            else 
            {
                result.AppendData.UserPicture = result.AppendData.ThirdPictureUrl;
             }
            if (result.AppendData.IsAnonymous == true)
            {
                result.AppendData.NickName = "匿名";
                result.AppendData.UserPicture = AppUtil.UserAnonymousHeader + AppUtil.ConvertJpg;
            }
            result.AppendData.EventItemPicture = result.AppendData.EventItemPictureDomain + result.AppendData.EventItemPicture + AppUtil.ConvertJpg;
            return result;
        }

        public OperationResult<List<EventItemReplyEntity>> ArticleReply_GetList(int reviewsId,int replyId,int pageIndex,int pageSize)
        {
                var result= dao.ArticleReply_GetList(reviewsId, replyId, pageIndex, pageSize);
                foreach (var item in result.AppendData)
                {
                    if (item.IsAnonymous == true)
                    {
                        item.FromNickName = "匿名";
                        item.UserPicture = AppUtil.UserAnonymousHeader + AppUtil.ConvertJpg;
                    }
                    else if (string.IsNullOrEmpty(item.UserPicture)==false)
                    {
                        item.UserPicture = item.UserPictureDomain + item.UserPicture + AppUtil.ConvertJpg;
                    }
                    else
                    {
                        item.UserPicture = item.ThirdPictureUrl;
                    }

                    if (item.ToIsAnonymous == true)
                    {
                        item.ToNickName = "匿名";
                    }
                }
                return result;
        }

        public OperationResult<AddReplyCommonEntity> ArticleReply_Insert(EventItemReplyEntity entity)
        {
            return dao.ArticleReply_Insert(entity);
        }

        public OperationResult<ReplyCommonEntity> CommentsLike_Insert(CommentLikeEntity entity)
        {
            return dao.CommentsLike_Insert(entity);
        }
        public OperationResult<ReviewCommonEntity> ReviewCommon_Get(EventItemCommentEntity entity)
        {
            return dao.ReviewCommon_Get(entity);
        }

        public OperationResult<bool> Report_Insert(ReportEntity entity) 
        {
            return dao.Report_Insert(entity);
        }

    }
}
