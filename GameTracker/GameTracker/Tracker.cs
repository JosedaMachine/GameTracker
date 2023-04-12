using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameTracker
{
    class TrackerSystem
    {
        private static TrackerSystem instance = null;
        public static TrackerSystem GetInstance() => instance;

        public TrackerSystem() {

        }



        public static bool Init(string gameID, string gameSession, string user) {
            Debug.Assert(instance == null);

            instance = new TrackerSystem();

            if (!instance.initPrivate(gameID, gameSession, user)) {
                instance = null;
                return false;
            }

            return true;
        }

        private bool initPrivate(string gameID, string gameSession, string user) {
            gameID_ = gameID;
            gameSession_ = gameSession;
            user_ = user;

            //TODO: Esto es una mierda hay que sacarlo de aqui.
            persistence = new FilePersistence();

            stopwatch = new Stopwatch();
            stopwatch.Start();

            commonContent_ = new CommonContent(gameID_, gameSession_, user_, stopwatch.ElapsedMilliseconds);

            return true;
        }


        public void Start() {
            queue_ = new ConcurrentQueue<TrackerEvent>();

            TrackerEvent start = new TrackerEvent(commonContent_);

            queue_.Enqueue(start);

            //TODO: Consumir hasta que no haya nada mas??¿?¿?¿
            //Parallel.Invoke(Process);

            dequeueEvents_thread = new Thread(SerializeEvents);
            dequeueEvents_thread.Start();
        }

        public void Stop()
        {
            stop = true;
            dequeueEvents_thread.Join();
            
        }

        public void Persist() {
            //Deberia ser en una hebra distinta el volcado???


            //TrackerEvent result;
            //if (!queue_.TryPeek(out result))
            //{
            //    Console.WriteLine("CQ: TryPeek failed when it should have succeeded");
            //}
            persistence.flush();
        }
        
        //Consumer.
        private void SerializeEvents()
        {
            while (queue_.Count > 0 || !stop)
            {
                TrackerEvent e;
                if(queue_.TryDequeue(out e))
                {
                    persistence.send(e);
                }
            }
        }

        //Enqueues event
        public void trackEvent(TrackerEvent event_)
        {
            queue_.Enqueue(event_);
        }


        // Creates ParryEvent
        public ParryEvent CreateParryEvent()
        {
            ParryEvent event_ = new ParryEvent(commonContent_);

            return event_;
        }

        public ParryInputAfterDeath CreateParryInputAfterDeathEvent()
        {
            ParryInputAfterDeath event_ = new ParryInputAfterDeath(commonContent_);

            return event_;
        }
        
        // Creates ObtainRedPowerUpEvent
        public ObtainRedPowerUpEvent CreateObtainRedPowerUpEvent()
        {
            ObtainRedPowerUpEvent event_ = new ObtainRedPowerUpEvent(commonContent_);

            return event_;
        }

        // Creates initial session event
        public InitSessionEvent CreateInitSessionEvent(){
            InitSessionEvent event_ = new InitSessionEvent(commonContent_);

            return event_;
        }

        // Creates initial session event
        public InitLevelEvent CreateInitLevelEvent()
        {
            InitLevelEvent event_ = new InitLevelEvent(commonContent_);

            return event_;
        }

        // Creates finish session event
        public FinishSessionEvent CreateFinishSessionEvent()
        {
            FinishSessionEvent event_ = new FinishSessionEvent(commonContent_);

            return event_;
        }

        // Creates finish level event
        public FinishLevelEvent CreateFinishLevelEvent()
        {
            FinishLevelEvent event_ = new FinishLevelEvent(commonContent_);

            return event_;
        }

        string gameID_, gameSession_, user_;
        ConcurrentQueue<TrackerEvent> queue_;
        private IPersistence persistence;
        Stopwatch stopwatch;
        Thread dequeueEvents_thread;
        CommonContent commonContent_;
        private bool stop;
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
