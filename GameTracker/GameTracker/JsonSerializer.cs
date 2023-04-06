using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTracker
{
    class JsonSerializer : Serializer
    {
        //TODO: cambiar a Tracker event
        virtual string serialize(Event e)
        {
            //TODO: pedirselo a la persitencia

            return e.toJson();
        }
    }
}
