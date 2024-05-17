using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogging.view.NotficationView
{
    public class NotificationResponseViewModel
    {
        public string Date { get; set; }
        public bool IsSeen { get; set; }
        public string Message { get; set; }
    }
}
