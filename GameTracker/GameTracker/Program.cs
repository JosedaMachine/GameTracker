using System;
using System.Collections.Concurrent;
using System.Threading;
using System.IO;
using GameTracker;

class Program
{
    static void sain(string[] args)
    {
        ConsoleKeyInfo keyInfo;
        bool stop = false;


        // Ruta del archivo de texto
        string filePath = @"U:\hlocal\UAj\events.txt";
        StreamWriter writer = new StreamWriter(filePath);
        // Escribir texto en archivo de texto


        //TrackerSystem.Init("NeonRider", "1", "Joseda");

        //TrackerSystem tracker = TrackerSystem.GetInstance();
        //tracker.Start();


        while (!stop){
            keyInfo = Console.ReadKey(true);


            string n = "Tecla presionada: " + keyInfo.KeyChar + "\n";
            writer.Write(n);

            //tracker.addEvent(n);



            if (keyInfo.Key == ConsoleKey.X){
                writer.Write    ("Deteniendo el bucle...\n");
                stop = true;
            }
        }

        writer.Close();
    }

    static ConcurrentQueue<int> queue = new ConcurrentQueue<int>(); // Cola concurrente para almacenar números
    static int sum = 0; // Variable para almacenar la suma de los números
    static bool stop = false; // Variable para indicar que el hilo debe finalizar

    static void Main()
    {
        //El FilePersistence deberia poder tener multiples serializadores??? No, no? no se ah

        //Parece que el timestamp es el mismo...

        //No deberia sobreescribir en el fichero

        //Strategy
        ISerializer serializerCSV = new CSVSerializer();
        IPersistence filePersistence = new FilePersistence(ref serializerCSV);

        TrackerSystem.Init("Game", "2", "Player", ref filePersistence);

        TrackerSystem tracker = TrackerSystem.GetInstance();

        ISerializer serializerJSON = new JsonSerializer();
        IPersistence filePersistenceCopy = new FilePersistence(ref serializerJSON);

        tracker.AddPersistence(ref filePersistenceCopy);

        tracker.Start();

        Console.WriteLine("Introduce números para sumar (0 para salir):");

        //Inicio de sesion

        InitSessionEvent initE = tracker.CreateEvent<InitSessionEvent>();
        tracker.trackEvent(initE);

        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int number))
            {
                TrackerEvent? e = null;

                switch (number)
                {
                    case 1:
                        //Inicio de partida
                        e = tracker.CreateEvent<InitLevelEvent>();
                        break;
                    case 2:
                        //Lanzar parry

                        e = tracker.CreateEvent<ParryEvent>();
                        break;
                    case 3:
                        //Obtener powerUP rojo
                        e = tracker.CreateEvent<ObtainRedPowerUpEvent>();
                        break;
                    case 4:
                        e = tracker.CreateEvent<FinishLevelEvent>();
                        //Final de partida
                        break;
                    default:
                        break;
                }

                if (e != null)
                    tracker.trackEvent(e);

                if (number == 0)
                    break;

                // Agregar número a la cola
                queue.Enqueue(number);
            }
            else
            {
                Console.WriteLine("¡Por favor, introduce un número válido!");
            }
        }

        //Final de sesion
        FinishSessionEvent finishE = tracker.CreateEvent<FinishSessionEvent>();
        tracker.trackEvent(finishE);

        //Parar
        tracker.Stop();

        //Volcar el fichero en disco
        tracker.Persist();

        //// Esperar a que se vacíe la cola antes de finalizar
        //while (!queue.IsEmpty)
        //{
        //    Thread.Sleep(100);
        //}
    }

    static void ReadFromQueue()
    {
        while (!stop)
        {
            if (queue.TryDequeue(out int number))
            {
                // Sumar número a la suma total
                sum += number;
            }
            else
            {
                // Esperar un momento si la cola está vacía
                Thread.Sleep(100);
            }
        }
    }
}