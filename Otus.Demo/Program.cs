using Otus.Lib4Preprocessor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

namespace Otus.Demo
{


    class Demo
    {
        public List<double> L = new List<double>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            //new UnsafeDemo().Show();
            new UnsafeDemo().Show();
#if STOP1
#error У нас ошибка
#endif
            //    new UnsafeDemo().Show();

        }
    }
}
