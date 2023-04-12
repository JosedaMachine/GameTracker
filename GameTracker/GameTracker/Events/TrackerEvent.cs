using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace GameTracker
{
    struct CommonContent
    {
        public string gameID { get; set; }
        public string sessionID { get; set; }
        public string userID { get; set; }
        public long time_stamp { get; set; }

        public CommonContent(string gID, string sID,string uID, long time)
        {
            gameID = gID;
            sessionID = sID;
            userID = uID;
            time_stamp = time;
        }
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

            var datos = new
            {
                GameID = commonContent_.gameID,
                SessionID = commonContent_.sessionID,
                UserID = commonContent_.userID,
                TimeStamp = commonContent_.time_stamp,
                EventType = eventType_
            };

            // Serialize collection with new data
            string jsonString = JsonConvert.SerializeObject(datos, new JsonSerializerSettings { Formatting = Formatting.Indented });

            //string format = "{ " +
            //    "\"GameID\": \"" + commonContent_.gameID + "\"," +
            //    "\"SessionID\": \"" + commonContent_.sessionID + "\"," +
            //    "\"UserID\": \"" + commonContent_.userID + "\"," +
            //    "\"TimeStamp\": \"" + commonContent_.time_stamp + "\"," +
            //    "\"EventType\": \"" + eventType_ + "\","; 

            return jsonString;
        }
    }
}