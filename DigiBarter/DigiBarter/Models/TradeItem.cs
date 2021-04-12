using System;
using System.Collections.Generic;
using System.Text;

namespace DigiBarter.Models
{
    public class TradeItem
    {
        public string item_id { get; set; }
        public string user_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string photo_url { get; set; }
        public string user_name { get; set; }
        public string user_photo_url { get; set; }
        public string posted_time { get; set; }
        public long sortable_time { get; set; }
    }
}
