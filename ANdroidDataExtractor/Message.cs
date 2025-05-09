using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AndroidDataExtractor.Models
{
    public partial class Message
    {
        public string Sender { get; set; }
        public string MessageContent { get; set; }
        public string Timestamp { get; set; }
    }
}

