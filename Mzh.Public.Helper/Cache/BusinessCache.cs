using Mzh.Public.Base;
using Mzh.Public.DAL;
using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remoting
{
    public class BusinessCache : MarshalByRefObject
    {
        public static BusinessModel Business = new BusinessModel();

        public void InitBusinessCache()
        {
            string sql = $@"SELECT *,bsp_businessimgs.businessimgid bi_businessimgid,bsp_businessimgs.businessid bi_businessid,bsp_businessimgs.img bi_img
                              FROM bsp_businesses
                              LEFT JOIN bsp_businessimgs ON bsp_businessimgs.businessid = bsp_businesses.businessid";
            var dt = SqlManager.FillDataTable(AppConfig.ConnectionString, new SqlCommand(sql));
            Business = dt.GetList<BusinessModel>("").Distinct(new DistinctModel<BusinessModel>()).FirstOrDefault();
            if (Business != null)
            {
                Business.imgs = new List<BusinessImgModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(dr["bi_businessimgid"].ToString()))
                    {
                        Business.imgs.Add(new BusinessImgModel()
                        {
                            bi_businessid = int.Parse(dr["bi_businessid"].ToString()),
                            bi_businessimgid = int.Parse(dr["bi_businessimgid"].ToString()),
                            bi_img = dr["bi_img"].ToString()
                        });
                    }
                }
            }
        }

        public ResultModel AddOrUpdateBusiness(BusinessModel businessModel)
        {
            using (brnshopEntities context = new brnshopEntities())
            {
                var tran = context.Database.BeginTransaction();
                try
                {
                    if(businessModel.businessid == 0)
                    {
                        bsp_businesses bsp_Businesses = new bsp_businesses();
                        bsp_Businesses.address = businessModel.address;
                        bsp_Businesses.addtime = DateTime.Now;
                        bsp_Businesses.businessEnd = businessModel.businessEnd;
                        bsp_Businesses.businessStart = businessModel.businessStart;
                        bsp_Businesses.businessTimeStr = businessModel.businessStart/60+":"+ businessModel.businessStart % 60 + "-" + businessModel.businessEnd / 60 + ":" + businessModel.businessEnd % 60;
                        bsp_Businesses.description = businessModel.description;
                        bsp_Businesses.name = businessModel.name;
                        bsp_Businesses.telphone = businessModel.telphone;
                        bsp_Businesses.avgconsume = businessModel.avgconsume;
                        bsp_Businesses.specialserver = businessModel.specialserver;
                        bsp_Businesses.latitude = businessModel.latitude;
                        bsp_Businesses.longitude = businessModel.longitude;
                        context.bsp_businesses.Add(bsp_Businesses);
                        context.SaveChanges();
                        foreach (var img in businessModel.imgs)
                        {
                            bsp_businessimgs bsp_Businessimgs = new bsp_businessimgs();
                            bsp_Businessimgs.businessid = bsp_Businesses.businessid;
                            bsp_Businessimgs.img = img.bi_img;
                            context.bsp_businessimgs.Add(bsp_Businessimgs);
                            context.SaveChanges();
                        }
                    }
                    else
                    {
                        bsp_businesses bsp_Businesses = context.bsp_businesses.SingleOrDefault(t => t.businessid == businessModel.businessid);
                        bsp_Businesses.address = businessModel.address;
                        bsp_Businesses.businessEnd = businessModel.businessEnd;
                        bsp_Businesses.businessStart = businessModel.businessStart;
                        bsp_Businesses.businessTimeStr = DateTime.Now.Date.AddMinutes(businessModel.businessStart).ToString("HH:mm") + "-" + DateTime.Now.Date.AddMinutes(businessModel.businessEnd).ToString("HH:mm");
                        bsp_Businesses.description = businessModel.description;
                        bsp_Businesses.name = businessModel.name;
                        bsp_Businesses.telphone = businessModel.telphone;
                        bsp_Businesses.avgconsume = businessModel.avgconsume;
                        bsp_Businesses.specialserver = businessModel.specialserver;
                        bsp_Businesses.latitude = businessModel.latitude;
                        bsp_Businesses.longitude = businessModel.longitude;
                        context.SaveChanges();

                        var imgs = context.bsp_businessimgs.Where(t => t.businessid == bsp_Businesses.businessid);
                        foreach(var img in imgs)
                        {
                            context.bsp_businessimgs.Remove(img);
                            context.SaveChanges();
                        }

                        foreach (var img in businessModel.imgs)
                        {
                            bsp_businessimgs bsp_Businessimgs = new bsp_businessimgs();
                            bsp_Businessimgs.businessid = bsp_Businesses.businessid;
                            bsp_Businessimgs.img = img.bi_img;
                            context.bsp_businessimgs.Add(bsp_Businessimgs);
                            context.SaveChanges();
                        }
                    }
                    tran.Commit();
                    InitBusinessCache();
                    return ResultModel.Success("保存");
                }
                catch(Exception ex)
                {
                    Logger._.Error("AddOrUpdateBusiness",ex);
                    tran.Rollback();
                    return ResultModel.Error(ex.ToString());
                }
            }
        }

        public ResultModel GetBusiness()
        {
            return ResultModel.Success("", Business);
        }
    }
}
