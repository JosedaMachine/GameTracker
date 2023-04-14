using System;

namespace GameTracker
{
    internal class ServerPersistence : IPersistence
    {
        private ISerializer serializer_;
        private string fileData_;

        public ServerPersistence(ref ISerializer serializer)
        {
            fileData_ = "";

            serializer_ = serializer;
        }
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
