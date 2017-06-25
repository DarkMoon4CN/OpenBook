using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mars.Server.BLL;
using Mars.Server.Entity;
using Mars.Server.Utils;
namespace Mars.Server.App.Modules
{
    public class FestivalModule : ModuleBase
    {
        BCtrl_Festival fesobj = new BCtrl_Festival();
        public FestivalModule()
            : base("/Festival")
        {
            Get["/Handshake"] = _ => {
                try
                {
                    var data = FecthQueryData();
                    DateTime dt = data.date;
                    string hash = data.hash;
                    string hash2= fesobj.QueryAllFestivalsHash(dt);
                    if (hash == hash2)
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "数据一致", Tag = "1" });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 1, Msg = "数据不一致", Tag = "0" });
                    }
                }
                catch(Exception ex ) {
                    return JsonObj<JsonMessageBase>.ToJson(new JsonMessageBase() { Status = 0, Msg =ex.Message });
                }
            };

            Get["/GetAllFestival"] = _ => {
                try
                {
                    var data = FecthQueryData();
                    DateTime dt = data.date;
                    List<FestivalEntity> items = fesobj.QueryAllFestivalsTillNow(dt);
                    if (items != null)
                    {
                        return JsonObj<JsonMessageBase<List<FestivalEntity>>>.ToJson(new JsonMessageBase<List<FestivalEntity>>() { Status = 1, Msg = "OK", Value=items });
                    }
                    else
                    {
                        return JsonObj<JsonMessageBase<List<FestivalEntity>>>.ToJson(new JsonMessageBase<List<FestivalEntity>>() { Status = 1, Msg = "数据为空", Value = new List<FestivalEntity>() });
                    }
                }
                catch (Exception ex)
                {
                    return JsonObj<JsonMessageBase<List<FestivalEntity>>>.ToJson(new JsonMessageBase<List<FestivalEntity>>() { Status = 0, Msg =ex.Message, Value = new List<FestivalEntity>() });
                }
            };

        }
    }
}
