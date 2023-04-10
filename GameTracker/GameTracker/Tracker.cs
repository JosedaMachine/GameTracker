using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
            stopwatch = new Stopwatch();
            stopwatch.Start();

            return true;
        }

        private CommonContent setCommonContent()
        {
            CommonContent common = new CommonContent();

            common.sessionID = gameSession_;
            common.gameID = gameID_;
            common.time_stamp = stopwatch.ElapsedMilliseconds;
            common.userID = user_;

            return common;
        }

        public void Start() {
            queue_ = new ConcurrentQueue<TrackerEvent>();

            TrackerEvent start = new TrackerEvent(setCommonContent());

            queue_.Enqueue(start);

            //Consumir hasta que no haya nada mas??¿?¿?¿
            Parallel.Invoke(Process);
        }

        //Consumidor
        private void Process() {
            TrackerEvent result;
            if (!queue_.TryPeek(out result))
            {
                Console.WriteLine("CQ: TryPeek failed when it should have succeeded");
            }
            //else if (result != )
            //{
            //    Console.WriteLine("CQ: Expected TryPeek result of 0, got {0}", result);
            //}
        }
        
        private void PersistEvents()
        {
            while (queue_.Count >0)
            {
                TrackerEvent e;
                if(queue_.TryDequeue(out e))
                {
                    persistance.send(e);
                }
            }
            persistance.flush();
        }

        public void trackEvent(TrackerEvent event_)
        {
            queue_.Enqueue(event_);
        }



        public ParryEvent CreateParryEvent()
        {
            ParryEvent event_ = new ParryEvent(setCommonContent());

            return event_;
        }

        string gameID_, gameSession_, user_;
        ConcurrentQueue<TrackerEvent> queue_;
        private IPersistence persistance;
        Stopwatch stopwatch;
    }
}

//class ThreadTest
//{
//    static bool done;

//    //static void Main()
//    //{
//    //    Thread t = new Thread(Go);          // Kick off a new thread
//    //    t.Start();                               // running WriteY()


//    //    Go();
//    //}

//    static void Go()
//    {
//        if (!done) { Console.WriteLine("Done"); done = true; }
//    }
//}



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
