using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace 拉网科技数据展示平台.Ashx
{
    /// <summary>
    /// mpIndex 的摘要说明
    /// </summary>
    public class mpIndex : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string action = context.Request.QueryString["action"];


            switch (action)
            {
                case "login": CheckLogin(context); break;
                case "exitlogin": ExitLogin(context); break;
            }
        }

        /// <summary>
        /// 校验用户名和密码是否存在，不存在就返回登陆界面
        /// </summary>
        /// <param name="context"></param>
        private void CheckLogin(HttpContext context)
        {
            if (context.Session["UserName"] == null)
            {
                context.Response.Redirect("~/login.html");
                context.Response.Write("false");
            }
            else
            {
                context.Response.Write("1," + context.Session["Name"].ToString() + "," + context.Session["UserID"].ToString());
            }
            return;
        }


        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <param name="context"></param>
        private void ExitLogin(HttpContext context)
        {
            context.Session.Clear();
            context.Session.Abandon();
            context.Response.Cookies["USERInfo_COOKIE"].Expires = DateTime.Now;
            context.Response.Redirect("~/login.html");
            context.Response.Write("ok");
            return;
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