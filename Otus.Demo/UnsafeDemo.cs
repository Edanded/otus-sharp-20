using System;
using System.Diagnostics;
using System.Linq;

namespace Otus.Demo
{
    class Foo { }
    public class UnsafeDemo : IBaseDemo
    {

#line 1

        private long ticks = 1_000_000;



        public string RandomString(int length)
        {
            var r = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => 'a').ToArray());
        }




        private void CheckSpeed()
        {
            var s = RandomString(30_000_000);
            var stopwatch = new Stopwatch();


            var a = 0;
            var sLength = s.Length;
            unsafe
            {
                stopwatch.Start();
                fixed (char* p = s)
                {
                    char* q = p;
                    while (*q != '\0')
                    {
                        var f = *q;
                        //    a++;
                        q++;
                    }
                }
                stopwatch.Stop();
                Console.WriteLine($"pointer = {stopwatch.ElapsedMilliseconds}");

            }

            stopwatch.Reset();
            stopwatch.Start();
            a = 0;

            //while (a < sLength)
            //{
            //    var f = s[a];
            //    a++;
            //}

            var en = s.GetEnumerator();

            while (en.MoveNext())
            {
              var  f = s[a];
                a++;
            }

            stopwatch.Stop();
            Console.WriteLine($"clr = {stopwatch.ElapsedMilliseconds}");
        }





        private void Move()
        {
            var a = new int[5] { 10, 20, 30, 40, 50 };

            unsafe
            {
                fixed (int* p = &a[0])
                {


                    int* p2 = p;
                    Console.WriteLine(*p2);

                    p2 += 1;
                    Console.WriteLine(*p2);
                    p2 += 1;
                    Console.WriteLine(*p2);
                    Console.WriteLine("--------");
                    Console.WriteLine(*p);

                    *p += 1;
                    Console.WriteLine(*p);
                    *p += 1;
                    Console.WriteLine(*p);
                }
            }

            Console.WriteLine("--------");
            Console.WriteLine(a[0]);
        }

        private void SafeRefs()
        {
            int a = 2;

            ref int p = ref a;

            a = 5;
            Console.WriteLine($"p is {p}");

        }

        private unsafe void PrintWord()
        {
            string s = "Привет";




            //      2Б  2Б  2Б  2Б  2Б  2Б
            // p -> [П] [р] [и] [в] [е] [т]


            fixed (char* p = s)
            {

                char* p1 = &p[2];
                Console.WriteLine($"address current {(long)p1} next {(long)(p1 + 1)}");
                for (var i = 0; p[i] != '\0'; i++)
                {
                    Console.WriteLine($"Values is {p[i]} memory address {(long)&p[i]}");
                }

            }
        }

        /// <summary>
        /// Обмен значений
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        void NormalSwap(ref int a, ref int b)
        {
            int c = a;
            a = b;
            b = c;
        }

        unsafe void PSwap(int* a, int* b)
        {
            int c = *a;
            *a = *b;
            *b = c;
        }


        private unsafe void ShowSwap()
        {
            int a = 2, b = 3;
            PSwap(&a, &b);
            Console.WriteLine($"a={a} b={b}");

        }

        private unsafe void ShowSimple()
        {
            var first = 1;
            var second = 4;
            // Указатель на адрес памяти 
            // где содержится значение i
            int* p = &first;

            int* q = &second;

            // В адрес, куда указывает p записываем 2
            // (а там же - место где хранится переменная i)
            *p = 2;


            Console.WriteLine($"first = {first}");

            first = 555;
            Console.WriteLine($"*p = {*p}");
            // Присваиваем p - адрес в памяти
            // где хранится second 
            p = q;

            second = 5;

            Console.WriteLine($"*p = {*p}");
        }

        public void Show()
        {

            CheckSpeed();
            return;
            PrintWord();
            ShowSimple();
            ShowSwap();


        }
    }
}
