using System;
using System.ServiceProcess;

namespace Extellect.Utilities.Services
{
    /// <summary>
    /// Adapts a Windows service into a regular console application for easier debugging.
    /// To use this class, change the following project properties:
    /// 1) On the Application tab set Output Type to Console Application
    /// 2) On the Debug tab set the Start Options -> Command Line Arguments to debug
    /// </summary>
    /// <typeparam name="TServiceInterface"></typeparam>
    public class DebuggableService<TServiceInterface> 
        where TServiceInterface : IService, new()
    {
        public static void Start(string[] args)
        {
            if (Environment.UserInteractive && args.Length > 0 && args[0].Equals("debug", StringComparison.OrdinalIgnoreCase))
            {
                var destinationArray = new string[args.Length - 1];
                Array.Copy(args, 1, destinationArray, 0, args.Length - 1);
                Debug(destinationArray);
            }
            else
            {
                Release(args);
            }
        }

        private static void Debug(string[] args)
        {
            IService service = new TServiceInterface();
            Console.WriteLine("Starting service...");
            service.Start(args);
            Console.WriteLine("Service started. Press enter to stop...");
            Console.ReadLine();
            Console.WriteLine("Stopping service...");
            service.Stop();
            Console.WriteLine("Service is stopped.");
        }

        private static void Release(string[] args)
        {
            ServiceBase.Run(
                new ServiceBase[]
                    {
                        new WindowsServiceAdaptor(new TServiceInterface()),
                    });
        }
    }
}
