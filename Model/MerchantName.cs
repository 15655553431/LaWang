using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MerchantName
    {
      //  ,[Name] 商户名
      //,[Number] 商户编号
      //,[Value1] 备用
      //,[Value2]
      //,[Value3]
      //,[Value4]

        public int ID { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
    }
}
