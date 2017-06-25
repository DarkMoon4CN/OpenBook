using Mars.Server.DAO;
using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Mars.Server.BLL
{
    public class BCtrl_Categories
    { 
        /// <summary>
        /// 新建分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Insert(CategoriesEntity entity)
        {
            bool isSuccess = categories.Insert(entity);
            if (isSuccess)
            {
                ClearCacheOrSession.ClearFunctionsCacheByCRUD();
            }
            return isSuccess;
        }
        public CategoriesEntity QueryCategories(int CategoriesID)
        {
            return categories.QueryCategories(CategoriesID);
        }
        /// <summary>
        /// 修改分类信息
        /// </summary>
        /// <param name="Cateedit"></param>
        /// <returns></returns>
        public bool Categoriesedit(CategoriesEntity Cateedit)
        {
            return (bool)categories.Categoriesedit(Cateedit);
        } 
        /// <summary>
        /// 查询出一级分类
        /// </summary>
        /// <returns></returns>
        public List<CategoriesEntity> CategoriesFirstLevel()
        {
            return categories.CategoriesFirstLevel();
        }
        CategoriesDAO categories = new CategoriesDAO(); 
        /// <summary>
        /// 一级分类管理信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="totalcnt"></param>
        /// <returns></returns>
        public DataTable QueryCategoriesTable(CategoriesSearchEntity entity, out int totalcnt)
        {
            return categories.QueryCategoriesTable(entity, out totalcnt); 
        }
        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string deleteCategories(string id)
        {
            return categories.delectCategories(id);
        }
        /// <summary>
        /// 判断分类是否可以添加
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="fname"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsUserable(int pid, string fname, int id)
        {
            return (bool)categories.IsUserable(pid, fname, id);
        }
    }
}
