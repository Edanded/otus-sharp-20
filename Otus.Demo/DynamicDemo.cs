using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace Otus.Demo
{

    public class TrashItem { }

    public class Can : TrashItem { }

    public class Paper : TrashItem { }

public class TrashDispancer
{
    public void Dispance(TrashItem item)
    => DispancePlease(item as dynamic);

    private void DispancePlease(Can can)
    => Console.WriteLine("Dispancing can");

    private void DispancePlease(Paper paper)
    => Console.WriteLine("Dispancing paper");
}




    public class DynamicDemo : IBaseDemo
    {

        class Bar
        {
            public Bar(string t)
                => _text = t;
            private string _text;

            public override string ToString()
            => $"{{Text: \"{_text}\"}}";
        }


interface IVehicle { }

class Car : IVehicle
{
    public string Engine;
}

private IVehicle V8() => new Car { Engine = "V8" };

        private void BasicExpandoDemo()
        {
            var eo = new ExpandoObject();
            dynamic d = eo;
            eo.TryAdd("A", 2);
            eo.TryAdd("B", 2);
            eo.TryAdd("Foo", new Func<string>(() => $"Hello Workd A={d.A} B={d.B}"));

            Console.WriteLine(d.Foo());

        }

        /// <summary>
        /// Добавление поей в Expando при помощи интерфейса словаря
        /// </summary>
        private void ExpandWithDict()
        {
            var eo = new ExpandoObject();

            var dict = (IDictionary<string, object>)eo;
            dict.Add("Field2Check", 2);
            if (dict.ContainsKey("Field2Check"))
            {
                Console.WriteLine("Has Field2Check");
            }
        }

        private void BasicDemo()
        {
            dynamic s = 2;

            Console.WriteLine($"Type={s.GetType()}, value={s}");
            Console.WriteLine($"s + 2 = {s + 2}");


            s = new Bar("I'm Text");
            Console.WriteLine($"s = {s}");


            // Ошибка компиляции
            dynamic v8 = V8();
            Console.WriteLine($"{v8.Engine}");
        }

        public void Show()
        {
            BasicDemo();
            BasicExpandoDemo();

            ExpandWithDict();

            var d = new TrashDispancer();

            d.Dispance(new Can());
            d.Dispance(new Paper());


            var accessTree = new AccessTreeProvider();
            var tree = accessTree.GetAccessTree();

        }
    }

    class ExcellDemo
    {
        public void Demo()
        {
            var excelApp = new Excel.Application
            {
                Visible = true
            };

            excelApp.Workbooks.Add();


            var workSheet = (Excel.Worksheet)excelApp.ActiveSheet;
            var r = workSheet.Cells[1, 'A'];
            dynamic column = workSheet.Columns[1];
            column.AutoFit();

        }
    }


    class AccessTreeProvider
    {
        class AccessRow
        {

            public AccessRow() { }

            public AccessRow(string alias, bool read, bool write)
            {
                Alias = alias;
                Read = read;
                Write = write;
            }
            public string Alias { get; set; }

            public bool Read { get; set; }

            public bool Write { get; set; }
        }


        /// <summary>
        /// Имитация базы данных
        /// </summary>
        private List<AccessRow> _data = new List<AccessRow>
        {

            new AccessRow("Order.Price",read:true, write:false),
            new AccessRow("Order.Quantity",read:true, write:true),
            new AccessRow("Order.Item",read:true, write:true),
            new AccessRow("Order",read:true, write:true),
            new AccessRow("Order.Item.InnerCode",read:false, write:false),
            new AccessRow("Order.Item.Name",read:false, write:true),

            /* 
             * 
             * 
             * 
             * Должно все выглядеть в итоге вот так
             * 
             * 
             * {
             *      Order:{
             *          read: true,
             *          write: true,
             *          Price: {
             *              read: true, 
             *              write:false,
             *          },
             *          Quantity: {
             *              read: true,
             *              write: true, 
             *          },
             *          Item: {
             *              read: true, 
             *              write: false,
             *              InnerCode: {
             *                  read: false,
             *                  write: false,
             *              },
             *              Name: {
             *                  read: false, 
             *                  write: true,
             *              }
             *          },
             * }
             */
        };

        public ExpandoObject GetAccessTree()
        {
            var res = new ExpandoObject();
            foreach (var d in _data)
            {
                var parts = d.Alias.Split('.').AsEnumerable();
                var e = parts.GetEnumerator();
                e.MoveNext();
                ProcessNode(res, e, d);
            }

            return res;

        }

        private void ProcessNode(
            ExpandoObject node,
            IEnumerator<string> enumerator,
            AccessRow row)
        {

            //
            var dict = node as IDictionary<string, object>;
            // Возвращаем текущее знчение в вспике
            var currentValue = enumerator.Current;


            ExpandoObject newNode;
            if (dict.ContainsKey(currentValue))
            {
                newNode = (ExpandoObject)dict[currentValue];
            }
            else
            {
                newNode = new ExpandoObject();
                dict.Add(currentValue, newNode);
            }

            if (enumerator.MoveNext())
            {
                ProcessNode(newNode, enumerator, row);
            }
            else
            {
                newNode.TryAdd("read", row.Read);
                newNode.TryAdd("write", row.Write);
            }

        }


    }
}