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
    public partial class CallLog
    {
        public string PhoneNumber { get; set; }
        public string CallType { get; set; } // Incoming / Outgoing / Missed
        public string Duration { get; set; }
    }
}
