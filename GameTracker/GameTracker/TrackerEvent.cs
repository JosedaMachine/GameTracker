using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTracker
{
    struct CommonContent
    {
        public string gameID { get; set; }
        public string sessionID { get; set; }
        public string userID { get; set; }
        public int time_stamp { get; set; }
    }


    internal class TrackerEvent
    {
        private CommonContent commonContent_;

        //For serialization purposes, we need a string event type.
        protected string eventType_;

        public TrackerEvent(CommonContent common)
        {
            commonContent_ = common;
            eventType_ = "NotDefined";
        }

        /// <summary>
        /// Method to format common attributes in CSV
        /// </summary>
        /// <returns>string CSV format</returns>
        public virtual string toCSV()
        {
            //string legend = "GameID,SessionID,UserID,TimeStamp,EventType, Params\n";
               
            string format = commonContent_.gameID + "," + commonContent_.sessionID + ","
                            + commonContent_.userID + "," + commonContent_.time_stamp + "," + eventType_;
            
            return format;
        }

        /// <summary>
        /// Method to format common attributes in CSV
        /// </summary>
        /// <returns>string CSV format</returns>
        public virtual string toJSON(){
            string format = "{ " +
                "\"GameID\": \"" + commonContent_.gameID + "\"," +
                "\"SessionID\": \"" + commonContent_.sessionID + "\"," +
                "\"UserID\": \"" + commonContent_.userID + "\"," +
                "\"TimeStamp\": \"" + commonContent_.time_stamp + "\"," +
                "\"EventType\": \"" + eventType_ + "\","; 
                //TODO : decir que falta cerra con esto:+"},";
            return format;
        }
    }
}