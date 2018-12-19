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
    /// MerchantRearList 的摘要说明
    /// </summary>
    public class MerchantRearList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string action = context.Request.QueryString["action"];

            switch (action)
            {
                case "getlist": ShowMerchantinfo(context); break;
                case "select": ShowSelect(context); break;
                case "getbyid": GetById(context); break;
                case "delete": Delete(context); break;
                case "save": Save(context); break;

            }
        }

        /// <summary>
        /// 根据用户id，绑定数据
        /// </summary>
        /// <param name="context"></param>
        private void GetById(HttpContext context)
        {
            string idstr = context.Request.Form["searchid"];
            MerchantRear uiSearch = new MerchantRear();
            if (!string.IsNullOrWhiteSpace(idstr))
            {
                uiSearch.ID = Convert.ToInt32(idstr);
            }
            string json = JsonHelper.ListToJson<MerchantRear>(MerchantRearDal.GetList(uiSearch));
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
                if (MerchantRearDal.Delete(Convert.ToInt32(id)))
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
        /// 绑定下拉框的值
        /// </summary>
        /// <param name="context"></param>
        private void ShowSelect(HttpContext context)
        {
            string json = JsonHelper.ListToJson<MerchantName>(MerchantRearDal.GetName());
            context.Response.Write(json);
        }

       

        private void Save(HttpContext context)
        {
            MerchantRear uisave = new MerchantRear();
            string idstr = context.Request.Form["userid"];
            if (!string.IsNullOrWhiteSpace(idstr))
            {
                uisave.ID = Convert.ToInt32(idstr);
            }
            uisave.Name = context.Request.Form["Name"];
            uisave.Number = context.Request.Form["Number"];
            uisave.FkID = Convert.ToInt32(context.Request.Form["selectname"]);

            if (uisave.ID > 0)
            {
                //修改商户信息
                if (MerchantRearDal.Update(uisave))
                {
                    context.Response.Write("updatetrue");
                }
            }
            else
            {
                //新增商户
                if (MerchantRearDal.Insert(uisave))
                {
                    context.Response.Write("addtrue");
                }

            }
        }


        /// <summary>
        /// 展示商户数据
        /// </summary>
        /// <param name="context"></param>
        private void ShowMerchantinfo(HttpContext context)
        {
            MerchantRear mnSearch = new MerchantRear();
            mnSearch.Name = context.Request.Form["MerchantName"];
            mnSearch.Number = context.Request.Form["MerchantNumber"];
            mnSearch.MName = context.Request.Form["SreachMName"];
            string json = JsonHelper.ListToJson<MerchantRear>(MerchantRearDal.GetList(mnSearch));
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