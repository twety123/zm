using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServersidePush
{
    public class Message
    {
        /// <summary>
        /// 收件人名称
        /// </summary>
        public string RecipientName { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent { get; set; }
    }
}