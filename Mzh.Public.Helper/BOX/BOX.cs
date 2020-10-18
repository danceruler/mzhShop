using Mzh.Public.Base;
using Mzh.Public.DAL;
using Mzh.Public.Model;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class BOX : MarshalByRefObject
    {
        /// <summary>
        /// 更新包厢
        /// </summary>
        /// <param name="boxid"></param>
        /// <param name="state"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultModel UpdateBox(int boxid, BoxState state, string name,decimal price,decimal bookprice)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    var box = context.bsp_boxes.SingleOrDefault(t => t.boxid == boxid);
                    if (box == null)
                    {
                        return ResultModel.Fail("该包厢已删除，请刷新");
                    }
                    box.state = (int)state;
                    box.name = name;
                    box.price = price;
                    box.bookprice = bookprice;
                    context.SaveChanges();

                    tran.Commit();
                    new BoxCache().Init();
                    return ResultModel.Success("修改成功");
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
        /// 新增包厢
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ResultModel AddBox(string code, string name, decimal price,decimal bookprice)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    var box = context.bsp_boxes.SingleOrDefault(t => t.code == code);
                    if(box != null)
                    {
                        return ResultModel.Fail("该包厢编号已经存在");
                    }
                    bsp_boxes newbox = new bsp_boxes();
                    newbox.booktime = null;
                    newbox.code = code;
                    newbox.name = name;
                    newbox.oid = 0;
                    newbox.state = (int)BoxState.Empty;
                    newbox.uid = 0;
                    newbox.price = price;
                    newbox.bookprice = bookprice;
                    context.bsp_boxes.Add(newbox);
                    context.SaveChanges();

                    tran.Commit();
                    new BoxCache().Init();
                    return ResultModel.Success("添加成功");
                }catch(Exception ex)
                {
                    tran.Rollback();
                    Logger._.Error(ex.ToString());
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 删除包厢
        /// </summary>
        /// <param name="boxid"></param>
        /// <returns></returns>
        public ResultModel DeleteBox(int boxid)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    var box = context.bsp_boxes.SingleOrDefault(t => t.boxid == boxid);
                    if (box == null)
                    {
                        return ResultModel.Fail("该包厢已删除，请刷新");
                    }
                    context.bsp_boxes.Remove(box);
                    context.SaveChanges();

                    tran.Commit();
                    new BoxCache().Init();
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
    }
}
