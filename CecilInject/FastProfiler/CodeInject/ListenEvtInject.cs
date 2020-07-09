using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FastProfiler
{
    public static partial class CodeInject
    {
        public static List<object> a;
        public static void AddListen()
        {
            //最糟糕
            for (int i = 0; i < 100; i++)
            {
                var str2 = "fa";
                A(() =>
                {
                    Console.WriteLine(str2);
                });               
            }
          
            //糟糕
            var str = "fa";
            for (int i = 0; i < 100; i++)
            {
                 A(() =>
                 {
                     Console.WriteLine(str);
                 });               
            }

   
            //可以忍
            for (int i = 0; i < 100; i++)
            {
                A(B);
            }

            //最好
            System.Action temp = B;
            for (int i = 0; i < 100; i++)
            {
                A(temp);
            }
            
            for (int i = 0; i < 100; i++)
            {
                A(()=>
                {
                    Debug.Close();
                });
            }
        }

        private static void A(System.Action a)
        {
            a();
        }

        private static void B()
        {
            
        }
    }
}