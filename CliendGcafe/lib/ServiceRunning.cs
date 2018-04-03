using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CCMS.lib
{
    class ServiceRunning
    {
       public static void MethodA()
        {
            int totalTime = (5 * 60000);
            while (true)
            {
                Thread.Sleep(totalTime);
                Helper.refreshMoney();
            }
        }
    }
}
