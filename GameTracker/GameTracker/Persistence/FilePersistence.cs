using System;

namespace GameTracker
{
    internal class FilePersistence : IPersistence
    {
        ISerializer serializer;

        public void send(Event e)
        {

            //string serializer.ser
        }

        public void flush()
        {
            throw new NotImplementedException();
        }
    }
}
