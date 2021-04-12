using System;
using System.Collections.Generic;
using System.Text;

namespace DigiBarter.Models
{
    public class TradeRequestItem
    {
        public string request_id { get; set; }
        public string item_id { get; set; }
        public string item_name { get; set; }
        public string owner_id { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_photo_url { get; set; }
        public bool is_declined { get; set; }
        public bool is_approved { get; set; }
        public bool is_hidden { get; set; }
        public string request_time { get; set; }
        public string sortable_time { get; set; }

        public string notification_text
        {
            get  {
                 return user_name+ " wants to trade " + item_name;
            }
        }
    }
}
