using Mars.Server.DAO;
using Mars.Server.Entity;
using Mars.Server.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.BLL
{
    public class BCtrl_Function
    {
        FunctionDAO functiondao = new FunctionDAO();

        public DataTable QueryFunctionTable(FunctionSearchEntity entity, out int totalcnt)
        {
            return functiondao.QueryFunctionTable(entity, out totalcnt);
        }

        /// <summary>
        /// 获取分组后的功能菜单
        /// </summary>
        /// <returns></returns>
        public List<FunctionEntity> QueryFunctionListByGroup(FunctionSearchEntity entity)
        {
            List<FunctionEntity> list = this.QueryFunctionList();
            List<FunctionEntity> resultList = null;
      
            if (list != null && list.Count > 0)
            {
                if (!string.IsNullOrEmpty(entity.Function_Name))
                {
                    resultList = list.Where(item => item.Function_Level == 1 && item.Function_ParentID == 0 && item.Function_Name.Contains(entity.Function_Name)).ToList();
                }
                else
                {
                    resultList = list.Where(item => item.Function_Level == 1 && item.Function_ParentID == 0).ToList();
                }               

                foreach (FunctionEntity item in resultList)
                {
                    List<FunctionEntity> childList = list.Where(child => child.Function_Level == 2 && child.Function_ParentID == item.Function_ID).ToList();

                    item.ChildFunctionList = childList;
                }
            }
            
            #region 作废 
            //if (table != null && table.Rows.Count > 0)
            //{
            //   list =  table.AsEnumerable().Where(row => row.Field<int>("Function_Level") == 1 && row.Field<int>("Function_ParentID") == 0)                  
            //         .Select(row => new FunctionEntity
            //         {
            //             Function_Level = row.Field<int>("Function_Level"),
            //             Function_Name = row.Field<string>("Function_Name"),
            //             Function_ID = row.Field<int>("Function_ID"),
            //             Function_URL_New = row.Field<string>("Function_URL_New"),
            //             CreateDate = row.Field<DateTime>("CreateDate")
            //         }).ToList();

            //    foreach (FunctionEntity funentity in list)
            //    {
            //        List<FunctionEntity> childFunList = table.AsEnumerable().Where(row => row.Field<int>("Function_Level") == 2 && row.Field<int>("Function_ParentID") == funentity.Function_ID)
            //                                            .OrderBy(row => row.Field<int>("Function_Order"))
            //                                            .Select(row => new FunctionEntity
            //                                            {
            //                                                Function_Level = row.Field<int>("Function_Level"),
            //                                                Function_Name = row.Field<string>("Function_Name"),
            //                                                Function_ID = row.Field<int>("Function_ID"),
            //                                                Function_URL_New = row.Field<string>("Function_URL_New"),
            //                                                CreateDate = row.Field<DateTime>("CreateDate")
            //                                            }).ToList();

            //        funentity.ChildFunctionList = childFunList;
            //    }

            //}
            #endregion

            return resultList;
        }

        /// <summary>
        /// 获取角色关联的菜单集
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<FunctionEntity> QueryFunctiolnList(int roleID)
        {
            return functiondao.QueryFunctiolnList(roleID);
        }

        /// <summary>
        /// 查询所有菜单
        /// </summary>
        /// <returns></returns>
        public List<FunctionEntity> QueryFunctionList()
        {
            return functiondao.QueryFunctionList();
        }

         /// <summary>
        /// 查询菜单实体
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public FunctionEntity QueryFunction(int functionID)
        {
            return functiondao.QueryFunction(functionID);
        }

        public bool Insert(FunctionEntity entity)
        {           
            bool isSuccess =  functiondao.Insert(entity);
            if (isSuccess)
            {
                ClearCacheOrSession.ClearFunctionsCacheByCRUD();
            }
            return isSuccess;
        }

        public bool Update(FunctionEntity entity)
        {            
            bool isSuccess = functiondao.Update(entity);
            if (isSuccess)
            {
                ClearCacheOrSession.ClearFunctionsCacheByCRUD();
            }
            return isSuccess;
        }

        /// <summary>
        /// 删除功能菜单
        /// 如果功能菜单已被用户使用，则不可删除
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public bool Delete(int functionID)
        {           
            bool isSuccess = functiondao.Delete(functionID);
            if (isSuccess)
            {
                ClearCacheOrSession.ClearFunctionsCacheByCRUD();
            }
            return isSuccess;
        }

        /// <summary>
        /// 判断菜单名是否可用 true可以 false不可以
        /// </summary>
        /// <param name="functionID"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        public bool IsUseableFunctionName(int functionID, string functionName)
        {
            return functiondao.IsUseableFunctionName(functionID, functionName);
        }

        /// <summary>
        /// 判断该菜单项是可否删除 true可删除  false不可删除
        /// </summary>
        /// <param name="functionID"></param>
        /// <returns></returns>
        public bool IsCanDelFunction(int functionID)
        {
            return functiondao.IsCanDelFunction(functionID);
        }

           /// <summary>
        /// 获取第一级菜单
        /// </summary>
        /// <param name="funLevel">菜单层级</param>
        /// <returns></returns>
        public List<FunctionEntity> QueryFirstLevelList(int funLevel =1)
        {
            return functiondao.QueryFirstLevelList(funLevel);
        }

        #region 以前功能集合
        /// <summary>
        /// 登录时保存用户状态得到用户能访问的频道集合
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.14
        /// </remarks> 
        /// <param name="userid">用户编号</param>
        /// <returns></returns>
        public List<FunctionEntity> GetFunction(String userid)
        {
            List<FunctionEntity> list = new List<FunctionEntity>();
            DataSet dsRelUser = functiondao.GetFunctionRelUser(userid);
            DataTable dtRelUser = null;
            if (dsRelUser.Tables.Count != 0)
            {
                dtRelUser = dsRelUser.Tables[0];
                foreach (DataRow var in dtRelUser.Rows)
                {
                    FunctionEntity fe = new FunctionEntity();
                    fe.Function_ID = Convert.ToInt32(var["Function_ID"].ToString());
                    fe.Function_Name = var["Function_Name"].ToString();
                    fe.Function_URL = var["Function_URL"].ToString();
                    fe.Function_ParentID = Convert.ToInt32(var["Function_ParentID"].ToString());
                    fe.Function_Order = var["Function_Order"].ToString();
                    fe.Function_isValid = Convert.ToInt32(var["Function_isValid"].ToString());
                    fe.Function_URL_New = var["Function_URL_New"].ToString();
                    fe.Function_IsNew = Convert.ToInt32(var["Function_isNew"].ToString());
                    list.Add(fe);
                }
            }
            return list;
        }
        /// <summary>
        /// 查询所有权限
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllFunction()
        {
            DataSet ds = functiondao.GetAllFunction();
            if (ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }

        public string GetFunctionTree()
        {
            StringBuilder sbTree_Html = new StringBuilder();

            sbTree_Html.Append("<ul>");
            //个人定制分类
            //sbTree_Html.Append(this.GetPrivateFunctionTree(userId));

            //公共分类
            sbTree_Html.Append(this.GetPublishFunctionTree());
            sbTree_Html.Append("</ul>");

            return sbTree_Html.ToString();
        }

        private string GetPublishFunctionTree()
        {
            string returnValue = "";
            string cachekey = "__bb_allfunction_treehtml";
            object o = MyCache<string>.Get(cachekey);
            if (o != null)
            {
                returnValue = o.ToString();
            }
            else
            {
                List<FunctionEntity> list = this.SelectAllFunctions();
                StringBuilder sbTree_Html = new StringBuilder();
                List<FunctionEntity> k1 = list.FindAll((_k) => { return _k.Function_Level == 1 && _k.Function_IsNew == 1 && _k.Function_isValid == 1; });
                List<FunctionEntity> k2 = list.FindAll((_k) => { return _k.Function_Level == 2 && _k.Function_IsNew == 1 && _k.Function_isValid == 1; });

                foreach (FunctionEntity k in k1)
                {
                    sbTree_Html.Append("<li class=\"open\"><input class=\"ace-checkbox-2\" type=\"checkbox\" checktype=\"public\" onclick=\"checkchange(this);\"  level=\"");
                    sbTree_Html.Append(k.Function_Level.ToString());
                    sbTree_Html.Append("\" value=\"");
                    sbTree_Html.Append(k.Function_ID);
                    sbTree_Html.Append("\" /><span>");
                    sbTree_Html.Append(k.Function_Name);
                    sbTree_Html.Append("</span>");
                    List<FunctionEntity> thissubkinds = k2.FindAll((_k) => { return _k.Function_ParentID == k.Function_ID; });
                    if (thissubkinds.Count > 0)
                    {
                        sbTree_Html.Append("<ul>");
                        foreach (FunctionEntity _subkind in thissubkinds)
                        {
                            sbTree_Html.Append("<li><input class=\"ace-checkbox-2\" type=\"checkbox\" checktype=\"public\" onclick=\"checkchange(this);\"  level=\"");
                            sbTree_Html.Append(_subkind.Function_Level.ToString());
                            sbTree_Html.Append("\" value=\"");
                            sbTree_Html.Append(_subkind.Function_ID);
                            sbTree_Html.Append("\" /><span>");
                            sbTree_Html.Append(_subkind.Function_Name);
                            sbTree_Html.Append("</span>");
                            sbTree_Html.Append("</li>");
                        }
                        sbTree_Html.Append("</ul>");
                    }
                    sbTree_Html.Append("</li>");
                }

                MyCache<string>.Insert(cachekey, sbTree_Html.ToString());
                returnValue = sbTree_Html.ToString();
            }

            return returnValue;
        }

        /// <summary>
        /// 根据查询前三级分类
        /// </summary>
        /// <param name="kid"></param>
        /// <returns></returns>
        public List<FunctionEntity> SelectAllFunctions()
        {
            string cache_key = "BB__AllFunction";
            object o = MyCache<List<FunctionEntity>>.Get(cache_key);
            if (o == null)
            {
                DataTable dt = functiondao.GetAllFunction().Tables[0];
                List<FunctionEntity> functions = new List<FunctionEntity>();
                foreach (DataRow dr in dt.Rows)
                {
                    FunctionEntity f = new FunctionEntity();
                    f.Function_URL_New = dr[7].ToString();
                    f.Function_ID = int.Parse(dr["Function_ID"].ToString());
                    f.Function_IsNew = int.Parse(dr["Function_IsNew"].ToString());
                    f.Function_isValid = int.Parse(dr["Function_isValid"].ToString());
                    f.Function_Level = int.Parse(dr["Function_Level"].ToString());
                    f.Function_Name = dr["Function_Name"].ToString();
                    f.Function_Order = dr["Function_Order"].ToString();
                    f.Function_ParentID = int.Parse(dr["Function_ParentID"].ToString());
                    f.Function_URL = dr["Function_URL"].ToString();


                    functions.Add(f);
                }
                if (functions.Count > 0)
                {
                    MyCache<List<FunctionEntity>>.Insert(cache_key, functions);
                }
                return functions;
            }
            else
            {
                return o as List<FunctionEntity>;
            }
        }

        /// <summary>
        /// 编辑用户的权限(带事务)
        /// </summary>
        /// <remarks>
        /// author: zp
        /// createtime: 2010.07.16
        /// </remarks> 
        /// <param name="userid">用户编号</param>
        /// <param name="list">权限集合</param>
        /// <returns></returns>
        public bool EditUserFunRel(String userid, List<FunctionEntity> list)
        {
            return functiondao.EditUserFunRel(userid, list);
        }
        /// <summary>
        /// 根据用户编号查询信息维护员的所在维护员管理下的所有节点
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public DataTable GetInfoManagerWithFunByID(String userid)
        {
            DataSet ds = functiondao.GetInfoManagerWithFunByID(userid);
            if (ds.Tables.Count != 0)
            {
                return ds.Tables[0];
            }
            else
            {
                return new DataTable();
            }
        }
        /// <summary>
        /// 为信息维护员删除某一节点的权限
        /// </summary>
        /// <remarks>
        /// create by perry
        /// create time 2010-07-22
        /// </remarks>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Boolean DelInfoManagerWithFun(String userid, int functionid)
        {
            return functiondao.DelInfoManagerWithFun(userid, functionid);
        }
        /// <summary>
        /// 为信息维护员添加某一节点的权限
        /// </summary>
        /// <remarks>
        /// create by perry
        /// create time 2010-07-22
        /// </remarks>
        /// <param name="userid"></param>
        /// <returns></returns>
        public Boolean AddInfoManagerWithFun(String userid, int functionid)
        {
            return functiondao.AddInfoManagerWithFun(userid, functionid);
        }
        #endregion
    }
}
