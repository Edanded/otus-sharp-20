#define Log
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Demo
{

#nullable enable
    public class NewsStoryViewModel
    {
        public DateTimeOffset Published { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; }
    }
#nullable restore

    public class PreprocessorDemo : IBaseDemo
    {
        public void Show()
        {

#if !DEBUG
#endif

#if Log
            Console.WriteLine("Log");
#endif
#if _WINDOWS
#line 200 "название текста"
#endif
            int i = 2;

#if Abracadabra
#define ALIBABA
            Console.WriteLine("i'm debug");
#else
            Console.WriteLine("i'm release");
#endif
        }
    }
}
