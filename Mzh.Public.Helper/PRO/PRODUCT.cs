using Mzh.Public.Base;
using Mzh.Public.DAL;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
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
                        (select bsp_categories.cateid,SUM(CASE WHEN bsp_products.isdelete = 1 THEN 0 ELSE 1 END) productCount
                        from bsp_categories
                        left join bsp_cateproducts on bsp_cateproducts.cateid = bsp_categories.cateid
                        left join bsp_products on bsp_products.pid = bsp_cateproducts.pid
                        where bsp_cateproducts.catepid is not null  {sqlwhere}
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
                        var sql = $@"select COUNT(bsp_cateproducts.catepid)
						    from bsp_cateproducts
						    left join bsp_products on bsp_products.pid = bsp_cateproducts.pid
						    where bsp_products.isdelete = 0 and bsp_cateproducts.cateid = {cateid}";
                        var dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (int.Parse(dt.Rows[0][0].ToString()) > 0)
                            {
                                tran.Rollback();
                                return ResultModel.Fail("分类下存在商品，不允许删除");
                            }
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
                    var sql = $@"select COUNT(bsp_cateproducts.catepid)
						    from bsp_cateproducts
						    left join bsp_products on bsp_products.pid = bsp_cateproducts.pid
						    where bsp_products.isdelete = 0 and bsp_cateproducts.cateid = {cateid}";
                    var dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
                    if(dt != null && dt.Rows.Count > 0)
                    {
                        if(int.Parse(dt.Rows[0][0].ToString()) > 0)
                        {
                            return ResultModel.Fail("该分类下存在商品，不允许删除");
                        }
                    }
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
                    newpro.cateid = 0;
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
                    newpro.state = (byte)model.state;
                    newpro.showimg = model.showimg;
                    newpro.shopprice = model.shopprice;
                    newpro.weight = model.weight;
                    newpro.psn = "";
                    newpro.isdelete = 0;
                    context.bsp_products.Add(newpro);
                    context.SaveChanges();

                    //添加商品分类信息
                    foreach(var cateid in model.cateids)
                    {
                        bsp_categories cate = context.bsp_categories.SingleOrDefault(t => t.cateid == cateid);
                        bsp_cateproducts newcateproduct = new bsp_cateproducts();
                        newcateproduct.cateid = cateid;
                        newcateproduct.catename = cate.name.Trim();
                        newcateproduct.pid = newpro.pid;
                        newcateproduct.pname = newpro.name;
                        context.bsp_cateproducts.Add(newcateproduct);
                        context.SaveChanges();
                    }

                    var attrindex = 0;
                    
                    //添加属性和值
                    foreach (var attribute in model.skuInfos)
                    {
                        bsp_attributes newattribute = new bsp_attributes();
                        if (attribute.attrid == 0)
                        {
                            newattribute.name = attribute.name;
                            newattribute.remark = attribute.remark;
                            context.bsp_attributes.Add(newattribute);
                        }//新增的属性
                        else
                        {
                            newattribute = context.bsp_attributes.SingleOrDefault(t => t.attrid == attribute.attrid);
                        }//原有的属性
                        newattribute.displayorder = attrindex++;
                        context.SaveChanges();
                        attribute.attrid = newattribute.attrid;

                        var valueindex = 0;
                        foreach (var value in attribute.attributevalues)
                        {
                            bsp_attributevalues newvalue = new bsp_attributevalues();
                            bsp_productskus newsku = new bsp_productskus();
                            if (value.attrvalueid == 0)
                            {
                                newvalue.attrid = newattribute.attrid;
                                newvalue.attrname = newattribute.name;
                                newvalue.attrvalue = value.attrvalue;
                                newvalue.attrgroupname = "";
                                newvalue.isinput = 0;
                                newvalue.attrdisplayorder = 0;
                                newvalue.attrshowtype = 0;
                                newvalue.attrvaluedisplayorder = 0;
                                newvalue.attrgroupid = 0;
                                newvalue.attrgroupdisplayorder = 0;
                                context.bsp_attributevalues.Add(newvalue);
                            }//新增值和sku
                            else
                            {
                                newvalue = context.bsp_attributevalues.SingleOrDefault(t => t.attrvalueid == value.attrvalueid);

                            }//原有值
                            newvalue.attrdisplayorder = valueindex++;
                            context.SaveChanges();
                            value.attrvalueid = newvalue.attrvalueid;
                        }
                    }

                    //添加商品的sku信息
                    var skus = GetAllSkuByAttrAndValue(model.skuInfos);

                    //得到商品已有的sku中的valuid，valueid从大到小按逗号分隔组成字符串比较
                    var allpskus = context.bsp_productskus.Where(t => t.pid == newpro.pid).ToList();
                    var sku_guids = allpskus.Select(t => t.skuguid).Distinct();
                    List<string> guid_valueidstr = new List<string>();
                    foreach(var sku_guid in sku_guids)
                    {
                        var valueids = allpskus.Where(t => t.skuguid == sku_guid).Select(t => t.attrvalueid.ToString()).ToArray();
                        guid_valueidstr.Add(string.Join(",", valueids));
                    }

                    foreach (var sku in skus)
                    {
                        var valueids = sku.Select(t => t.valueid.ToString()).ToArray();
                        var nowvalueidstr = string.Join(",", valueids);
                        //该sku不存在，则新增
                        if (!guid_valueidstr.Contains(nowvalueidstr))
                        {
                            Guid newskuguid = Guid.NewGuid();
                            foreach(var s in sku)
                            {
                                bsp_productskus newsku = new bsp_productskus();
                                newsku.attrid = (short)s.attrid;
                                newsku.attrvalueid = s.valueid;
                                newsku.inputattr = model.skuInfos.FirstOrDefault(t => t.attrid == newsku.attrid).name;
                                newsku.inputvalue = model.skuInfos.FirstOrDefault(t => t.attrid == newsku.attrid).attributevalues.FirstOrDefault(t => t.attrvalueid == newsku.attrvalueid).attrvalue;
                                newsku.isdefaultprice = 1;
                                newsku.pid = newpro.pid;
                                newsku.price = -1;
                                newsku.skugid = 0;
                                newsku.skuguid = newskuguid;
                                newsku.stock = -1;
                                context.bsp_productskus.Add(newsku);
                                context.SaveChanges();
                            }
                        }
                    }

                    //添加商品图片
                    if (model.mainImgs != null)
                    {
                        foreach (var img in model.mainImgs)
                        {
                            bsp_productimages newimg = new bsp_productimages();
                            newimg.displayorder = img.displayorder;
                            newimg.ismain = 1;
                            newimg.pid = newpro.pid;
                            newimg.showimg = img.showimg;
                            context.bsp_productimages.Add(newimg);
                            context.SaveChanges();
                        }
                    }
                    
                    if(model.detailImgs != null)
                    {
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
                    }
                    

                    tran.Commit();
                    new ProductCache().Init();
                    return ResultModel.Success("添加成功", newpro.pid);

                }
                catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    tran.Rollback();
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 删除商品的规格信息
        /// </summary>
        /// <param name="type">1表示删除某个属性的规格，2为删除某个属性值的规格</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel DeleteProductSku(int pid,int type,int id)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    var pro = context.bsp_products.SingleOrDefault(t => t.pid == pid);
                    if(type == 1)
                    {
                        var skuguids = context.bsp_productskus.Where(t => t.pid == pid & t.attrid == id).Select(t => t.skuguid).Distinct();
                        foreach(var skuguid in skuguids)
                        {
                            var deleteskus = context.bsp_productskus.Where(t => t.skuguid == skuguid).ToList();
                            context.bsp_productskus.RemoveRange(deleteskus);
                        }
                    }
                    else
                    {
                        var skuguids = context.bsp_productskus.Where(t => t.pid == pid & t.attrvalueid == id).Select(t => t.skuguid).Distinct();
                        foreach (var skuguid in skuguids)
                        {
                            var deleteskus = context.bsp_productskus.Where(t => t.skuguid == skuguid).ToList();
                            context.bsp_productskus.RemoveRange(deleteskus);
                        }
                    }
                    context.SaveChanges();

                    tran.Commit();
                    new ProductCache().Init();
                    return ResultModel.Success("");

                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    tran.Rollback();
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <returns></returns>
        public ResultModel UpdatePorduct(AddProductModel model)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    var pro = context.bsp_products.SingleOrDefault(t => t.pid == model.pid);
                    //pro.addtime = DateTime.Now;
                    pro.cateid = 0;
                    pro.costprice = model.costprice;
                    pro.description = model.description;
                    pro.displayorder = model.displayorder;
                    pro.isbest = model.isbest;
                    pro.ishot = model.ishot;
                    pro.isnew = model.isnew;
                    pro.isfullcut = model.isfullcut;
                    pro.marketprice = model.marketprice;
                    pro.name = model.name;
                    pro.packprice = model.packprice;
                    pro.showimg = model.showimg;
                    pro.shopprice = model.shopprice;
                    pro.weight = model.weight;
                    
                    context.SaveChanges();

                    //编辑商品分类信息
                    foreach (var cateid in model.cateids)
                    {
                        bsp_categories cate = context.bsp_categories.SingleOrDefault(t => t.cateid == cateid);
                        bsp_cateproducts cateproduct = context.bsp_cateproducts.SingleOrDefault(t => t.pid == pro.pid & t.cateid == cateid);
                        if(cateproduct == null)
                        {
                            cateproduct = new bsp_cateproducts();
                            cateproduct.cateid = cateid;
                            cateproduct.catename = cate.name;
                            cateproduct.pid = pro.pid;
                            cateproduct.pname = pro.name;
                            context.bsp_cateproducts.Add(cateproduct);
                        }
                        else
                        {
                            cateproduct.catename = cate.name;
                            cateproduct.pname = pro.name;
                        }
                        context.SaveChanges();
                    }
                    //删除不存在的分类信息
                    var deletecateproducts = context.bsp_cateproducts.Where(t => t.pid == pro.pid & !model.cateids.Contains(t.cateid));
                    context.bsp_cateproducts.RemoveRange(deletecateproducts);
                    context.SaveChanges();

                    var attrindex = 0;
                    //编辑商品规格(商品规格的删除有专门接口)
                    foreach (var attribute in model.skuInfos)
                    {
                        bsp_attributes newattribute = new bsp_attributes();
                        if (attribute.attrid == 0)
                        {
                            newattribute.name = attribute.name;
                            newattribute.remark = attribute.remark;
                            context.bsp_attributes.Add(newattribute);
                        }//新增的属性
                        else
                        {
                            newattribute = context.bsp_attributes.SingleOrDefault(t => t.attrid == attribute.attrid);
                        }//原有的属性
                        newattribute.displayorder = attrindex++;
                        context.SaveChanges();

                        var valueindex = 0;
                        foreach (var value in attribute.attributevalues)
                        {
                            bsp_attributevalues newvalue = new bsp_attributevalues();
                            bsp_productskus newsku = new bsp_productskus();
                            if (value.attrvalueid == 0)
                            {
                                newvalue.attrid = newattribute.attrid;
                                newvalue.attrname = newattribute.name;
                                newvalue.attrvalue = value.attrvalue;
                                context.bsp_attributevalues.Add(newvalue);
                            }//新增值和sku
                            else
                            {
                                newvalue = context.bsp_attributevalues.SingleOrDefault(t => t.attrvalueid == value.attrvalueid);

                            }//原有值
                            newvalue.attrdisplayorder = valueindex++;
                            context.SaveChanges();

                            //判断该商品是否已经存在该属性和值的sku，存在更新不存在新增
                            var psku = context.bsp_productskus.SingleOrDefault(t => t.pid == pro.pid & t.attrid == newattribute.attrid & t.attrvalueid == newvalue.attrvalueid);
                            if (psku == null)
                            {
                                newsku.attrid = newattribute.attrid;
                                newsku.attrvalueid = newvalue.attrvalueid;
                                newsku.inputattr = newattribute.name;
                                newsku.inputvalue = newvalue.attrvalue;
                                newsku.isdefaultprice = value.price == -1 ? 1 : 0;
                                newsku.price = value.price;
                                newsku.stock = value.stock;
                                context.bsp_productskus.Add(newsku);
                            }
                            else
                            {
                                newsku = psku;
                                newsku.inputattr = newattribute.name;
                                newsku.inputvalue = newvalue.attrvalue;
                                newsku.isdefaultprice = value.price == -1 ? 1 : 0;
                                newsku.price = value.price;
                                newsku.stock = value.stock;
                            }
                            context.SaveChanges();
                        }

                    }

                    //删除商品图片
                    var imgs = context.bsp_productimages.Where(t => t.pid == pro.pid);
                    context.bsp_productimages.RemoveRange(imgs);
                    context.SaveChanges();

                    //添加商品图片
                    foreach (var img in model.mainImgs)
                    {
                        bsp_productimages newimg = new bsp_productimages();
                        newimg.displayorder = img.displayorder;
                        newimg.ismain = 1;
                        newimg.pid = pro.pid;
                        newimg.showimg = img.showimg;
                        context.bsp_productimages.Add(newimg);
                        context.SaveChanges();
                    }

                    foreach (var img in model.detailImgs)
                    {
                        bsp_productimages newimg = new bsp_productimages();
                        newimg.displayorder = img.displayorder;
                        newimg.ismain = 0;
                        newimg.pid = pro.pid;
                        newimg.showimg = img.showimg;
                        context.bsp_productimages.Add(newimg);
                        context.SaveChanges();
                    }

                    tran.Commit();
                    new ProductCache().Init();
                    return ResultModel.Success("");

                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    tran.Rollback();
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ResultModel Deleteproduct(int pid)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    var pro = context.bsp_products.SingleOrDefault(t => t.pid == pid);
                    pro.isdelete = 1;
                    context.SaveChanges();
                    tran.Commit();
                    new ProductCache().Init();
                    return ResultModel.Success("删除成功");
                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    tran.Rollback();
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 在新增或编辑商品时通过属性和属性值得到需要的sku列表
        /// </summary>
        private List<List<skuattrvalueItem>> GetAllSkuByAttrAndValue(List<AttributeInfo> attributes)
        {
            List<List<skuattrvalueItem>> allsku = new List<List<skuattrvalueItem>>();
            List<skuattrvalueItem> items = new List<skuattrvalueItem>();
            additemInAllSku(ref attributes, ref allsku, ref items, 0);
            return allsku;
        }

        private void additemInAllSku(ref List<AttributeInfo> attributes,ref List<List<skuattrvalueItem>> allsku,ref List<skuattrvalueItem> items, int nowlevel)
        {
            for(int i = 0;i< attributes[nowlevel].attributevalues.Count; i++)
            {
                items.Add(new skuattrvalueItem() { 
                    attrid = attributes[nowlevel].attrid,
                    valueid = attributes[nowlevel].attributevalues[i].attrvalueid
                });
                
                if(nowlevel == attributes.Count - 1 && i< attributes[nowlevel].attributevalues.Count - 1)
                {
                    var temparray = items.ToArray();
                    allsku.Add(temparray.ToList());
                    items.RemoveAt(items.Count - 1);
                }else if(nowlevel == attributes.Count - 1 && i == attributes[nowlevel].attributevalues.Count - 1)
                {
                    var temparray = items.ToArray();
                    allsku.Add(temparray.ToList());
                    items.RemoveAt(items.Count - 1);
                    return;
                }else if(nowlevel < attributes.Count - 1)
                {
                    var nextlevel = nowlevel + 1;
                    additemInAllSku(ref attributes, ref allsku, ref items, nextlevel);
                    items.RemoveAt(items.Count - 1);
                }
            }
        }



        class skuattrvalueItem
        {
            public int attrid { get; set; }
            public int valueid { get; set; }
        }
    }
}
