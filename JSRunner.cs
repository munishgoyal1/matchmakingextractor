using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace JSRunner
{
    class JSRunner
    {
        static void Main(string[] args)
        {
            RunJS();
        }

        public static void RunJS()
        {
            int profiles = 70000;
            int workPerInstance = 10000;

            int workChunk = profiles / workPerInstance;

            for (int i = 0; i < workChunk; i++)
            {
            // Prepare the process to run 
            ProcessStartInfo start = new ProcessStartInfo();
            string jsExeFolder = @"..\..\..\jeevansathi\bin\debug";

            // Copy JS exe folder
            //System.

            string jsExeLocation = Path.Combine(jsExeFolder, @"jeevansathi.exe");
            // Enter the executable to run, including the complete path 
            start.FileName = jsExeLocation;// Path.Combine(executingExeDir, jsExeLocation);

            // Do you want to show a console window? 
            start.WindowStyle = ProcessWindowStyle.Normal;// Hidden;
            start.CreateNoWindow = false;// true;

            
                int starting = i * workPerInstance;
                int ending = starting + workPerInstance;
                // Enter in the command line arguments, everything you would enter after the executable name itself 
                start.Arguments = string.Format("{0} {1} {2}", starting, ending, i);

                //Logger.LogIt(Directory.GetCurrentDirectory());
                //Thread.Sleep(1000 * 3);
                using (Process proc = Process.Start(start))
                {
                    Console.WriteLine(string.Format("Process # {0} started", jsExeLocation));
                }
            }

            // once started, this should return
        }
    }
}
