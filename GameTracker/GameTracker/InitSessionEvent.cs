using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTracker
{
    internal class InitSessionEvent : TrackerEvent
    {
        public InitSessionEvent(CommonContent common) : base(common){
            eventType_ = "Init Session";
        }

        public override string toCSV()
        {
            //Inicio de CSV


            string format = base.toCSV();

            //formatear los datos

            return format;
        }

        public override string toJSON()
        {

            //Inicio de llaves


            string format = base.toJSON();

            //formatear los datos
            
            //cerrar contenedor
            format += "},";
            return format;
        }

    }
}
