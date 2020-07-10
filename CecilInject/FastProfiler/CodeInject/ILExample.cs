using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FastProfiler
{
    public  class Example
    {
        public  List<object> a;
        public  void AddListen()
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
            
            //装箱
            for (int i = 0; i < 100; i++)
            {
                a.Add(i);
            }
            
            //语法实现
            //https://docs.unity3d.com/ScriptReference/Object.html 
            //This class doesn't support the null-conditional operator (?.) and the null-coalescing operator (??).
            var t = this?.a;
            if (this == null)
            {
                t = this.a;
            }
        }

        private  void A(System.Action a)
        {
            a();
            B();
        }

        private  void B()
        {
 
        }


        private static void Swap(ref int a, ref int b)
        {
            var t = b;
            b = a;
            a = t;
        }

        private static int Sum(int a, int b)
        {
            return a + b;
        }

        private static void Test()
        {
            int a = 3, b = 6;
            Swap(ref a, ref b);
            var ret = Sum(a, b);
        }
    }
}