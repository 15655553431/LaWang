using Common;
using DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace 拉网科技数据展示平台.Ashx
{
    /// <summary>
    /// mpMainDataList 的摘要说明
    /// </summary>
    public class mpMainDataList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string action = context.Request.QueryString["action"];

            switch (action)
            {
                case "getsum": GetSumMoney(context); break;
                case "get2018sum": Get2018sum(context); break;
                case "getdaydata": GetDayData(context); break;
                case "gethbdata": GetHBdata(context); break;
                case "gethbsum": GetHBsumdata(context); break;
                case "getweekdata": GetWeekData(context); break;
                case "getmonthdata": GetMonthData(context); break;
                case "getyeardata": GetYearData(context); break;
                case "getmralldata": GetMRAllData(context); break;
                case "getsumbydate": GetSumByDate(context); break;
                case "getanalyzedate": GetAnalyzeData(context); break;
                case "getyesterdaydate": GetYesterdayData(context); break;
                case "getyesterdaysumdate": GetYesterdaysumData(context); break;
                case "getmonthbyyear": GetMonthByYear(context); break;
               
            }
        }



        /// <summary>
        /// 通过给定的年份信息，获取该年份有交易的月交易总计信息
        /// </summary>
        /// <param name="context"></param>
        public void GetMonthByYear(HttpContext context)
        {
            string yearstr = context.Request.Form["year"];
            if (string.IsNullOrWhiteSpace(yearstr) || yearstr.Length != 4)
            {
                yearstr = DateTime.Now.Year.ToString();
            }

            DataTable dt = MainDataDal.GetMonthByYear(yearstr);

            string json = "";
            foreach (DataRow row in dt.Rows)
            {
                json += row["month"].ToString();
                json += ",";
                json += row["money"].ToString();

                json += ";";
            }
            json = json.Substring(0, json.Length - 1);

            context.Response.Write(json);
        }

        /// <summary>
        /// 环比总额分析
        /// </summary>
        /// <param name="context"></param>
        private void GetHBsumdata(HttpContext context)
        {
            string startdate = context.Request.Form["startdate"];
            string enddate = context.Request.Form["enddate"];
            DateTime startsearch = DateTime.Now.AddDays(-30);
            if (!string.IsNullOrWhiteSpace(startdate))
            {
                startsearch = Convert.ToDateTime(startdate);
            }
            DateTime endsearch = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(enddate))
            {
                endsearch = Convert.ToDateTime(enddate);
            }
            string midstr = context.Request.Form["mid"];
            context.Response.Write(MainDataDal.GetHBsum(startsearch,endsearch,Convert.ToInt32(midstr)));
        }



        /// <summary>
        /// 环比分析
        /// </summary>
        /// <param name="context"></param>
        private void GetHBdata(HttpContext context)
        {
            string startdate = context.Request.Form["startdate"];
            string enddate = context.Request.Form["enddate"];
            DateTime startsearch = DateTime.Now.AddDays(-30);
            if (!string.IsNullOrWhiteSpace(startdate))
            {
                startsearch = Convert.ToDateTime(startdate);
            }
            DateTime endsearch = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(enddate))
            {
                endsearch = Convert.ToDateTime(enddate);
            }
            string midstr = context.Request.Form["mid"];
            string json = JsonHelper.ListToJson<MainData>(MainDataDal.GetHBData(startsearch, endsearch, Convert.ToInt32(midstr)));
            context.Response.Write(json);
        }

        /// <summary>
        /// 获得2018年至今的总交易额
        /// </summary>
        /// <param name="context"></param>
        private void Get2018sum(HttpContext context)
        {
            context.Response.Write(MainDataDal.GetSumMoney(Convert.ToDateTime("2018-01-01"), DateTime.Now.Date).ToString());
        }

        /// <summary>
        /// 获取指定时间内交易总额
        /// </summary>
        /// <param name="context"></param>
        private void GetSumMoney(HttpContext context)
        {
            string startdate = context.Request.Form["startdate"];
            string sumtype = context.Request.Form["type"];

            DateTime startsearch = DateTime.Now.AddDays(-1);

            DateTime endsearch = startsearch.AddDays(1);
            if (!string.IsNullOrWhiteSpace(sumtype))
            {
                if (sumtype == "1")
                {
                    startsearch = DateTime.Now.AddDays(-1);
                    if (!string.IsNullOrWhiteSpace(startdate))
                    {
                        startsearch = Convert.ToDateTime(startdate);
                    }
                    endsearch = startsearch.AddDays(1);
                }
                else if (sumtype == "2")
                {
                    startsearch = DateTime.Now.AddDays(-7);
                    if (!string.IsNullOrWhiteSpace(startdate))
                    {
                        startsearch = Convert.ToDateTime(startdate);
                    }
                    endsearch = startsearch.AddDays(7);
                }
                else if (sumtype == "3")
                {
                    startsearch = DateTime.Now.AddMonths(-1);
                    if (!string.IsNullOrWhiteSpace(startdate))
                    {
                        startsearch = Convert.ToDateTime(startdate);
                    }
                    endsearch = startsearch.AddMonths(1); 
                }
                else if(sumtype == "4")
                {
                    startsearch = DateTime.Now.AddYears(-1);
                    if (!string.IsNullOrWhiteSpace(startdate))
                    {
                        startsearch = Convert.ToDateTime(startdate);
                    }
                    endsearch = startsearch.AddYears(1);
                }
            }

            context.Response.Write(MainDataDal.GetSumMoney(startsearch, endsearch).ToString());

        }



        /// <summary>
        /// 日数据统计
        /// </summary>
        /// <param name="context"></param>
        private void GetDayData(HttpContext context)
        {
            string startdate = context.Request.Form["startdate"];
            DateTime startsearch = DateTime.Now.AddDays(-1);
            if (!string.IsNullOrWhiteSpace(startdate))
            {
                startsearch = Convert.ToDateTime(startdate);
            }
            DateTime endsearch = startsearch.AddDays(1);

            string json = JsonHelper.ListToJson<MainData>(MainDataDal.GetDayData(startsearch, endsearch));
            context.Response.Write(json);
        }


        /// <summary>
        /// 周数据统计
        /// </summary>
        /// <param name="context"></param>
        private void GetWeekData(HttpContext context)
        {
            string startdate = context.Request.Form["startdate"];
            DateTime startsearch = DateTime.Now.AddDays(-7);
            if (!string.IsNullOrWhiteSpace(startdate))
            {
                startsearch = Convert.ToDateTime(startdate);
            }
            DateTime endsearch = startsearch.AddDays(7);

            string json = JsonHelper.ListToJson<MainData>(MainDataDal.GetDayData(startsearch, endsearch));
            context.Response.Write(json);
        }


        /// <summary>
        /// 月数据统计
        /// </summary>
        /// <param name="context"></param>
        private void GetMonthData(HttpContext context)
        {
            string startdate = context.Request.Form["startdate"];
            DateTime startsearch = DateTime.Now.AddMonths(-1);
            if (!string.IsNullOrWhiteSpace(startdate))
            {
                startsearch = Convert.ToDateTime(startdate);
            }
            DateTime endsearch = startsearch.AddMonths(1);

            string json = JsonHelper.ListToJson<MainData>(MainDataDal.GetDayData(startsearch, endsearch));
            context.Response.Write(json);
        }


        /// <summary>
        /// 年数据统计
        /// </summary>
        /// <param name="context"></param>
        private void GetYearData(HttpContext context)
        {
            string startdate = context.Request.Form["startdate"];
            DateTime startsearch = DateTime.Now.AddYears(-1);
            if (!string.IsNullOrWhiteSpace(startdate))
            {
                startsearch = Convert.ToDateTime(startdate);
            }
            DateTime endsearch = startsearch.AddYears(1);

            string json = JsonHelper.ListToJson<MainData>(MainDataDal.GetDayData(startsearch, endsearch));
            context.Response.Write(json);
        }


        /// <summary>
        /// 数据自主统计
        /// </summary>
        /// <param name="context"></param>
        private void GetMRAllData(HttpContext context)
        {
            string startdate = context.Request.Form["startdate"];
            string enddate = context.Request.Form["enddate"];
            DateTime startsearch = DateTime.Now.AddDays(-7);
            if (!string.IsNullOrWhiteSpace(startdate))
            {
                startsearch = Convert.ToDateTime(startdate);
            }
            DateTime endsearch = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(enddate))
            {
                endsearch = Convert.ToDateTime(enddate);
            }
            string json = JsonHelper.ListToJson<MainData>(MainDataDal.GetAllData(startsearch, endsearch));
            context.Response.Write(json);
        }

        /// <summary>
        /// 获取指定范围内的总交易额
        /// </summary>
        /// <param name="context"></param>
        private void GetSumByDate(HttpContext context)
        {
            string startdate = context.Request.Form["startdate"];
            string enddate = context.Request.Form["enddate"];
            DateTime startsearch = DateTime.Now.AddDays(-7);
            if (!string.IsNullOrWhiteSpace(startdate))
            {
                startsearch = Convert.ToDateTime(startdate);
            }
            DateTime endsearch = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(enddate))
            {
                endsearch = Convert.ToDateTime(enddate);
            }
            context.Response.Write(MainDataDal.GetSumMoney(startsearch, endsearch).ToString());
            
        }


        /// <summary>
        /// 同比分析
        /// </summary>
        /// <param name="context"></param>
        private void GetAnalyzeData(HttpContext context)
        {
            string startdate = context.Request.Form["startdate"];
            string enddate = context.Request.Form["enddate"];
            DateTime startsearch = DateTime.Now.AddDays(-7);
            if (!string.IsNullOrWhiteSpace(startdate))
            {
                startsearch = Convert.ToDateTime(startdate);
            }
            DateTime endsearch = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(enddate))
            {
                endsearch = Convert.ToDateTime(enddate);
            }
            string json = JsonHelper.ListToJson<MainData>(MainDataDal.GetDayData(startsearch, endsearch));
            context.Response.Write(json);
        }


        /// <summary>
        /// 昨日统计
        /// </summary>
        /// <param name="context"></param>
        private void GetYesterdayData(HttpContext context)
        {
            string json = JsonHelper.ListToJson<MainData>(MainDataDal.GetAllData(DateTime.Now.AddDays(-1), DateTime.Now));
            context.Response.Write(json);
        }

        /// <summary>
        /// 昨日成交额
        /// </summary>
        /// <param name="context"></param>
        private void GetYesterdaysumData(HttpContext context)
        {
            context.Response.Write(MainDataDal.GetSumMoney(DateTime.Now.AddDays(-1), DateTime.Now).ToString());
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