using Mzh.Public.Base;
using Mzh.Public.DAL;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class SKU : MarshalByRefObject
    {
        /// <summary>
        /// 添加规格属性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultModel AddAttribute(string name)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var attr = context.bsp_attributes.SingleOrDefault(t => t.name == name);
                    if (attr != null)
                    {
                        return ResultModel.Fail("该属性已经存在");
                    }
                    bsp_attributes newattr = new bsp_attributes();
                    newattr.name = name;
                    context.bsp_attributes.Add(newattr);
                    context.SaveChanges();
                    return ResultModel.Success("", newattr.attrid);
                }
                catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 添加规格属性值
        /// </summary>
        /// <param name="attrid"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultModel AddAttributeValue(short attrid,string value)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var attrvalue = context.bsp_attributevalues.SingleOrDefault(t => t.attrid == attrid && t.attrvalue == value);
                    if (attrvalue != null)
                    {
                        return ResultModel.Fail("该属性值已经存在");
                    }
                    var attr = context.bsp_attributes.SingleOrDefault(t => t.attrid == attrid);
                    bsp_attributevalues newattrvalue = new bsp_attributevalues();
                    newattrvalue.attrid = attrid;
                    newattrvalue.attrname = attr.name;
                    newattrvalue.attrvalue = value;
                    context.bsp_attributevalues.Add(newattrvalue);
                    context.SaveChanges();
                    return ResultModel.Success("", newattrvalue.attrvalueid);
                }
                catch(Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 商品添加sku信息
        /// </summary>
        public ResultModel AddSKU(int pid,int valueid,int isdefaultprice,decimal price)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var sku = context.bsp_productskus.SingleOrDefault(t => t.pid == pid && t.attrvalueid == valueid);
                    if (sku != null)
                    {
                        return ResultModel.Fail("该规格SKU已经存在");
                    }
                    var attrvalue = context.bsp_attributevalues.SingleOrDefault(t => t.attrvalueid == valueid);
                    bsp_productskus newsku = new bsp_productskus();
                    newsku.pid = pid;
                    newsku.attrid = attrvalue.attrid;
                    newsku.attrvalueid = valueid;
                    newsku.inputattr = attrvalue.attrname;
                    newsku.inputvalue = attrvalue.attrvalue;
                    newsku.isdefaultprice = isdefaultprice;
                    newsku.price = price;
                    context.bsp_productskus.Add(newsku);
                    context.SaveChanges();
                    ProductCache.InitProductList();
                    return ResultModel.Success("", newsku.recordid);
                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 商品删除SKU信息
        /// </summary>
        /// <returns></returns>
        public ResultModel DeleteSKU(int pid, int valueid)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                try
                {
                    var sku = context.bsp_productskus.SingleOrDefault(t => t.pid == pid && t.attrvalueid == valueid);
                    if (sku == null)
                    {
                        return ResultModel.Fail("该规格SKU已经删除");
                    }
                    context.bsp_productskus.Remove(sku);
                    context.SaveChanges();
                    ProductCache.InitProductList();
                    return ResultModel.Success();
                }
                catch (Exception ex)
                {
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error(ex.ToString());
                }
            }
        }
    }
}
