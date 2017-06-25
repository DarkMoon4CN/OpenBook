using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.App.Modules
{
    public class TestModule:ModuleBase
    {
        public TestModule():base("/Test")
        {
            Get["/Helloworld"] = _ => {
                return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() {  Status=1, Msg="Hello world From Get"});
            };

            Post["/Helloworld"] = _ =>
            {
                return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "Hello world From Post" });
            };
        }
    }
}
