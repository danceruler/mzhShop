using Mzh.Public.Base;
using Mzh.Public.DAL;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
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
                    context.bsp_categories.Add(newcate);
                    context.SaveChanges();
                    return ResultModel.Success("", newcate.cateid);
                }
                catch(Exception ex)
                {
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
                    return ResultModel.Success();
                }
                catch (Exception ex)
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
                    ProductCache.InitProductList();
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
