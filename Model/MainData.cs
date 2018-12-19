using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
  //  SELECT TOP 1000 [ID]
  //    ,[Date]
  //    ,[FKID]
  //    ,[Money]
  //    ,[RegistrarName]
  //    ,[Value1]
  //    ,[Value2]
  //    ,[Value3]
  //FROM [LaWang].[dbo].[MainData]

    public class MainData
    {
        public int ID { get; set; }
        public int FKID { get; set; }
        public DateTime Date { get; set; }
        public decimal Money { get; set; }
        public string RegistrarName { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }


        //以下是数据库中没有的，用来查询结果存储用的
        public string MName { get; set; }
        public string MRName { get; set; }
        public int MID { get; set; }

        //存储时间用的
        public string Datestr { get; set; }

        public MainData()
        {
            this.Date = DateTime.Now;
        }

    }
}
