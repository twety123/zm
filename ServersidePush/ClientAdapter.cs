using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServersidePush
{
    /// <summary>
    /// 用来管理多个客户端实例
    /// </summary>
    public class ClientAdapter
    {
        /// <summary>
        /// 收件人列表
        /// </summary>
        private Dictionary<string, Client> recipients = new Dictionary<string, Client>();


        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(Message message)
        {
            if (recipients.ContainsKey(message.RecipientName))
            {
                Client client = recipients[message.RecipientName];
                client.EnquereMessage(message);
            }
        }

        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="userName"></param>
        public string GetMessage(string userName)
        {
            string messageContent = string.Empty;
            if (recipients.ContainsKey(userName))
            {
                Client client = recipients[userName];
                messageContent = client.DequereMessage().MessageContent;
            }
            return messageContent;
        }
        /// <summary>
        /// 加入一个用户到收件人列表
        /// </summary>
        /// <param name="userName"></param>
        public void Join(string userName)
        {
            recipients[userName] = new Client();
        }

        /// <summary>
        /// 单例模式，确保只有个这个类的实例系统
        /// </summary>
        public static ClientAdapter Instance=new ClientAdapter();\

        private ClientAdapter()
        {
            
        }
    }
}