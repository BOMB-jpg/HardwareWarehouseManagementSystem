using System;
using System.Collections.Generic;
using System.Text;

namespace WarehouseManagementSystemAPI
{
    public delegate void QueueEventHandler<T, U>(T sender, U eventArgs);  // 泛型委托
    public class CustomQueue<T> where T:IEntityPrimaryProperties, IEntityAdditionalProperties
    {
        Queue<T> _queue = null;  //初始化一个队列字段

        public event QueueEventHandler<CustomQueue<T>, QueueEventArgs> CustomQueueEvent;
        //T是事件的发送者类型  U是事件参数的类型  
        //因此，这个事件可以被用来订阅并处理队列中的各种事件，比如元素添加、移除等等。

        public CustomQueue() //构造函数
        {
            _queue = new Queue<T>();  //T是泛型  
        }

        public int QueueLength  //队列的长度
        {
            get {
                return _queue.Count;
            }
        }
        public void AddItem(T item)  //添加
        {
            _queue.Enqueue(item);

            QueueEventArgs queueEventArgs = new QueueEventArgs { Message = $"DateTime: {DateTime.Now.ToString(Constants.DateTimeFormat)}, Id ({item.Id}), Name ({item.Name}), Type ({item.Type}), Quantity ({item.Quantity}), has been added to the queue." };

            OnQueueChanged(queueEventArgs); //事件订阅进行通知


        }
        public T GetItem()  //删除
        {
            T item = _queue.Dequeue();  //队列的出队
            //将队列出队的元素放到queueEventArargs   
            QueueEventArgs queueEventArgs = new QueueEventArgs { Message = $"DateTime: {DateTime.Now.ToString(Constants.DateTimeFormat)}, Id ({item.Id}), Name ({item.Name}), Type ({item.Type}), Quantity ({item.Quantity}), has been processed." };

            OnQueueChanged(queueEventArgs); // //事件订阅进行通知

            return item;

        }
        protected virtual void OnQueueChanged(QueueEventArgs a)
        {
            CustomQueueEvent(this, a); //通知订阅
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator(); //GetEnumerator() 方法返回的迭代器允许你在队列上进行迭代操作，就像使用 foreach 循环一样。通过调用 GetEnumerator() 方法，你可以逐个访问队列中的元素，执行相关的操作。GetEnumerator() 方法返回的迭代器允许你在队列上进行迭代操作，就像使用 foreach 循环一样。通过调用 GetEnumerator() 方法，你可以逐个访问队列中的元素，执行相关的操作。
        }
    }

    public class QueueEventArgs:System.EventArgs
    { 
        public string Message { get; set; }
     //添加或移除元素时，会创建一个包含相关信息的 QueueEventArgs 实例，并将其传递给事件处理程序。这样，事件处理程序就可以根据事件的发生情况来执行相应的操
    }
    
}
