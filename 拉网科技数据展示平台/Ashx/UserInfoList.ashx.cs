using Common;
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
    /// UserInfoList 的摘要说明
    /// </summary>
    public class UserInfoList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string action = context.Request.QueryString["action"];

            switch (action)
            {
                case "getlist": ShowUserinfo(context); break;
                case "getbyid": GetById(context); break;
                case "delete": Delete(context); break;
                case "save": Save(context); break;
                case "changepwd": ChangePwd(context); break;

            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="context"></param>
        private void ChangePwd(HttpContext context)
        {
            string id = context.Request.Form["idnum"];
            string newpwd = context.Request.Form["newpassword"];
            if ((!string.IsNullOrWhiteSpace(id)) &&( !string.IsNullOrWhiteSpace(newpwd)))
            {
                if (UserInfoDal.UpdatePwd(Convert.ToInt32(id),newpwd))
                {
                    context.Response.Write("true");
                }
            }
        }

        /// <summary>
        /// 展示账户数据
        /// </summary>
        /// <param name="context"></param>
        private void ShowUserinfo(HttpContext context)
        {
            UserInfo uiSearch = new UserInfo();
            uiSearch.UserName = context.Request.Form["UserName"];
            string json = JsonHelper.ListToJson<UserInfo>(UserInfoDal.GetList(uiSearch));
            context.Response.Write(json);
        }

        /// <summary>
        /// 根据用户id，获取一条数据
        /// </summary>
        /// <param name="context"></param>
        private void GetById(HttpContext context)
        {
            string idstr = context.Request.Form["sreachid"];
            UserInfo uiSearch = new UserInfo();
            if (!string.IsNullOrWhiteSpace(idstr))
            {
                uiSearch.ID = Convert.ToInt32(idstr);
            }
            string json = JsonHelper.ListToJson<UserInfo>(UserInfoDal.GetList(uiSearch));
            context.Response.Write(json);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        /// <param name="context"></param>
        private void Delete(HttpContext context)
        {
            string id = context.Request.Form["delid"];
            if (context.Session["UserID"].ToString() != id && !string.IsNullOrWhiteSpace(id))
            {
                if (UserInfoDal.Delete(Convert.ToInt32(id)))
                {
                    context.Response.Write("true");
                }
            }
        }


        /// <summary>
        /// 添加或修改用户信息
        /// </summary>
        /// <param name="context"></param>
        private void Save(HttpContext context)
        {
            UserInfo uisave = new UserInfo();
            string idstr=context.Request.Form["userid"];
            if (!string.IsNullOrWhiteSpace(idstr))
            {
                uisave.ID = Convert.ToInt32(idstr);
            }
            uisave.Name = context.Request.Form["Name"];
            uisave.UserName = context.Request.Form["UserName"];
            uisave.Department = context.Request.Form["Department"];
            uisave.Phone = context.Request.Form["Phone"];

            if (uisave.ID > 0)
            {
                //修改用户信息
                if (UserInfoDal.Update(uisave))
                {
                    context.Response.Write("updatetrue");
                }
            }
            else
            {
                //新增用户
                if (UserInfoDal.Insert(uisave))
                {
                    context.Response.Write("addtrue");
                }

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