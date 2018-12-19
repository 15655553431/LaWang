using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// 存储的是Maindata表查询条件，
    /// </summary>
    public class QueryCon
    {
        public DateTime Mindate { get; set; }
        public DateTime Maxdate { get; set; }
        public decimal Minmoney { get; set; }
        public decimal Maxmoney { get; set; }
    }
}
