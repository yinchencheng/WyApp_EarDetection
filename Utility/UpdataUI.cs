using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WY_App.Utility
{
    internal class UpdataUI
    {
        public class MoreTime
        {
            public delegate void InvokeOtherThead(int i);//委托


            public InvokeOtherThead MainThread;//事件


            public void WaitMoreTime()
            {
                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(1);
                    MainThread(i);//调用事件
                }
            }
        }
    }
}
