using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace 拉网科技数据展示平台.Ashx
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string action = context.Request.QueryString["action"];


            switch (action)
            {
                case "checkname": CheckName(context); break;
                case "checkpwd": CheckPwd(context); break;
                case "login": CheckLogin(context); break;
            }
            
        }

        /// <summary>
        /// 校验密码
        /// </summary>
        /// <param name="context"></param>
        private void CheckPwd(HttpContext context)
        {
            string UserName = context.Request.Form["name"];
            string PassWord = context.Request.Form["pwd"];
            if (!UserInfoDal.Login(UserName,PassWord))
            {
                context.Response.Write("false");
            }
        }

        /// <summary>
        /// 校验用户名是否存在
        /// </summary>
        /// <param name="context"></param>
        private void CheckName(HttpContext context)
        {
            string UserName = context.Request.Form["name"];
            if (!UserInfoDal.CheckUserName(UserName))
            {
                context.Response.Write("false");
            }
        }

        /// <summary>
        /// 用户名密码都正确，校验后直接转到登陆页面,并检查是否需要记住密码
        /// </summary>
        /// <param name="context"></param>
        private void CheckLogin(HttpContext context)
        {
            string UserName = context.Request.Form["name"];
            string PassWord = context.Request.Form["pwd"];
            string remember = context.Request.Form["online"];

            if (UserInfoDal.Login(UserName, PassWord))
            {
                if (remember == "true")
                {
                    HttpCookie cookie = new HttpCookie("USERInfo_COOKIE");
                    //cookie.Path = "H-ui.admin/";
                    cookie["UserName"] = UserName;
                    cookie["qwer"] = "qwer";
                    //这里是设置Cookie的过期时间，这里设置一个星期的时间，过了一个星期之后状态保持自动清空。
                    cookie.Expires = System.DateTime.Now.AddDays(7.0);
                    string test = cookie["UserName"];
                    context.Response.Cookies.Add(cookie);
                    
                }
                else
               //没有选择记住密码，删除cookie
                {
                    context.Response.Cookies["USERInfo_COOKIE"].Expires = DateTime.Now;
                }

                context.Session["UserName"] = UserName;
                context.Session["UserPassword"] = PassWord;
                string idstr = UserInfoDal.GetUserInfoID(UserName, PassWord);
                context.Session["UserID"] = idstr;
                UserInfo ui=new UserInfo();
                ui.ID = Convert.ToInt32(idstr);
                context.Session["Name"] = UserInfoDal.GetList(ui).First(u => true).Name;
                context.Response.Redirect("../PC/index.html"); 
            }
            else
     
            {
                context.Response.Redirect("../PC/login.html"); 
            }

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