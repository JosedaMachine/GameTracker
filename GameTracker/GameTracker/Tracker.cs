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

        private uint frecuencyPersistanceTimeSec_ = 0;

        private static TrackerSystem instance = null;
        public static TrackerSystem GetInstance() => instance;

        private Dictionary<string, bool> eventsTracking = new Dictionary<string, bool>();

        /// <summary>
        /// Sets frecuency time to persist event data. If time is 0, it 
        /// will persist only when Tracker stops, or when Persist() is called.
        /// </summary>
        /// <param name="time_"></param>
        public void setFrecuencyPersistanceTimeSeconds(uint time_){
            frecuencyPersistanceTimeSec_ = time_;
        }

        public TrackerSystem() {

        }

        /// <summary>
        /// Initializes Tracker with given parameters and a initial persistance method.
        /// </summary>
        /// <param name="gameID"></param>
        /// <param name="gameSession"></param>
        /// <param name="user"></param>
        /// <param name="persistence"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Starts new thread to process and track events.
        /// </summary>
        /// <returns>True if starting succeed. Otherwise, false since it was called twice.</returns>
        public bool Start() {

            if(queue_ != null)
                return false;

            queue_ = new ConcurrentQueue<TrackerEvent>();

            dequeueEvents_thread = new Thread(SerializeEvents);
            dequeueEvents_thread.Start();

            InitSessionEvent ISE = CreateEvent<InitSessionEvent>();
            
            if(ISE != null)
                trackEvent(ISE);

            return true;
        }

        /// <summary>
        /// Stops the thread and persists remaining events.
        /// </summary>
        /// <returns>True if stoping succeed. Otherwise, false since starts wasn't called</returns>
        public bool Stop(){

            if (queue_ == null)
                return false;


            FinishSessionEvent FSE = CreateEvent<FinishSessionEvent>();
            
            if(FSE != null)
                trackEvent(FSE);

            stop_ = true;
            SerializeEvents();
            Persist();

            dequeueEvents_thread.Join();

            queue_ = null;

            return true;
        }

        /// <summary>
        /// Sets an event to be tracked or not.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isTracked"></param>
        /// <returns>True if T is subclass of TrackerEvent. Otherwise, false.</returns>
        public bool setEventToBeTracked<T>(bool isTracked){
            if (!typeof(T).IsSubclassOf(typeof(TrackerEvent)))
                return false;

            Type eventClass = typeof(T);

            string eventType = eventClass.Name;

            if (eventsTracking.ContainsKey(eventType)){
                eventsTracking[eventType] = isTracked;
            }  
            else
                eventsTracking.Add(eventType, isTracked);

            return true;
        }

        /// <summary>
        /// Persists enqueued events.
        /// </summary>
        public void Persist() {
            foreach (IPersistence persistence in persistencesList){
                persistence.flush();
            }
        }

        /// <summary>
        /// Adds a new persistance method that coexist simultaneously with others. 
        /// </summary>
        /// <param name="persistence"></param>
        public void AddPersistence(ref IPersistence persistence){
            persistencesList.Add(persistence);
        }
        
        //Consumer.
        private void SerializeEvents()
        {
            long lastTime = getCurrTime();

            while (queue_.Count > 0 || !stop_)
            {
                long currTime = getCurrTime();
                if (frecuencyPersistanceTimeSec_ > 0 && currTime - lastTime > frecuencyPersistanceTimeSec_){
                    Persist();

                    lastTime = currTime;
                }

                TrackerEvent e;
                //Usamos Reflection para obtener la llamada al metodo eventType estatico del evento en cuestion.
                if(queue_.TryDequeue(out e)){
                    Type eventClass = e.GetType();

                    string eventType = eventClass.Name;

                    bool isTracked;
                        
                    //Si se ha definido que sea rastreado, se inspecciona su valor
                    if (eventsTracking.TryGetValue(eventType, out isTracked))
                    {
                        if (isTracked)
                        {
                            foreach (IPersistence persistence in persistencesList)
                                persistence.send(e);
                        }
                    }
                    else //Si no esta definido, se rastrea de todas formas.
                    {
                        foreach (IPersistence persistence in persistencesList)
                            persistence.send(e);
                    }
                }
            }
        }

        /// <summary>
        /// Enqueues event. Event must not be null.
        /// </summary>
        /// <param name="event_"></param>
        public void trackEvent(TrackerEvent event_)
        {
            queue_.Enqueue(event_);
        }


        /// <summary>
        /// Creates an custom event that must derivate from TrackerEvent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parametros"> event custom parameters</param>
        /// <returns>instance of the event</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public T? CreateEvent<T>(params object[] parametros)
        {
            if (!typeof(T).IsSubclassOf(typeof(TrackerEvent)))
                return default(T);


            // Obtener el tipo T
            Type tipo = typeof(T);

            // Verificamos si el tipo T tiene un constructor que coincida con la cantidad de parámetros recibidos
            Type[] tiposParametros = new Type[parametros.Length + 1]; // Aumenta el tamaño del arreglo en 1 para incluir el primerParámetro


            //Asignamos el nuevo tiempo
            currentTime_ = DateTime.UtcNow;
            commonContent_.time_stamp = getCurrTime();

            //Añadimos el parametro a la posicion inicial
            tiposParametros[0] = commonContent_.GetType(); // Agrega el tipo del primerParámetro al inicio del arreglo
            
            for (int i = 0; i < parametros.Length; i++)
            {
                tiposParametros[i + 1] = parametros[i].GetType();
            }

            ConstructorInfo constructor = tipo.GetConstructor(tiposParametros);
            if (constructor == null)
            {
                throw new InvalidOperationException("No se encontró un constructor adecuado para los parámetros proporcionados.");
            }

            // Crea una nueva instancia del objeto T con los parámetros proporcionados
            object[] parametrosCompletos = new object[parametros.Length + 1]; // Aumenta el tamaño del arreglo en 1 para incluir el primerParámetro
            parametrosCompletos[0] = commonContent_; // Agrega el primerParámetro al inicio del arreglo
            for (int i = 0; i < parametros.Length; i++)
            {
                parametrosCompletos[i + 1] = parametros[i];
            }

            T objeto = (T)constructor.Invoke(parametrosCompletos);
            return objeto;
        }

        private long getCurrTime(){
            currentTime_ = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)currentTime_).ToUnixTimeSeconds();

            return unixTime;
        }
    }
}
