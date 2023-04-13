using System;
using System.IO;

namespace GameTracker
{
    internal class FilePersistence : IPersistence
    {
        private ISerializer serializer_;
        private string path_ = "";

        private string fileData_;

        public FilePersistence(ref ISerializer serializer)
        {
            fileData_ = "";

            serializer_ = serializer;
        }

        public void setOutPutPath(string path) => path_ = path;

        public void send(TrackerEvent e)
        {
           fileData_ += serializer_.serialize(e);
        }

        public void flush()
        {
            try
            {
                // Abrir el archivo en modo append
                StreamWriter outputFile = new StreamWriter(path_ + serializer_.getName(), true);

                //Escribir contenido
                outputFile.WriteLine(fileData_);

                Console.WriteLine("Fichero escrito");

                outputFile.Close();

                //Reestablecer cadena que guarda los eventos
                fileData_ = "";
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
