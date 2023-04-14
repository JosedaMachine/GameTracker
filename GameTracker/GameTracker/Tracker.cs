using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;

namespace GameTracker
{
    class TrackerSystem
    {
        private List<IPersistence> persistencesList = null;

        string gameID_, gameSession_, user_;
        
        private ConcurrentQueue<TrackerEvent> queue_;
        
        private Thread dequeueEvents_thread;
        
        private CommonContent commonContent_;
        
        private bool stop_;
        
        private DateTime currentTime_;

        private static TrackerSystem instance = null;
        public static TrackerSystem GetInstance() => instance;

        public TrackerSystem() {

        }

        public static bool Init(string gameID, string gameSession, string user, ref IPersistence persistence) {
            Debug.Assert(instance == null);

            instance = new TrackerSystem();

            if (!instance.initPrivate(gameID, gameSession, user, ref persistence)) {
                instance = null;
                return false;
            }

            return true;
        }

        private bool initPrivate(string gameID, string gameSession, string user, ref IPersistence persistence) {
            gameID_ = gameID;
            gameSession_ = gameSession;
            user_ = user;

            persistencesList = new List<IPersistence>();

            AddPersistence(ref persistence);

            currentTime_ = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)currentTime_).ToUnixTimeSeconds();
            Console.WriteLine(unixTime);

            commonContent_ = new CommonContent(gameID_, gameSession_, user_, 0);

            return true;
        }

        //release y relese private se hace un Dipose? TODO

        public void Start() {
            queue_ = new ConcurrentQueue<TrackerEvent>();

            dequeueEvents_thread = new Thread(SerializeEvents);
            dequeueEvents_thread.Start();
        }

        public void Stop(){
            stop_ = true;
            dequeueEvents_thread.Join();
        }

        public void Persist() {
            //Deberia ser en una hebra distinta el volcado???
            foreach (IPersistence persistence in persistencesList){
                persistence.flush();
            }
        }

        public void AddPersistence(ref IPersistence persistence){
            persistencesList.Add((IPersistence)persistence);
        }
        
        //Consumer.
        private void SerializeEvents()
        {
            while (queue_.Count > 0 || !stop_)
            {
                TrackerEvent e;
                if(queue_.TryDequeue(out e))
                {
                    foreach (IPersistence persistence in persistencesList){
                        persistence.send(e);
                    }
                }
            }
        }

        //Enqueues event
        public void trackEvent(TrackerEvent event_)
        {
            queue_.Enqueue(event_);
        }


        //// Creates ParryEvent
        //public ParryEvent CreateParryEvent()
        //{
        //    ParryEvent event_ = new ParryEvent(commonContent_);

        //    return event_;
        //}

        //public ParryInputAfterDeath CreateParryInputAfterDeathEvent()
        //{
        //    long unixTime = ((DateTimeOffset)currentTime_).ToUnixTimeSeconds();
            
        //    commonContent_.time_stamp = unixTime;

        //    ParryInputAfterDeath event_ = new ParryInputAfterDeath(commonContent_);

        //    return event_;
        //}
        
        //// Creates ObtainRedPowerUpEvent
        //public ObtainRedPowerUpEvent CreateObtainRedPowerUpEvent()
        //{
        //    long unixTime = ((DateTimeOffset)currentTime_).ToUnixTimeSeconds();

        //    commonContent_.time_stamp = unixTime;

        //    ObtainRedPowerUpEvent event_ = new ObtainRedPowerUpEvent(commonContent_);

        //    return event_;
        //}

        //// Creates initial session event
        //public InitSessionEvent CreateInitSessionEvent(){
            
        //    long unixTime = ((DateTimeOffset)currentTime_).ToUnixTimeSeconds();

        //    commonContent_.time_stamp = unixTime;

        //    InitSessionEvent event_ = new InitSessionEvent(commonContent_);

        //    return event_;
        //}

        //// Creates initial session event
        //public InitLevelEvent CreateInitLevelEvent()
        //{
        //    long unixTime = ((DateTimeOffset)currentTime_).ToUnixTimeSeconds();

        //    commonContent_.time_stamp = unixTime;

        //    InitLevelEvent event_ = new InitLevelEvent(commonContent_);

        //    return event_;
        //}

        //// Creates finish session event
        //public FinishSessionEvent CreateFinishSessionEvent()
        //{
        //    long unixTime = ((DateTimeOffset)currentTime_).ToUnixTimeSeconds();

        //    commonContent_.time_stamp = unixTime;

        //    FinishSessionEvent event_ = new FinishSessionEvent(commonContent_);

        //    return event_;
        //}

        //// Creates finish level event
        //public FinishLevelEvent CreateFinishLevelEvent()
        //{
        //    long unixTime = ((DateTimeOffset)currentTime_).ToUnixTimeSeconds();

        //    commonContent_.time_stamp = unixTime;

        //    FinishLevelEvent event_ = new FinishLevelEvent(commonContent_);

        //    return event_;
        //}

        public T CreateEvent<T>(params object[] parametros)
        {
            // Obt�n el tipo T
            Type tipo = typeof(T);

            // Verifica si el tipo T tiene un constructor que coincida con la cantidad de par�metros recibidos
            Type[] tiposParametros = new Type[parametros.Length + 1]; // Aumenta el tama�o del arreglo en 1 para incluir el primerPar�metro


            //Asignamos el nuevo tiempo
            long unixTime = ((DateTimeOffset)currentTime_).ToUnixTimeSeconds();
            commonContent_.time_stamp = unixTime;

            //A�adimos el parametro a la posicion inicial
            tiposParametros[0] = commonContent_.GetType(); // Agrega el tipo del primerPar�metro al inicio del arreglo
            
            for (int i = 0; i < parametros.Length; i++)
            {
                tiposParametros[i + 1] = parametros[i].GetType();
            }

            ConstructorInfo constructor = tipo.GetConstructor(tiposParametros);
            if (constructor == null)
            {
                throw new InvalidOperationException("No se encontr� un constructor adecuado para los par�metros proporcionados.");
            }

            // Crea una nueva instancia del objeto T con los par�metros proporcionados
            object[] parametrosCompletos = new object[parametros.Length + 1]; // Aumenta el tama�o del arreglo en 1 para incluir el primerPar�metro
            parametrosCompletos[0] = commonContent_; // Agrega el primerPar�metro al inicio del arreglo
            for (int i = 0; i < parametros.Length; i++)
            {
                parametrosCompletos[i + 1] = parametros[i];
            }

            T objeto = (T)constructor.Invoke(parametrosCompletos);
            return objeto;
        }

        // Creates die from bullet event
        public DieFromBulletEvent CreateDieFromBulletEvent()
        {
            long unixTime = ((DateTimeOffset)currentTime_).ToUnixTimeSeconds();

            commonContent_.time_stamp = unixTime;

            DieFromBulletEvent event_ = new DieFromBulletEvent(commonContent_);

            return event_;
        }
    }
}



//class CQ_EnqueueDequeuePeek
//{
//    // Demonstrates:
//    // ConcurrentQueue<T>.Enqueue()
//    // ConcurrentQueue<T>.TryPeek()
//    // ConcurrentQueue<T>.TryDequeue()
//    static void Main()
//    {
//        // Construct a ConcurrentQueue.
//        ConcurrentQueue<int> cq = new ConcurrentQueue<int>();

//        // Populate the queue.
//        for (int i = 0; i < 10000; i++)
//        {
//            cq.Enqueue(i);
//        }

//        // Peek at the first element.
//        int result;
//        if (!cq.TryPeek(out result))
//        {
//            Console.WriteLine("CQ: TryPeek failed when it should have succeeded");
//        }
//        else if (result != 0)
//        {
//            Console.WriteLine("CQ: Expected TryPeek result of 0, got {0}", result);
//        }

//        int outerSum = 0;
//        // An action to consume the ConcurrentQueue.
//        Action action = () =>
//        {
//            int localSum = 0;
//            int localValue;
//            while (cq.TryDequeue(out localValue)) localSum += localValue;
//            Interlocked.Add(ref outerSum, localSum);
//        };

//        // Start 4 concurrent consuming actions.
//        Parallel.Invoke(action, action, action, action);

//        Console.WriteLine("outerSum = {0}, should be 49995000", outerSum);
//    }
//}
