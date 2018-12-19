using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace 拉网科技数据展示平台.Ashx
{
    /// <summary>
    /// MainDataList 的摘要说明
    /// </summary>
    public class MainDataList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string action = context.Request.QueryString["action"];

            switch (action)
            {
                case "getlist": ListInfo(context); break;
                case "getposname": GetPosName(context); break;
                case "getrearname": GetRearName(context); break;
                case "save": Save(context); break;
                case "getbyid": GetById(context); break;
                case "delete": Delete(context); break;
                case "deletelot": DeleteLot(context); break;
                

            }
        }


        

        public void DeleteLot(HttpContext context)
        {
            string idstr = context.Request.Form["bigdel"];
            string[] ids = idstr.Split(',');
            bool flag = true;
            string errid = "";
            foreach (string id in ids)
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    if (!MainDataDal.Delete(Convert.ToInt32(id)))
                    {
                        flag = false;
                        errid = id;
                        break;
                    }
                }
            }
            if (flag)
            {
                context.Response.Write("true");
            }
            else
            {
                context.Response.Write(errid);
            }
        }

        public void Delete(HttpContext context)
        {
            string idstr = context.Request.Form["delid"];
            if (!string.IsNullOrWhiteSpace(idstr))
            {
                if (MainDataDal.Delete(Convert.ToInt32(idstr)))
                {
                    context.Response.Write("true");
                }
            }
        }

        public void ListInfo(HttpContext context)
        {
            string mintime = context.Request.Form["min"];
            string maxtime = context.Request.Form["max"];
            string regname = context.Request.Form["name"];
            MainData md = new MainData();
            QueryCon qc = new QueryCon();
            md.RegistrarName = regname;
            if (!string.IsNullOrWhiteSpace(mintime))
            {
                qc.Mindate = Convert.ToDateTime(mintime);
            }
            if (!string.IsNullOrWhiteSpace(maxtime))
            {
                qc.Maxdate = Convert.ToDateTime(maxtime);
            }

            List<MainData> listinfo = new List<MainData>();
            listinfo = MainDataDal.GetList(md, qc);
            string json = JsonHelper.ListToJson<MainData>(listinfo);
            context.Response.Write(json);
        }

        private void GetById(HttpContext context)
        {
            MainData md = new MainData();
            string idstr = context.Request.Form["sreachid"];
            if (!string.IsNullOrWhiteSpace(idstr))
            {
                md.ID = Convert.ToInt32(idstr);
            }
            QueryCon qc = new QueryCon();
            List<MainData> listinfo = new List<MainData>();
            listinfo = MainDataDal.GetList(md, qc);
            string json = JsonHelper.ListToJson<MainData>(listinfo);
            context.Response.Write(json);
        }

        private void Save(HttpContext context)
        {
            MainData mdsave = new MainData();
            string idstr = context.Request.Form["userid"];
            mdsave.FKID = Convert.ToInt32(context.Request["rearname"]);
            mdsave.Date = Convert.ToDateTime(context.Request["dateinput"]);
            mdsave.Money = Convert.ToDecimal(context.Request.Form["money"]);
            mdsave.RegistrarName = context.Session["Name"].ToString();

            if (string.IsNullOrWhiteSpace(idstr))
            {
                //增加
                if (MainDataDal.Add(mdsave))
                {
                    context.Response.Write("addtrue");
                }
            }
            else
            {
                //修改
                mdsave.ID = Convert.ToInt32(idstr);
                if (MainDataDal.Update(mdsave))
                {
                    context.Response.Write("updatetrue");
                }

            }
        }


        public void GetPosName(HttpContext context)
        {
            context.Response.Write(JsonHelper.ListToJson<MerchantName>(MerchantNameDal.GetList(new MerchantName())));
        }

        public void GetRearName(HttpContext context)
        {
            string posid = context.Request.Form["posid"];
            MerchantRear mr = new MerchantRear();
            if (!string.IsNullOrWhiteSpace(posid))
            {
                mr.FkID = Convert.ToInt32(posid);
            }
            List<MerchantRear> listinfo = MerchantRearDal.GetList(mr);
            if (listinfo.Count > 0)
            {
                context.Response.Write(JsonHelper.ListToJson<MerchantRear>(listinfo));
            }
            else
            {
                context.Response.Write("no");
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