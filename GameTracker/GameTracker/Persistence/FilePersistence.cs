using System;
using System.IO;

namespace GameTracker
{
    internal class FilePersistence : IPersistence
    {
        ISerializer serializer;
        const string path = "../Data/";

        string fileData;

        FilePersistence()
        {
            fileData = "";
        }

        public void send(TrackerEvent e)
        {
            serializer = new JsonSerializer();

            fileData += serializer.serialize(e);
        }

        public void flush()
        {
            // Abrir el archivo en modo append
            StreamWriter outputFile = new StreamWriter(path+"data", true);

            //Escribir contenido
            outputFile.WriteLine(fileData);

            Console.WriteLine("Fichero escrito");

            outputFile.Close();

            //Reestablecer cadena que guarda los eventos
            fileData = "";
        }
    }
}
