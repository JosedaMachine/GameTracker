using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTracker
{
    class JsonSerializer : ISerializer
    {
        private string name_ = "data.json";
        string ISerializer.serialize(TrackerEvent e)
        {
            return e.toJSON();
        }

        public void setName(string name){
            name_ = name;
        }


        string ISerializer.getName() { return name_; }
    }
}
