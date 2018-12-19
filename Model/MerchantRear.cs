using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class MerchantRear
    {
        //,[FkID] 外键 
        //  ,[Name] 商户后台名
        //,[Number] 商户后台编号
        //,[Value1] 备用
        //,[Value2]
        //,[Value3]
        //,[Value4]
        
        public int ID { get; set; }
        public int FkID { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }

        public string MName { get; set; }
    }
}
