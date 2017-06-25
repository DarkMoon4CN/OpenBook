using Mars.Server.App.Core;
using Mars.Server.Entity;
using SparrowF.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.App.Modules
{
    public class ModuleBase : Nancy.NancyModule
    {

        public ModuleBase() { }

        public ModuleBase(string path, bool addjsonheader = true)
            : base(path)
        {
            if (addjsonheader)
            {
                After += ctx =>
                {
                    ctx.Response.ContentType = "application/json";
                };
            }

        }

        protected dynamic FecthQueryData()
        {
            return JsonObj<dynamic>.FromJson(Request.Query.data);
        }

        protected dynamic FetchFormData()
        {
            return JsonObj<dynamic>.FromJson(Request.Form.data);
        }

        protected T FetchFormData<T>()
        {
            return JsonObj<T>.FromJson(Request.Form.data);
        }

        protected T FecthQueryData<T>()
        {
            return JsonObj<T>.FromJson(Request.Query.data);
        }


        protected string SendSucessMsg(string msg, int state = 1)
        {
            return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = state, Msg = msg });
        }
        protected string SendFailedMsg(string msg)
        {
            return SendSucessMsg(msg, 0);
        }

        public SessionIdentity CurrentUser
        {
            get
            {
                if (this.Context.CurrentUser != null)
                {
                    MarsUserIdentity id = this.Context.CurrentUser as MarsUserIdentity;
                    if (id != null)
                        return SessionCenter.GetIdentity(id.SessionID);
                    else
                        return null;
                }
                else
                    return null;
            }
        }

    }
}
