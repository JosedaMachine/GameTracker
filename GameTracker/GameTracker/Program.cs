﻿using System;

using Test;

class Program
{
    static void Main(string[] args)
    {
        ConsoleKeyInfo keyInfo;
        bool stop = false;


        // Ruta del archivo de texto
        string filePath = @"U:\hlocal\UAj\events.txt";
        string pruebitaSergioPrueba = @"U:\hlocal\UAj\events.txt";
        StreamWriter writer = new StreamWriter(filePath);
        // Escribir texto en archivo de texto


        TrackerSystem.Init("NeonRider", "1", "Joseda");

        TrackerSystem tracker = TrackerSystem.GetInstance();
        tracker.Start();


        while (!stop){
            keyInfo = Console.ReadKey(true);


            string n = "Tecla presionada: " + keyInfo.KeyChar + "\n";
            writer.Write(n);

            tracker.addEvent(n);



            if (keyInfo.Key == ConsoleKey.X){
                writer.Write    ("Deteniendo el bucle...\n");
                stop = true;
            }
        }

        writer.Close();
    }
}