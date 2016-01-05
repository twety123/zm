using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ServersidePush
{
    /// <summary>
    /// 客户端用于同步消息发送和接收信息
    /// </summary>
    public class Client
    {
        /// <summary>
        /// 通知一个或者多个正在等待的线程已发生事件
        /// </summary>
        private ManualResetEvent messagEvent = new ManualResetEvent(false);
        /// <summary>
        /// 消息队列模型
        /// </summary>
        private Queue<Message> messageQueue = new Queue<Message>();

        /// <summary>
        /// 专为发送方发送一条信息客户端
        /// </summary>
        /// <param name="message">信息</param>
        public void EnquereMessage(Message message)
        {
            //防止多个线程同时应用此代码
            lock (messageQueue)
            {
                messageQueue.Enqueue(message);
                messagEvent.Set();
            }
        }

        /// <summary>
        ///专为收件人接收信息
        /// </summary>
        /// <returns></returns>
        public Message DequereMessage()
        {
            messagEvent.WaitOne();
            lock (messageQueue)
            {
                if (messageQueue.Count == 1)
                {
                    messagEvent.Reset();
                }
                return messageQueue.Dequeue();
            }
        }
    }
}