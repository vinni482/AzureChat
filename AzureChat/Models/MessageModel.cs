using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureChat.Models
{
    public class MessageModel
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}