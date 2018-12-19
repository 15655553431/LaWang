using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class FileRecord
    {
      //,[Type]
      //,[FileName]
      //,[FileTime]
      //,[Count]
      //,[State]

        public int ID { get; set; }
        public int Type { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public DateTime FileTime { get; set; }
        public string OperatorName { get; set; }
        public int Count { get; set; }
        public string State { get; set; }

        //记录时间
        public string FileTimestr { get; set; }


        //写一个构造函数
        public FileRecord()
        {
        }
        public FileRecord(int type)
        {
            this.Type = type;
            //1,代表导入记录，
            //2，代表导出记录
        }
    }
}
