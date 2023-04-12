using System;
using System.IO;

namespace GameTracker
{
    internal class FilePersistence : IPersistence
    {
        ISerializer serializer;
        const string path = "";

        string fileData;

        public FilePersistence()
        {
            fileData = "";

            serializer = new JsonSerializer();
        }

        public void send(TrackerEvent e)
        {
           fileData += serializer.serialize(e);
        }

        public void flush()
        {
            try
            {
                // Abrir el archivo en modo append
                StreamWriter outputFile = new StreamWriter(path + serializer.getName(), true);

                //Escribir contenido
                outputFile.WriteLine(fileData);

                Console.WriteLine("Fichero escrito");

                outputFile.Close();

                //Reestablecer cadena que guarda los eventos
                fileData = "";
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
