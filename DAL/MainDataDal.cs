using Model;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common.CommandTrees.ExpressionBuilder;

namespace DAL
{
    public class MainDataDal
    {
        /// <summary>
        /// 根据条件查询交易表
        /// </summary>
        /// <param name="md"></param>
        /// <param name="qc"></param>
        /// <returns></returns>
        public static List<MainData> GetList(MainData md,QueryCon qc)
        {
            string sql = "select top 99 A.*,B.Name as MRName,c.Name as MName,c.ID as MID from MainData A ";
            sql += " left join MerchantRear B on A.FKID = B.ID ";
            sql += " left join MerchantName C on B.FKID = C.ID where 1=1 ";
            if (md.ID > 0)
            {
                sql += string.Format(" and A.id={0} ", md.ID);
            }
            if (!string.IsNullOrWhiteSpace(md.MName))
            {
                sql += string.Format(" and C.Name like '%{0}%' ", md.MName);
            }
            if (!string.IsNullOrWhiteSpace(md.MRName))
            {
                sql += string.Format(" and B.Name like '%{0}%' ", md.MRName);
            }
            if (!string.IsNullOrWhiteSpace(md.RegistrarName))
            {
                sql += string.Format(" and A.RegistrarName like '%{0}%' ", md.RegistrarName);
            }
            if (qc.Mindate > Convert.ToDateTime("1900-01-01"))
            {
                sql += string.Format(" and A.Date >= '{0}'", qc.Mindate);
            }
            if (qc.Maxdate > Convert.ToDateTime("1900-01-01"))
            {
                sql += string.Format(" and A.Date <= '{0}'", qc.Maxdate);
            }
            if (qc.Minmoney > 0)
            {
                sql += string.Format(" and A.money >= '{0}'", qc.Minmoney);
            }
            if (qc.Maxmoney > 0)
            {
                sql += string.Format(" and A.money <= '{0}'", qc.Maxmoney);
            }
            DataTable dt = SqlHelper.Query(sql).Tables[0];
            dt.Columns.Add("Datestr", Type.GetType("System.String"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Datestr"] = Convert.ToDateTime(dt.Rows[i]["Date"]).ToShortDateString();
            }
            return dt.ToList<MainData>();
        }

        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="md"></param>
        /// <returns></returns>
        public static bool Add(MainData md)
        {
            //增加的时候,先查询是否已经存在这条记录,如果存在了,就执行更新,
            string sqlquery = "select A.id from MainData A where A.date='{0}' and A.FKID='{1}'";
            sqlquery = string.Format(sqlquery,md.Date,md.FKID);
            string sql = "";
            if (SqlHelper.Query(sqlquery).Tables[0].Rows.Count == 0)
            {
                sql = "insert into MainData(Date,FKID,Money,RegistrarName) values('{0}','{1}','{2}','{3}')";
                sql = string.Format(sql, md.Date, md.FKID, md.Money, md.RegistrarName);
            }
            else
            {
                sql = "update MainData set Money='{0}',RegistrarName='{1}' where Date='{2}' and FKID='{3}'";
                sql = string.Format(sql, md.Money, md.RegistrarName, md.Date, md.FKID);
            }
            return SqlHelper.ExecuteSql(sql) > 0;
        }

        public static bool Update(MainData md)
        {
            string sql = "update MainData set Date='{0}',FKID='{1}',Money='{2}',RegistrarName='{3}' where ID='{4}'";
            sql = string.Format(sql, md.Date, md.FKID, md.Money, md.RegistrarName, md.ID);
            return SqlHelper.ExecuteSql(sql) > 0;
        }

        public static bool Delete(int id)
        {
            string sql = "delete from MainData where ID={0}";
            sql = string.Format(sql, id);
            return SqlHelper.ExecuteSql(sql) > 0;
        }


        /// <summary>
        /// 根据一个日期查询出该日期下的所有交易记录
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static List<MainData> SearchByDate(DateTime date)
        {
            string sql = "select * from  MainData  where date='{0}'";
            sql = string.Format(sql, date.Date);
            return SqlHelper.Query(sql).Tables[0].ToList<MainData>();
        }


        #region 手机端查询数据用


        /// <summary>
        /// 给定一个开始日期和结束日期，返回该日期内，POS总商户的收款总额倒序
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static List<MainData> GetDayData(DateTime startdate,DateTime enddate)
        {

            string sql = "select '{0}' as Datestr,SUM(A.Money) as Money,C.Name as MName from MainData A ";
            sql += " Left Join MerchantRear B on A.FKID =B.ID ";
            sql += " Left Join MerchantName C on C.ID=B.FkID ";
            sql += " where  A.Date>='{1}' AND A.Date<'{2}' ";
            sql += " GRoup by C.ID,C.Name order by SUM(A.Money) desc ";

            sql = string.Format(sql, startdate.ToShortDateString() + "(+" + (enddate - startdate).Days.ToString() + ")", startdate.ToShortDateString(), enddate.ToShortDateString());

            return SqlHelper.Query(sql).Tables[0].ToList<MainData>();

        }


        /// <summary>
        /// 给定一个开始日期和结束日期，返回该日期内，POS总商户下分商户的收款总额倒序
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static List<MainData> GetAllData(DateTime startdate, DateTime enddate)
        {
            string sql = "select '{0}' as Datestr,SUM(A.Money) as Money,C.Name as MName,B.Name as MRName from MainData A ";
            sql += " Left Join MerchantRear B on A.FKID =B.ID ";
            sql += " Left Join MerchantName C on C.ID=B.FkID ";
            sql += " where 1=1 AND A.Date>='{1}' AND A.Date<'{2}' ";
            sql += " GRoup by B.ID,C.Name,B.Name order by SUM(A.Money) desc";

            sql = string.Format(sql, startdate.ToShortDateString() + "(+" + (enddate - startdate).Days.ToString() + ")", startdate.ToShortDateString(), enddate.ToShortDateString());

            return SqlHelper.Query(sql).Tables[0].ToList<MainData>();
        }

        /// <summary>
        /// 计算某段时间内的交易总额
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <returns></returns>
        public static decimal GetSumMoney(DateTime startdate, DateTime enddate)
        {
            string sql = "select SUM(A.Money) as MoneySum from MainData A  where 1=1 AND A.Date>='{0}' AND A.Date<'{1}'";
            sql = string.Format(sql,startdate.ToShortDateString(),enddate.ToShortDateString());

            decimal summoney = 0;
            string moneystr = SqlHelper.Query(sql).Tables[0].Rows[0][0].ToString();
            if (!string.IsNullOrWhiteSpace(moneystr))
            {
                summoney = Convert.ToDecimal(moneystr);
            }
            return summoney;
        }


        /// <summary>
        /// 环比分析，给一个开始时间一个结束时间，一个POS商户总ID，分析在指定时间指定商户的交易额
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static List<MainData> GetHBData(DateTime startdate, DateTime enddate, int id)
        {
            string sql = "select top 99 Date,Money,B.Name as MRName from MainData A  ";
            sql += "  Left Join MerchantRear B on A.FKID =B.ID ";
            sql += "   Left Join MerchantName C on C.ID=B.FkID ";
            sql += " where  A.Date>='{0}' AND A.Date<'{1}' AND C.ID={2} order by Money desc";
            sql = string.Format(sql, startdate.ToShortDateString(), enddate.ToShortDateString(), id);
            DataTable dt = SqlHelper.Query(sql).Tables[0];
            dt.Columns.Add("Datestr", Type.GetType("System.String"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Datestr"] = Convert.ToDateTime(dt.Rows[i]["Date"]).ToShortDateString();
            }
            return dt.ToList<MainData>();

        }

        /// <summary>
        /// 环比总额分析，给一个开始时间一个结束时间，一个POS商户总ID，分析在指定时间指定商户的交易总额额
        /// </summary>
        /// <param name="startdate"></param>
        /// <param name="enddate"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static decimal GetHBsum(DateTime startdate, DateTime enddate, int id)
        {
            string sql = "select SUM(A.Money) as MoneySum from MainData A  ";
            sql += "  Left Join MerchantRear B on A.FKID =B.ID ";
            sql += "   Left Join MerchantName C on C.ID=B.FkID ";
            sql += "where A.Date>='{0}' AND A.Date<'{1}' And C.ID='{2}'";
            sql = string.Format(sql, startdate.ToShortDateString(), enddate.ToShortDateString(),id);

            decimal summoney = 0;
            string moneystr = SqlHelper.Query(sql).Tables[0].Rows[0][0].ToString();
            if (!string.IsNullOrWhiteSpace(moneystr))
            {
                summoney = Convert.ToDecimal(moneystr);
            }
            return summoney;

        }


        /// <summary>
        /// 通过给定的年份信息，获取该年内有交易的月份的每月交易总额统计
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DataTable GetMonthByYear(string year)
        {
            string sql = "select MONTH(date) as month, sum(money) as money  from MainData ";
            sql += "  where YEAR(date)='{0}' ";
            sql += "  Group by MONTH(date)  ";
            sql += "  order by MONTH(date) ";

            sql = string.Format(sql, year);

            return SqlHelper.Query(sql).Tables[0];
        }

        /// <summary>
        /// 通过给定的月份信息，获取该月内有交易的日期的交易总额统计
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static DataTable GetDayByMonth(DateTime month)
        {
            string sql = "select DAY(date) as day, sum(money) as money  from MainData  ";
            sql += "  where YEAR(date)='{0}' and  MONTH(date)='{1}' ";
            sql += "  Group by DAY(date)  ";
            sql += "  order by DAY(date) ";

            sql = string.Format(sql, month.Year.ToString(),month.Month.ToString());

            return SqlHelper.Query(sql).Tables[0];
        }

        #endregion
    }
}
