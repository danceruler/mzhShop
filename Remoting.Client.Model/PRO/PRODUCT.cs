using Mzh.Public.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata;
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <returns></returns>
        public ResultModel AddCateGory(string name, int displayorder = 0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 批量删除分类
        /// </summary>
        /// <param name="cateids"></param>
        /// <returns></returns>
        public ResultModel DeleteCateGories(int[] cateids)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="cateid"></param>
        /// <returns></returns>
        public ResultModel DeleteCateGory(int cateid)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改商品分类信息
        /// </summary>
        /// <param name="cateid"></param>
        /// <param name="name"></param>
        /// <param name="displayorder"></param>
        /// <returns></returns>
        public ResultModel UpdateCateGory(int cateid, string name, int displayorder)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddProduct(AddProductModel model)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 删除商品的规格信息
        /// </summary>
        /// <param name="type">1表示删除某个属性的规格，2为删除某个属性值的规格</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel DeleteProductSku(int pid, int type, int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 编辑商品信息
        /// </summary>
        /// <returns></returns>
        public ResultModel UpdatePorduct(AddProductModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ResultModel Deleteproduct(int pid)
        {
            throw new NotImplementedException();
        }
    }
}
