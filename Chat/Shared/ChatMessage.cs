using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Shared
{
    public class ChatMessage
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime Date { get; set; }

        public int SerialNo { get; set; }
    }
}
