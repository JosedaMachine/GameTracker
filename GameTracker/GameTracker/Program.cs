using System;
using System.Collections.Concurrent;
//using Test;

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
        // Iniciar hilo para leer números de la cola
        Thread readerThread = new Thread(ReadFromQueue);
        readerThread.Start();

        Console.WriteLine("Introduce números para sumar (0 para salir):");

        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int number))
            {
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

        // Esperar a que se vacíe la cola antes de finalizar
        while (!queue.IsEmpty)
        {
            Thread.Sleep(100);
        }

        // Indicar que el hilo debe finalizar
        stop = true;

        // Esperar a que el hilo termine
        readerThread.Join();

        Console.WriteLine($"La suma total es: {sum}");
        Console.WriteLine("Presiona cualquier tecla para salir.");
        Console.ReadKey();
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