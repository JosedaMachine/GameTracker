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
        string pruebitaSergioPrueba = @"U:\hlocal\UAj\events.txt";
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


        //CommonContent commonContent = new CommonContent();
        //commonContent.sessionID = "12";
        //commonContent.gameID = "Apex";
        //commonContent.time_stamp = 1123132132;
        //commonContent.userID = "Joseda";

        //InitSessionEvent initS_E = new InitSessionEvent(commonContent);
        //InitlLevelEvent initL_E = new InitlLevelEvent(commonContent);
        //FinishlLevelEvent finishL_E = new FinishlLevelEvent(commonContent);
        //FinishSessionEvent finishS_E = new FinishSessionEvent(commonContent);

        //string json = initS_E.toJSON();
        //json = json + initL_E.toJSON();
        //json = json + finishL_E.toJSON();
        //json = json + finishS_E.toJSON();

        //string csv = initS_E.toCSV();
        //csv = csv + initL_E.toCSV();
        //csv = csv + finishL_E.toCSV();
        //csv = csv + finishS_E.toCSV();


        //Console.WriteLine(json);
        //Console.WriteLine(csv);

        TrackerSystem.Init("NeonRider", "1", "Joseda");

        TrackerSystem tracker = TrackerSystem.GetInstance();
        tracker.Start();

        IPersistence persistence = new FilePersistence();


        Console.WriteLine("Introduce números para sumar (0 para salir):");

        //Inicio de sesion



        InitSessionEvent initE = tracker.CreateInitSessionEvent();
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
                        e = tracker.CreateInitLevelEvent();
                        break;
                    case 2:
                        //Lanzar parry
                        e = tracker.CreateParryEvent();
                        break;
                    case 3:
                        //Obtener powerUP rojo
                        e = tracker.CreateObtainRedPowerUpEvent();
                        break;
                    case 4:
                        e = tracker.CreateFinishLevelEvent();
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
        FinishSessionEvent finishE = tracker.CreateFinishSessionEvent();
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