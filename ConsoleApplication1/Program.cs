using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string loginName = "fbamembershipprovider|xueqingxia";
            loginName = loginName.Substring(loginName.IndexOf("|") + 1);
            string login = loginName.Substring(loginName.IndexOf("\\") + 1).ToLower();
            Console.WriteLine(login );
        }
    }
}
