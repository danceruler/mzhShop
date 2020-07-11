using Mzh.Public.Base;
using Mzh.Public.DAL;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class PRODUCT:MarshalByRefObject
    {
        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <returns></returns>
        public List<ShowCatecory> GetCategories(int pid = 0)
        {
            string sqlwhere = pid == 0 ? "" : $" and bsp_cateproducts.pid = {pid}";
            string sql = $@"select bsp_categories.cateid,bsp_categories.name,bsp_categories.displayorder,T.productCount
                        from bsp_categories
                        join ((select bsp_categories.cateid,0 productCount
                        from bsp_categories
                        left join bsp_cateproducts on bsp_cateproducts.cateid = bsp_categories.cateid
                        where bsp_cateproducts.catepid is null {sqlwhere}
                        group by bsp_categories.cateid)
                        union
                        (select bsp_categories.cateid,COUNT(bsp_categories.cateid) productCount
                        from bsp_categories
                        left join bsp_cateproducts on bsp_cateproducts.cateid = bsp_categories.cateid
                        where bsp_cateproducts.catepid is not null {sqlwhere}
                        group by bsp_categories.cateid)) T ON T.cateid = bsp_categories.cateid
                        order by bsp_categories.displayorder desc";
            SqlCommand cmd = new SqlCommand(sql);
            DataTable dt = SqlManager.FillDataTable(AppConfig.ConnectionString, cmd);
            var result = dt.GetList<ShowCatecory>("");
            result.ForEach(c => c.name = c.name.Trim());
            return result;
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <returns></returns>
        public ResultModel AddCateGory(string name,int displayorder = 0 )
        {
            using(brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var catrgory = context.bsp_categories.SingleOrDefault(t => t.name == name);
                    if (catrgory != null)
                    {
                        return ResultModel.Fail("已存在该分类");
                    }
                    bsp_categories newcate = new bsp_categories();
                    newcate.name = name;
                    newcate.displayorder = displayorder;
                    newcate.pricerange = "";
                    newcate.path = "";
                    context.bsp_categories.Add(newcate);
                    context.SaveChanges();
                    return ResultModel.Success("添加成功", newcate.cateid);
                }
                catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 批量删除分类
        /// </summary>
        /// <param name="cateids"></param>
        /// <returns></returns>
        public ResultModel DeleteCateGories(int[] cateids)
        {
            using (brnshopEntities context = new brnshopEntities()) 
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    foreach (var cateid in cateids)
                    {
                        var cateproCount = context.bsp_products.Where(t => t.cateid == cateid).Count();
                        var cate = context.bsp_categories.SingleOrDefault(t => t.cateid == cateid);
                        if (cate == null)
                        {
                            tran.Rollback();
                            return ResultModel.Fail($"ID为{cateid}的分类已经删除，请刷新");
                        }
                        if (cateproCount > 0)
                        {
                            tran.Rollback();
                            return ResultModel.Fail($"{cate.name}分类下存在商品，不允许删除");
                        }
                        context.bsp_categories.Remove(cate);
                        context.SaveChanges();
                    }
                    tran.Commit();
                    return ResultModel.Success("删除成功");
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="cateid"></param>
        /// <returns></returns>
        public ResultModel DeleteCateGory(int cateid)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var cateproCount = context.bsp_products.Where(t => t.cateid == cateid).Count();
                    if (cateproCount > 0) 
                        return ResultModel.Fail("该分类下存在商品，不允许删除");
                    var cate = context.bsp_categories.SingleOrDefault(t => t.cateid == cateid);
                    if(cate == null) 
                        return ResultModel.Fail("该分类已经删除，请刷新");
                    context.bsp_categories.Remove(cate);
                    context.SaveChanges();
                    return ResultModel.Success("删除成功");
                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 修改商品分类信息
        /// </summary>
        /// <param name="cateid"></param>
        /// <param name="name"></param>
        /// <param name="displayorder"></param>
        /// <returns></returns>
        public ResultModel UpdateCateGory(int cateid,string name,int displayorder)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var cate = context.bsp_categories.SingleOrDefault(t => t.cateid == cateid);
                    cate.name = name;
                    cate.displayorder = displayorder;
                    context.SaveChanges();
                    return ResultModel.Success("修改成功");
                }
                catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddProduct(AddProductModel model)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    bsp_products newpro = new bsp_products();
                    newpro.addtime = DateTime.Now;
                    newpro.cateid = model.cateid;
                    newpro.costprice = model.costprice;
                    newpro.description = model.description;
                    newpro.displayorder = model.displayorder;
                    newpro.isbest = model.isbest;
                    newpro.ishot = model.ishot;
                    newpro.isnew = model.isnew;
                    newpro.isfullcut = model.isfullcut;
                    newpro.marketprice = model.marketprice;
                    newpro.name = model.name;
                    newpro.packprice = model.packprice;
                    newpro.showimg = model.showimg;
                    newpro.shopprice = model.shopprice;
                    newpro.weight = model.weight;
                    context.bsp_products.Add(newpro);
                    context.SaveChanges();


                    foreach(var img in model.mainImgs)
                    {
                        bsp_productimages newimg = new bsp_productimages();
                        newimg.displayorder = img.displayorder;
                        newimg.ismain = 1;
                        newimg.pid = newpro.pid;
                        newimg.showimg = img.showimg;
                        context.bsp_productimages.Add(newimg);
                        context.SaveChanges();
                    }

                    foreach (var img in model.detailImgs)
                    {
                        bsp_productimages newimg = new bsp_productimages();
                        newimg.displayorder = img.displayorder;
                        newimg.ismain = 0;
                        newimg.pid = newpro.pid;
                        newimg.showimg = img.showimg;
                        context.bsp_productimages.Add(newimg);
                        context.SaveChanges();
                    }

                    tran.Commit();
                    new ProductCache().Init();
                    return ResultModel.Success("", newpro.pid);

                }
                catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    tran.Rollback();
                    return ResultModel.Error(ex.ToString());
                }
            }
        }
    }
}
