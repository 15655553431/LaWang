using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 拉网科技数据展示平台.Ashx
{
    public partial class CheckCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string checkcode = Request["checkcode"];
            if ((Session["CheckCode"] != null) && (Session["CheckCode"].ToString() != ""))
            {
                string t = Session["CheckCode"].ToString().ToLower();
                if (Session["CheckCode"].ToString().ToLower() != checkcode.ToLower())
                {
                    Response.Write("false");
                }
                else
                {
                    Response.Write("true");
                }
            }
            else
            {
                Response.Write("false");
            }

        }
    }
}