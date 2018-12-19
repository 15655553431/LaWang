using Model;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MerchantRearDal
    {
        /// <summary>
        /// 获取商户后台信息
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public static List<MerchantRear> GetList(MerchantRear mr)
        {
            string sql = "SELECT A.*,B.Name as MName FROM MerchantRear A left join MerchantName B on A.FkID=B.ID where 1=1 ";
            if (mr.ID > 0)
            {
                sql += string.Format(" and A.id={0} ", mr.ID);
            }
            if (mr.FkID > 0)
            {
                sql += string.Format(" and A.fkid={0} ", mr.FkID);
            }
            if (!string.IsNullOrWhiteSpace(mr.Name))
            {
                sql += string.Format(" and A.Name like '%{0}%' ", mr.Name);
            }
            if (!string.IsNullOrWhiteSpace(mr.Number))
            {
                sql += string.Format(" and A.Number like '%{0}%' ", mr.Number);
            }
            if (!string.IsNullOrWhiteSpace(mr.MName))
            {
                sql += string.Format(" and B.Name like '%{0}%' ", mr.MName);
            }

            sql += " order by A.id";
            return SqlHelper.Query(sql).Tables[0].ToList<MerchantRear>();
        }

        //查询商户名
        public static List<MerchantName> GetName()
        {
            string sql = "select * from MerchantName";

            return SqlHelper.Query(sql).Tables[0].ToList<MerchantName>();
        }

        /// <summary>
        /// 插入商户数据
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public static bool Insert(MerchantRear ui)
        {
            string sql = "insert into MerchantRear(Name,Number,FKid) ";
            sql += "values('{0}','{1}','{2}')";
            sql = string.Format(sql, ui.Name, ui.Number,ui.FkID);
            return SqlHelper.ExecuteSql(sql) > 0;
        }

        /// <summary>
        /// 更新商户数据
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public static bool Update(MerchantRear ui)
        {
            string sql = "update MerchantRear set Name='{0}',Number='{1}',Fkid='{2}' where ID={3}";
            sql = string.Format(sql, ui.Name, ui.Number,ui.FkID, ui.ID);
            return SqlHelper.ExecuteSql(sql) > 0;
        }

        /// <summary>
        /// 根据id删除商户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(int id)
        {
            //删除之前先确定，交易记录表是否有关联
            string sqlcon = "select id from MainData where FKID={0}";
            sqlcon = string.Format(sqlcon, id);
            if (SqlHelper.Query(sqlcon).Tables[0].Rows.Count > 0)
            {
                return false;
            }
            else
            {
                string sql = "delete from MerchantRear where ID={0}";
                sql = string.Format(sql, id);
                return SqlHelper.ExecuteSql(sql) > 0;
            }

        }
    }
}
