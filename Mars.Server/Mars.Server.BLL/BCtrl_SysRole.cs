using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mars.Server.DAO;
using Mars.Server.Entity;
using System.Data;

namespace Mars.Server.BLL
{
   public class BCtrl_SysRole
    {
        SysRoleDAO dao = new SysRoleDAO();

        public DataTable QueryRoleTableByPage(SysRoleSearchEntity entity, out int totalcnt)
        {
            return dao.QueryRoleTableByPage(entity, out totalcnt);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public SysRoleEntity QueryEntity(int roleID)
        {
            return dao.QueryEntity(roleID);
        }

        /// <summary>
        /// 添加角色并和菜单关联
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <param name="roleFunRelList"></param>
        /// <returns></returns>
        public bool Insert(SysRoleEntity roleEntity, List<SysRoleFunctionRelEntity> roleFunRelList)
        {
            return dao.Insert(roleEntity, roleFunRelList);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(SysRoleEntity entity)
        {
            return dao.Insert(entity);
        }

        public bool Update(SysRoleEntity entity)
        {
            return dao.Update(entity);
        }

        /// <summary>
        /// 更新角色及关联菜单
        /// </summary>
        /// <param name="roleEntity"></param>
        /// <param name="roleFunRelList"></param>
        /// <returns></returns>
        public bool Update(SysRoleEntity roleEntity, List<SysRoleFunctionRelEntity> roleFunRelList)
        {
            return dao.Update(roleEntity, roleFunRelList);
        }

        /// <summary>
        /// 删除角色 
        /// 执行此操作前，先调用 IsCanDelRole（）方法判断是否可以删除当前角色
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public bool Delete(int roleID)
        {
            return dao.Delete(roleID);
        }

        /// <summary>
        /// 判断角色名是否可用 
        /// </summary>
        /// <param name="roleID"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public bool IsUseableRoleName(int roleID, string rolename)
        {
            return dao.IsUseableRoleName(roleID, rolename);
        }

        /// <summary>
        /// 判断是否可以删除角色
        /// 当 用户没有使用该角色时方可删除
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public bool IsCanDelRole(int roleID)
        {
            return dao.IsCanDelRole(roleID);
        }
    }
}
