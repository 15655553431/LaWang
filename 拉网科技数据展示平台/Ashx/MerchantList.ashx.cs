using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 拉网科技数据展示平台.Ashx
{
    /// <summary>
    /// MerchantList 的摘要说明
    /// </summary>
    public class MerchantList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string action = context.Request.QueryString["action"];

            switch (action)
            {
                case "getlist": ShowMerchantinfo(context); break;
                case "getbyid": GetById(context); break;
                case "delete": Delete(context); break;
                case "save": Save(context); break;

            }
        }

        private void Save(HttpContext context)
        {
            MerchantName uisave = new MerchantName();
            string idstr = context.Request.Form["userid"];
            if (!string.IsNullOrWhiteSpace(idstr))
            {
                uisave.ID = Convert.ToInt32(idstr);
            }
            uisave.Name = context.Request.Form["Name"];
            uisave.Number = context.Request.Form["Number"];

            if (uisave.ID > 0)
            {
                //修改商户信息
                if (MerchantNameDal.Update(uisave))
                {
                    context.Response.Write("updatetrue");
                }
            }
            else
            {
                //新增商户
                if (MerchantNameDal.Insert(uisave))
                {
                    context.Response.Write("addtrue");
                }

            }
        }

        /// <summary>
        /// 根据用户id，绑定数据
        /// </summary>
        /// <param name="context"></param>
        private void GetById(HttpContext context)
        {
            string idstr = context.Request.Form["searchid"];
            MerchantName uiSearch = new MerchantName();
            if (!string.IsNullOrWhiteSpace(idstr))
            {
                uiSearch.ID = Convert.ToInt32(idstr);
            }
            string json = JsonHelper.ListToJson<MerchantName>(MerchantNameDal.GetList(uiSearch));
            context.Response.Write(json);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="context"></param>
        private void Delete(HttpContext context)
        {
            string id = context.Request.Form["delid"];
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (MerchantNameDal.Delete(Convert.ToInt32(id)))
                {
                    context.Response.Write("true");
                }
                else
                {
                    context.Response.Write("false");
                }
            }
        }


        /// <summary>
        /// 展示商户数据
        /// </summary>
        /// <param name="context"></param>
        private void ShowMerchantinfo(HttpContext context)
        {
            MerchantName mnSearch = new MerchantName();
            mnSearch.Name = context.Request.Form["MerchantName"];
            mnSearch.Number = context.Request.Form["MerchantNumber"];
            string json = JsonHelper.ListToJson<MerchantName>(MerchantNameDal.GetList(mnSearch));
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}