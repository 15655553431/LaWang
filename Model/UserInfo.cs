using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class UserInfo
    {
      //,[Name]  姓名
      //,[Department]  部门
      //,[UserName]   登陆名
      //,[PassWord]  密码
      //,[Type]  账户类型
      //,[Phone]   手机号
      //,[RegTime]   注册时间
      //,[Value1]   备用1
      //,[Value2]   备用2
      //,[Value3]    备用3
        
        public int ID { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int Type { get; set; }
        public string Phone { get; set; }
        public DateTime RegTime { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }



    }
}
