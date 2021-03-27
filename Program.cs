/*202110 CS-2860-001
FixMatrix-part1
FixMatrix-part1
Due Monday by 8:45am Points 10 Submitting a text entry box
In the textbook we use POSIX library to send semaphores.  Here is a quick cross-reference table I've prepared for you to illustrate the differences with C# and semaphores.  You can read more at: https://docs.microsoft.com/en-us/dotnet/api/system.threading.autoresetevent?view=netcore-3.1 (Links to an external site.) 

POSIX Command	C# Equivalent	Purpose */

 

using System;
using System.Threading;
namespace semaphore

{
    class Program
    {
        private static char[] myLine = new char[80];
        private static AutoResetEvent[] toKids = new AutoResetEvent[80];
        private static AutoResetEvent[] toMain = new AutoResetEvent[80];
        private static Thread[] tList = new Thread[80];
        private static volatile bool kidsCanRun=true;
        static void Main(string[] args)
        {
            //setup list 
            Console.WriteLine("Program Setup");
            for (int i = 0; i < 80; i++)  
            {  //Doing this EIGHTY TIMES... (need to initialize)
                tList[i] = new Thread(MatrixStream);//New Thread initialization
                toKids[i]=new AutoResetEvent(false);//toKid event/semaphore initialization
                toMain[i]=new AutoResetEvent(false);//toMain event/semaphore initialization
                tList[i].Start(i);  //NewThread start
            }  
            for(int j=0;j<28;j++)
            {
                for (int i=0; i<80;i++){
                    toKids[i].Set();//Notify ALL kidThreads
                }

//////////////// DELETED CODE
//////////////// DELETED CODE
//////////////// DELETED CODE

               /********* WHAT GOES HERE? **************/
               /********* I need something to make this Main thread 'sleep' until each of the kidThreads are 'done' computing a character
               ************** FIX THIS SECTION OF CODE ******************/
               for (int i=0; i<80;i++){
                    toMain[i].WaitOne();//Notify ALL kidThreads
                }

                Console.WriteLine($"Line # {j} is: {new string(myLine)}");
            }
            kidsCanRun=false;
            for (int i = 0; i < 80; i++)  
            {   toKids[i].Set(); //one last run to break the WAIT, and then the Kid will hear the 'kidsCanRun=False' setting and DIE!
                tList[i].Join();
            }
            Console.WriteLine("All Threads are done.");
        }
        static void MatrixStream(object id)  
        {  
            int i=(int) id;
            char [] choseList= {'a','b','c','d','e'};//Required: use A,B,C,D,E and then cycle back to A,B,C,D,E cycle to A,B,C,D,E ...continued...
            int position=0;
            while(kidsCanRun)
            {
                try  
                {  
                    //Blocks the current thread until the current WaitHandle receives a signal.   
                    toKids[i].WaitOne();
                    if (position>=choseList.Length) 
                    { position=0;
                        //reset to start of string.
                    }
                    myLine[i]= choseList[position];
                    position++;
                }  
                finally  
                {  
                    //method to singal semaphore  to main
                    toMain[i].Set();
                }
            }   
        }
    }
}
