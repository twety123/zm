using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    class Response
    {


        public int CodeStatus { get; set; }
        public int ContentLenth { get; set; }
        public string ContentType { get; set; }
        public byte[] Buffer { get; set; }

        public Response(int codeStatus, byte[] buffer, string ext)
        {
            FillCodeStaDic();
            this.Buffer = buffer;
            this.CodeStatus = codeStatus;
            this.ContentLenth = buffer.Length;

        }

        Dictionary<int, string> codeStatusDic = new Dictionary<int, string>();
        private void FillCodeStaDic()
        {
            codeStatusDic[200] = "OK";
            codeStatusDic[404] = "请求的页面不存在";
        }


        /// <summary>
        /// 根据请求文件的后缀名，确定响应体的类型
        /// </summary>
        /// <param name="ext"></param>
        void GetContentType(string ext)
        {
            switch (ext)
            {
                case ".css":
                    this.ContentType = "text/css";
                    break;
                case ".gif":
                    this.ContentType = "image/gif";
                    break;
                case ".ico":
                    this.ContentType = "image/x-icon";
                    break;
                case ".jpe":
                case ".jpeg":
                case ".jpg":
                    this.ContentType = "image/jpeg";
                    break;
                case "bmp":
                    this.ContentType = "image/bmp";
                    break;
                case ".js":
                    this.ContentType = "application/x-javascript";
                    break;
                case "stm":
                case ".htm":
                case ".html":
                    this.ContentType = "text/html";
                    break;
            }
        }

        /// <summary>
        /// 拼接响应报文
        /// </summary>
        /// <returns></returns>
        public byte[] GetResponse()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("HTTP/1.0" + this.CodeStatus + "" + codeStatusDic[this.CodeStatus] + "\r\n");
            sb.Append("Content-Type:" + this.ContentType + "\r\n");
            sb.Append("Content-Length:" + this.ContentLenth + "\r\n");
            sb.Append("Server:ghhSever/1.0\r\n");
            sb.Append("X-Powered-By:MannyGou\r\n");
            sb.Append("\r\n");
            //创建响应报文头
            byte[] header = Encoding.UTF8.GetBytes(sb.ToString());
            //创建响应报文体
            byte[] content = this.Buffer;
            List<byte> bliList = new List<byte>();
            bliList.AddRange(header);
            bliList.AddRange(content);
            return bliList.ToArray();
        }
    }
}
