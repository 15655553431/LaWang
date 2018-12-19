using Model;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MerchantNameDal
    {
        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public static List<MerchantName> GetList(MerchantName mn)
        {
            string sql = "SELECT * FROM MerchantName where 1=1";
            if (mn.ID > 0)
            {
                sql += string.Format(" and id={0} ", mn.ID);
            }
            if (!string.IsNullOrWhiteSpace(mn.Name))
            {
                sql += string.Format(" and Name like '%{0}%' ", mn.Name);
            }
            if (!string.IsNullOrWhiteSpace(mn.Number))
            {
                sql += string.Format(" and Number like '%{0}%' ", mn.Number);
            }
            sql += " order by id";
            return SqlHelper.Query(sql).Tables[0].ToList<MerchantName>();
        }


        /// <summary>
        /// 插入商户数据
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public static bool Insert(MerchantName ui)
        {
            string sql = "insert into MerchantName(Name,Number) ";
            sql += "values('{0}','{1}')";
            sql = string.Format(sql, ui.Name, ui.Number);
            return SqlHelper.ExecuteSql(sql) > 0;
        }

        /// <summary>
        /// 更新商户数据
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public static bool Update(MerchantName ui)
        {
            string sql = "update MerchantName set Name='{0}',Number='{1}' where ID={2}";
            sql = string.Format(sql, ui.Name, ui.Number, ui.ID);
            return SqlHelper.ExecuteSql(sql) > 0;
        }

        /// <summary>
        /// 根据id删除商户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(int id)
        {
            //删除之前先确定，商户后台表是否有关联
            string sqlcon = "select id from MerchantRear where FKID={0}";
            sqlcon = string.Format(sqlcon, id);
            if (SqlHelper.Query(sqlcon).Tables[0].Rows.Count > 0)
            {
                return false;
            }
            else
            {
                string sql = "delete from MerchantName where ID={0}";
                sql = string.Format(sql, id);
                return SqlHelper.ExecuteSql(sql) > 0;
            }

            
        }


    }
}
