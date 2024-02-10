using System;
using WarehouseManagementSystemAPI;

namespace HardwareWarehouseManagementSystem
{

    class Program
    {
        const int Batch_Size = 5;
        static void Main(string[] args)
        {
            CustomQueue<HardwareItem> hardwareItemQueue = new CustomQueue<HardwareItem>();
      //委托的一部分
            hardwareItemQueue.CustomQueueEvent += CustomQueue_CustomQueueEvent;
            System.Threading.Thread.Sleep(2000);

            //comes into stock - device scans a bar code or QR code
            hardwareItemQueue.AddItem(new Drill { Id = 1, Name = "Drill 1", Type = "Drill", UnitValue = 20.00m, Quantity = 10 });

            System.Threading.Thread.Sleep(1000);

            hardwareItemQueue.AddItem(new Drill { Id = 2, Name = "Drill 2", Type = "Drill", UnitValue = 30.00m, Quantity = 20 });

            System.Threading.Thread.Sleep(2000);

            hardwareItemQueue.AddItem(new Ladder { Id = 3, Name = "Ladder 1", Type = "Ladder", UnitValue = 100.00m, Quantity = 5 });

            System.Threading.Thread.Sleep(1000);

            hardwareItemQueue.AddItem(new Hammer { Id = 4, Name = "Hammer 1", Type = "Hammer", UnitValue = 10.00m, Quantity = 80 });
            System.Threading.Thread.Sleep(3000);

            hardwareItemQueue.AddItem(new PaintBrush { Id = 5, Name = "Paint Brush 1", Type = "PaintBrush", UnitValue = 5.00m, Quantity = 100 });
            System.Threading.Thread.Sleep(3000);

            hardwareItemQueue.AddItem(new PaintBrush { Id = 6, Name = "Paint Brush 2", Type = "PaintBrush", UnitValue = 5.00m, Quantity = 100 });
            System.Threading.Thread.Sleep(3000);

            hardwareItemQueue.AddItem(new PaintBrush { Id = 7, Name = "Paint Brush 3", Type = "PaintBrush", UnitValue = 5.00m, Quantity = 100 });
            System.Threading.Thread.Sleep(3000);

            hardwareItemQueue.AddItem(new Hammer { Id = 8, Name = "Hammer 2", Type = "Hammer", UnitValue = 11.00m, Quantity = 80 });
            System.Threading.Thread.Sleep(3000);

            hardwareItemQueue.AddItem(new Hammer { Id = 9, Name = "Hammer 3", Type = "Hammer", UnitValue = 13.00m, Quantity = 80 });
            System.Threading.Thread.Sleep(3000);

            hardwareItemQueue.AddItem(new Hammer { Id = 10, Name = "Hammer 4", Type = "Hammer", UnitValue = 14.00m, Quantity = 80 });
            System.Threading.Thread.Sleep(3000);

            Console.ReadKey();

        }
        private static void ProcessItems(CustomQueue<HardwareItem> customQueue)
            //用于处理事物，每次处理完之后线程都会休息3000毫秒
        {
            // 感觉这里有点异步通信的感觉
            while (customQueue.QueueLength > 0)
            {
                System.Threading.Thread.Sleep(3000);   //线程休息了3000毫秒
                HardwareItem hardWareItem = customQueue.GetItem(); 
                //休息完之后调用 getiItem 队列出队新值
            }

        }
        //事件处理程序 CustomQueue_CustomQueueEvent
        private static void CustomQueue_CustomQueueEvent(CustomQueue<HardwareItem> sender, QueueEventArgs eventArgs)
            //sender 是触发事件的对象，即发出事件的对象。在这种情况下，sender 是 CustomQueue<HardwareItem> 类型的实例，表示发出队列事件的自定义队列对象。
            //通过 sender，可以访问触发事件的对象的属性和方法，从而执行相应的操作。
        {
            Console.Clear();  //屏幕进行清零之后

            Console.WriteLine(MainHeading());  //主标题
            Console.WriteLine();
            Console.WriteLine(RealTimeUpdateHeading());   //时间更新

            if (sender.QueueLength > 0)  
            {
              //  打印事件参数中的消息，该消息包含有关队列中添加或处理物品的详细信息。
                Console.WriteLine(eventArgs.Message);
                Console.WriteLine();
                Console.WriteLine();
                //印 "Items Queued for Processing" 标题，表示待处理的物品。
                Console.WriteLine(ItemsInQueueHeading());
                //
                Console.WriteLine(FieldHeadings());

                WriteValuesInQueueToScreen(sender);
     
                if (sender.QueueLength == Batch_Size)
                {
                    ProcessItems(sender);

                }

            }
            else
            {
                Console.WriteLine("Status: All items have been processed.");
            }


        }

        private static void WriteValuesInQueueToScreen(CustomQueue<HardwareItem> hardwareItems)
        {
            
            foreach (var hardwareItem in hardwareItems)
            {
                Console.WriteLine($"{hardwareItem.Id,-6}{hardwareItem.Name,-15}{hardwareItem.Type,-20}{hardwareItem.Quantity,10}{hardwareItem.UnitValue,10}");
            }
        }

        //Headings
        private static string FieldHeadings()  //在文字下面添加一条下划线
            //生成包含硬件物品字段的标题
        {
            return UnderLine($"{"Id",-6}{"Name",-15}{"Type",-20}{"Quantity",10}{"Value",10}");
        }

        private static string RealTimeUpdateHeading()  //
        { //生成包含“实时更新”的标题。
            return UnderLine("Real-time Update");
        }

        private static string ItemsInQueueHeading()
        {
            // 生成包含“待处理物品”的标题。
            
            return UnderLine("Items Queued for Processing");
        }

        private static string MainHeading()
        {
            //生成包含“仓库管理系统”的主标题。
            return UnderLine("Warehouse Management System");
        }

        private static string UnderLine(string heading)
        {
            return $"{heading}{Environment.NewLine}{new string('-', heading.Length)}";
        }
        //Headings

    }

    public abstract class HardwareItem : IEntityPrimaryProperties, IEntityAdditionalProperties
    // 硬件主体 是使用了IEntityPrimary IEntity这些接口
    {
        public int Id { get; set; }     
        public string Name { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }   
        public decimal UnitValue { get; set;  }
    }

    public interface IDrill //钻头
    { 
        string DrillBrandName { get; set; } //属性
    }
    public class Drill : HardwareItem, IDrill  
    {
        public string DrillBrandName { get; set; }   //继承了接口之后并把方法重写
    
    }
    public interface ILadder //梯子
    {
        string LadderBrandName { get; set; }
    }
    public class Ladder : HardwareItem, ILadder 
    {
        public string LadderBrandName { get; set; }

    }
    public interface IPaintBrush   //油漆刷
    {
        string PaintBrushBrandName { get; set; }
    }
    public class PaintBrush : HardwareItem, IPaintBrush
    {
        public string PaintBrushBrandName { get; set; }

    }
    public interface IHammer //锤子
    {
        string HammerBrandName { get; set; }
    }
    public class Hammer : HardwareItem, IHammer
    {
        public string HammerBrandName { get; set; }

    }

}
