using Model;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DAL
{
    public class FileRecordDal
    {
        public static List<FileRecord> GetFileList(FileRecord fr)
        {
            string sql = "  select * from FileRecord where 1=1 ";
            if (fr.Type > 0)
            {
                sql += string.Format(" and Type='{0}' ", fr.Type);
            }
            if (fr.ID > 0)
            {
                sql += string.Format(" and ID='{0}' ", fr.ID);
            }
            if (!string.IsNullOrWhiteSpace(fr.FileName))
            {
                sql += string.Format(" and FileName like '%{0}%' ", fr.FileName);
            }

            DataTable dt = new DataTable();
            dt = SqlHelper.Query(sql).Tables[0];
            dt.Columns.Add("FileTimestr", Type.GetType("System.String"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["FileTimestr"] = Convert.ToDateTime(dt.Rows[i]["FileTime"]).ToShortDateString();
            }
            return dt.ToList<FileRecord>();
        }

        public static bool Insert(FileRecord fr)
        {
            string sql = " insert into FileRecord(Type,Url,FileName,FileTime,OperatorName,Count,State) Values({0},'{1}','{2}','{3}','{4}',{5},'{6}')";
            sql = string.Format(sql, fr.Type,fr.Url, fr.FileName, fr.FileTime,fr.OperatorName, fr.Count, fr.State);
            return SqlHelper.ExecuteNonQuery(sql) > 0;
        }
    }
}
