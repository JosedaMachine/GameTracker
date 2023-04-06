using System;

namespace GameTracker
{
    internal class ServerPersistence : IPersistence
    {
        public void flush()
        {
            throw new NotImplementedException();
        }

        public void send(TrackerEvent e)
        {
            throw new NotImplementedException();
        }
    }
}
