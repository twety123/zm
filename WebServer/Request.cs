using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    class Request
    {
        private string _rawUrl;
        private string _method;

        public string RawUrl { get; set; }
        public string Method { get; set; }

        //构造函数
        public Request(string content)
        {
            string[] lines = content.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            this.Method = lines[0].Split(' ')[0];
            this.RawUrl = lines[0].Split(' ')[1];
        }
    }
}
