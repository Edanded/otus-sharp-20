#define STOP1

#if DEBUG

#undef STOP1

#endif
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
            var cl = new Class2();
#if STOP1
#error У нас ошибка
#endif
            //    new UnsafeDemo().Show();

        }
    }
}
