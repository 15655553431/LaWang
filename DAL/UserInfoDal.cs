using Common;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserInfoDal
    {
        /// <summary
        /// 校验用户名和密码是否正确
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public static bool Login(string UserName, string PassWord)
        {
            string sql = "select ID from UserInfo where UserName='{0}' and PassWord='{1}'";
            sql = string.Format(sql, UserName, Md5Helper.GetMd5(PassWord));
            return SqlHelper.Query(sql).Tables[0].Rows.Count > 0;

        }

        /// <summary>
        /// 根据用户名和密码获取用户ID,如果不存在就返回-1
        /// </summary>
        /// <returns></returns>
        public static string GetUserInfoID(string UserName, string PassWord)
        {
            string sql = "select ID from UserInfo where UserName='{0}' and PassWord='{1}'";
            sql = string.Format(sql, UserName,PassWord.GetMd5());
            string idstr = "-1";
            DataTable dt = SqlHelper.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                idstr = dt.Rows[0]["id"].ToString();
            }
            return idstr;
        }

        /// <summary>
        /// 校验用户名是否存在
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public static bool CheckUserName(string UserName)
        {
            string sql = "select ID from UserInfo where UserName='{0}'";
            sql = string.Format(sql, UserName);
            return SqlHelper.Query(sql).Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// 获取userinfo
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public static List<UserInfo> GetList(UserInfo ui)
        {
            string sql = "SELECT * FROM UserInfo where 1=1 ";
            if (ui.ID > 0)
            {
                sql += string.Format(" and id={0} ", ui.ID);
            }
            if (!string.IsNullOrWhiteSpace(ui.UserName))
            {
                sql += string.Format(" and UserName like '%{0}%' ", ui.UserName);
            }
            return SqlHelper.Query(sql).Tables[0].ToList<UserInfo>() ;
        }

        /// <summary>
        /// 插入用户数据
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public static bool Insert(UserInfo ui)
        {
            string sql = "insert into UserInfo(Name,Department,UserName,Phone,PassWord,Type,RegTime) ";
            sql += "values('{0}','{1}','{2}','{3}','{4}','1',GETDATE())";
            sql = string.Format(sql,ui.Name,ui.Department,ui.UserName,ui.Phone,"123456".GetMd5());
            return SqlHelper.ExecuteSql(sql) > 0;
        }



        /// <summary>
        /// 更新用户数据
        /// </summary>
        /// <param name="ui"></param>
        /// <returns></returns>
        public static bool Update(UserInfo ui)
        {
            string sql = "update UserInfo set Name='{0}',UserName='{1}',Department='{2}',Phone='{3}' where ID={4}";
            sql = string.Format(sql, ui.Name, ui.UserName, ui.Department, ui.Phone, ui.ID);
            return SqlHelper.ExecuteSql(sql) > 0;
        }

        /// <summary>
        /// 根据id删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Delete(int id)
        {
            string sql = "delete from UserInfo where ID={0}";
            sql = string.Format(sql,id);
            return SqlHelper.ExecuteSql(sql) > 0;
        }

        /// <summary>
        /// 设置新密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool UpdatePwd(int id,string newpwd)
        {
            string sql = "update UserInfo set PassWord='{0}'  where ID={1} ";
            sql = string.Format(sql, newpwd.GetMd5(),id);
            return SqlHelper.ExecuteSql(sql) > 0;
        }


    }
}
