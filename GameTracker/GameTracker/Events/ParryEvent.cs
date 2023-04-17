using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace GameTracker
{
    internal class ParryEvent : TrackerEvent
    {

        bool blocked, purplePowerUp;
        short level;

        public ParryEvent(CommonContent common) : base(common){
            eventType_ = "Parry";
        }

        public override string toCSV()
        {
            //Base information
            string format = base.toCSV();

            return format+","+ level+","+ blocked + ","+ purplePowerUp + "\n";
        }

        public override string toJSON()
        {
            //Base information
            string format = base.toJSON();

            //extract collection data
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(format);

            //Add data
            data["Level"] = level;
            data["Blocked"] = blocked;
            data["PurplePowerUp"] = purplePowerUp;

            // Serialize collection with new data
            string newCollection = JsonConvert.SerializeObject(data, new JsonSerializerSettings { Formatting = Formatting.Indented });

            //Close file
            newCollection += ",\n";

            return newCollection;
        }

        public void setLevel(short level_)
        {
            level = level_;
        }
        public void setPurplePowerUp(bool ppu_)
        {
            purplePowerUp = ppu_;
        }
        public void setBlocked(bool blocked_)
        {
            blocked = blocked_;
        }
    }
}
