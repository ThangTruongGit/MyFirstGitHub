using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncProgramming
{

    public delegate int BinaryOP(int x, int y);

    class AddParams
    {
        public int a, b;

        public AddParams(int numb1, int numb2)
        {
            a = numb1;
            b = numb2;
        }
    }

    class Program
    {
        private static bool isDone = false;

        private static AutoResetEvent waitHandle = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            //Console.WriteLine("**** Synch Delegate Review ****");

            ////print out the ID if the excuitng thread

            //Console.WriteLine("Main() invoke on thread {0}", Thread.CurrentThread.ManagedThreadId);

            ////Invoke Add() in a synchronous manner
            ////BinaryOP b = new BinaryOP(Add);

            //////could also write b.Invoke (10,10)

            ////int answer = b(10, 10);


            //////these lines will not execute until the Add() method has completed
            ////Console.WriteLine("Doing more work in Main()");

            ////Console.WriteLine("10 + 10 is {0}.", answer);

            ////Invoke Add() on a secondary thread
            //BinaryOP b = new BinaryOP(Add);
            //IAsyncResult ar = b.BeginInvoke(10, 10, null, null);

            ////do other work on primary thread
            //Console.WriteLine("Doing more work in Main()");


            ////this message will keep printing until the Add() method is finished

            //while(!ar.IsCompleted)
            //{
            //    Console.WriteLine("doing more work in Main() while waiting");
            //    Thread.Sleep(1000);
            //}

            ////obtain the result of the Add() method when ready

            //int answer = b.EndInvoke(ar);

            //Console.WriteLine("10 + 10 is {0}.", answer);


            //Console.WriteLine("to see if execute before the answer");


            //Console.ReadLine();


            //************* second example

            //Console.WriteLine("**** AsyncCallbackDelegate Example *****");
            //Console.WriteLine("Main() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);

            //BinaryOP b = new BinaryOP(Add);

            //IAsyncResult ar = b.BeginInvoke(10, 10, new AsyncCallback(AddComplete), null);

            //while (!isDone)
            //{
            //    Console.WriteLine("Main working...");
            //    Thread.Sleep(1000);
            //}

            //*************** third example

            Console.WriteLine("***** Adding with Thread objects ****");
            Console.WriteLine("ID of thread in Main(): {0}", Thread.CurrentThread.ManagedThreadId);
            AddParams ap = new AddParams(10, 10);
            Thread t = new Thread(new ParameterizedThreadStart(Add));
            t.Start(ap);

            //Wait here until you are notified
            waitHandle.WaitOne();

            Console.WriteLine("Ohter thread is done!");


         
            Console.ReadLine();
        }


        static int Add(int x, int y)
        {
            //print out the id of the executing thread
            Console.WriteLine("Add() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);


            //pause to simulate a lengthy operation
            Thread.Sleep(5000);
            return x + y;
        }

        static void AddComplete(IAsyncResult iar)
        {
            Console.WriteLine("AddComplete() invoked on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Your addition is complete");

            //now get the result
            AsyncResult ar = (AsyncResult)iar;

            BinaryOP b = (BinaryOP)ar.AsyncDelegate;
            Console.WriteLine("10 + 10 = {0}.", b.EndInvoke(iar));



            isDone = true;
        }

        static void Add(object data)
        {
            if(data is AddParams)
            {
                Console.WriteLine("ID of thread in Add(): {0}", Thread.CurrentThread.ManagedThreadId);

                AddParams ap = (AddParams)data;
                Console.WriteLine("{0} + {1} is {2}", ap.a, ap.b, ap.a + ap.b);

                //Tell other thread we are done
                Thread.Sleep(5000);
                waitHandle.Set();
            }
        }
    }
}
