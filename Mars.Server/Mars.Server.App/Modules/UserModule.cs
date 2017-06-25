using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.App.Modules
{
    public class UserModule : ModuleBase
    {
        BCtrl_Users userobj = new BCtrl_Users();
        public UserModule()
            : base("/User")
        {
            Post["/Update"] = _ =>
            {
                try
                {
                    UserEntity e = FetchFormData<UserEntity>();
                    e.UserID = CurrentUser.UserID;
                    bool state = userobj.UpdateUser(e);
                    if (state)
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "保存成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "保存失败" });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };

            Get["/QueryMyInfo"]=_=>{
                try
                {
                    UserEntity userinfo = userobj.QueryUserInfo(CurrentUser.UserID);
                    if (userinfo!=null)
                    {
                        return JsonObj<JsonMessageBase<UserEntity>>.ToJson(new JsonMessageBase<UserEntity>() {  Value=userinfo, Status = 1, Msg = "成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<UserEntity>>.ToJson(new JsonMessageBase<UserEntity>() { Value = new UserEntity(), Status = 0, Msg = "失败" });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase<UserEntity>>.ToJson(new JsonMessageBase<UserEntity>() { Value = new UserEntity(), Status = 0, Msg = ex.Message });
                }
            };

            Post["/ChangeMyPwd"] = _ =>
            {
                try
                {
                    dynamic data = FetchFormData();
                    string OldPwd = data.OldPwd;
                    string newpwd = data.NewPwd;
                    string msg = string.Empty;
                    bool state = userobj.ChangePwd(OldPwd, newpwd, CurrentUser.UserID, out msg);
                    if (state)
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "修改成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = msg });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };

            Post["/ChangeMyNickName"] = _ =>
            {
                try
                {
                    dynamic data = FetchFormData();
                    string nickname = data.NickName;
                    bool state = userobj.UpdateMyNickName(CurrentUser.UserID, nickname);
                    if (state)
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "修改成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "修改失败" });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };

            Post["/SaveUserPicture"] = _ => {
                try
                {
                    dynamic data = FetchFormData();
                    int pid = data.PictureID;
                    bool state = userobj.SaveUserPicture(pid,CurrentUser.UserID);
                    if (state)
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "修改成功" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "修改失败" });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = ex.Message });
                }
            };

            Get["/ClearUser"] = _ =>
            {
                var data = FecthQueryData();
                string key = data.DelKey;
                if (key == "001001011011" && CurrentUser.UserID > 0)
                {
                  bool state=userobj.ClearUser(CurrentUser.UserID);
                  if (state) 
                  {
                      return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "删除成功！" });
                  }
                }
                return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg = "删除用户不存在" });
            };

            Post["/RemindAsync"] = _ =>
            {

                dynamic data = FetchFormData().Value;
                string dataStr = data.ToString();
                IList<UserRemindEntity> entitys =JsonObj<List<UserRemindEntity>>.FromJson(dataStr);
                if (entitys == null)
                {
                    entitys = new List<UserRemindEntity>();
                }
                var result = BCtrl_UserRemind.Instance.UserRemind_InsertOrUpdate(entitys, CurrentUser.UserID);
                if (result.ResultType == OperationResultType.Success && result.AppendData == true)
                {
                    return JsonObj<JsonMessageBase<bool>>.ToJson(new JsonMessageBase<bool>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.GetEnumDescription(result.ResultType),
                        Value = true
                    });
                }
                else 
                {
                    return JsonObj<JsonMessageBase<bool>>.ToJson(new JsonMessageBase<bool>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.GetEnumDescription(result.ResultType),
                        Value = false
                    });
                }
            };
            Get["/QueryRemind"] = _ => 
            {
                var result=BCtrl_UserRemind.Instance.UserRemind_GetByUserID(CurrentUser.UserID);
                IList<UserRemindEntity> entitys = result.AppendData;
                if (entitys != null && entitys.Count != 0)
                {
                    return JsonObj<JsonMessageBase<IList<UserRemindEntity>>>.ToJson(new JsonMessageBase<IList<UserRemindEntity>>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.GetEnumDescription(result.ResultType),
                        Value = entitys
                    });
                }
                else 
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.GetEnumDescription(result.ResultType),
                    });
                }
            };

            Get["/QueryRemindType"] = _ =>
            {
                var result = BCtrl_UserRemind.Instance.UserRemindType_GetWhere();
                IList<UserRemindTypeEntity> entitys = result.AppendData;
                if (entitys != null && entitys.Count != 0)
                {
                    return JsonObj<JsonMessageBase<IList<UserRemindTypeEntity>>>.ToJson(new JsonMessageBase<IList<UserRemindTypeEntity>>()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.GetEnumDescription(result.ResultType),
                        Value = entitys
                    });
                }
                else 
                {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase()
                    {
                        Status = (int)result.ResultType,
                        Msg = result.GetEnumDescription(result.ResultType),
                    });
                }
            };

        }
    }
}
