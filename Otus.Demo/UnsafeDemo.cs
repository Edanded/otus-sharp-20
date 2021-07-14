using System;

namespace Otus.Demo
{
    class Foo { }
    public class UnsafeDemo : IBaseDemo
    {
        private long ticks = 1_000_000;
        public unsafe void CheckUnsafeAllocation()
        {
            long l = 0;
            for (var i = 0; i < ticks; i++)
            {
                var r = new Foo();
            }
        }

        public void CheckNormalAllocation()
        {
            long l = 0;
            for (var i = 0; i < ticks; i++)
            {
                var r = new Foo();
            }
        }

        private void Move()
        {
            var a = new int[5] { 10, 20, 30, 40, 50 };
            // Must be in unsafe code to use interior pointers.
            unsafe
            {
                // Must pin object on heap so that it doesn't move while using interior pointers.
                // Ф
                fixed (int* p = &a[0])
                {

                    // p is pinned as well as object, so create another pointer to show incrementing it.
                    int* p2 = p;
                    Console.WriteLine(*p2);
                    // Incrementing p2 bumps the pointer by four bytes due to its type ...
                    p2 += 1;
                    Console.WriteLine(*p2);
                    p2 += 1;
                    Console.WriteLine(*p2);
                    Console.WriteLine("--------");
                    Console.WriteLine(*p);
                    // Dereferencing p and incrementing changes the value of a[0] ...
                    *p += 1;
                    Console.WriteLine(*p);
                    *p += 1;
                    Console.WriteLine(*p);
                }
            }

            Console.WriteLine("--------");
            Console.WriteLine(a[0]);
        }

        private unsafe void PrintWord()
        {
            string s = "Привет";



            fixed (char* p = s)
            {

                char* p1 = &p[9];
                Console.WriteLine($"address {(long)p1} {(long)(p1 + 1)}");
                for (var i = 0; i <= 10; i++)
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

            // Присваиваем p - адрес в памяти
            // где хранится second 
            p = q;

            second = 5;

            Console.WriteLine($"*p = {*p}");
        }

        public void Show()
        {
            PrintWord();
            ShowSimple();
            ShowSwap();
            return;

        }
    }
}
