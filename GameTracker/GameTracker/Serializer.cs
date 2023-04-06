using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTracker
{
    class Serializer
    {
        //TODO: cambiar a TrackerEvent
        virtual string serialize(Event e);
    }
}
