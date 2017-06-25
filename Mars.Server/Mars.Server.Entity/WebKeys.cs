using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.Entity
{
    public class WebKeys
    {
        /// <summary>
        /// WebApp管理员Key
        /// </summary>
        public const string AdminSessionKey = "__MarsServerWebAppAdmin";

        /// <summary>
        /// WebApp用户Key
        /// </summary>
        public const string UserSessionKey = "__MarsServerWebAppUser";

        /// <summary>
        /// WebApp左侧菜单
        /// </summary>
        public const string CK_SYS_LEFTMENU = "__Sys_LeftMenu_{0}";


        /// <summary>
        /// 管理员密码加密随机码
        /// </summary>
        public const string AdminPwdRandom = "OpenBookMars";

        public const string ExhibitorCacheKey = "__ExhibitorCacheKey_{0}";
        public const string ExhibitorLocationConsoleCacheKey = "__ExhibitorLocationConsoleCacheKey_{0}";
        public const string ActivityCacheKey = "__ActivityCacheKey_{0}";
        public const string SearchKeyWordyCacheKey = "__SearchKeyWordyCacheKey_{0}";
        /// <summary>
        /// 春联
        /// </summary>
        public const string CoupletCacheKey = "__CoupletCacheKey";
        /// <summary>
        /// 福字
        /// </summary>
        public const string FuimageCacheKey = "__FuimageCacheKey";
        /// <summary>
        /// AccessToken
        /// </summary>
        public const string AccessTokenCacheKey = "__AccessTokenCacheKey";
    }
}
