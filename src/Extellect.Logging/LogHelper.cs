using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net.Config;
using System.IO;
using log4net;
using System.Reflection;
using log4net.Repository;

namespace Extellect.Logging
{
    /// <summary>
    /// Exposes static methods that reuse common routines in setting up log4net
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// Initialises log4net with the default config file and throws an error if the file cannot be found.
        /// </summary>
        /// <param name="loggerName">Name of the default logger to be returned.</param>
        /// <returns>A configured logger.</returns>
        public static ILog Init(string loggerName)
        {
            Configure();

            var caller = Assembly.GetCallingAssembly();

            var callingAssemblyFullName = caller != null ? caller.FullName : "No calling assembly information found";

            var log = LogManager.GetLogger("DEFAULT", loggerName);

            log.Info(callingAssemblyFullName);

            log.Info("Logger configured and initialised");

            return log;
        }

        /// <summary>
        /// Initialises log4net with the default config file and throws an error if the file cannot be found.
        /// </summary>
        public static void Configure()
        {
            Configure("log4net.config", true);
        }

        private static ILoggerRepository Repository => LogManager.CreateRepository("DEFAULT");

        /// <summary>
        /// Initialises log4net with custom options.
        /// </summary>
        /// <param name="relativePath">Path to log4net configuration file, relative to current appdomain base directory</param>
        /// <param name="isRequired"></param>
        public static void Configure(string relativePath, bool isRequired)
        {
            var logConfig = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath));
            if (!logConfig.Exists)
            {
                var message = $"Required configuration file '{logConfig.FullName}' is missing. Unable to continue...";
                if (isRequired)
                {
                    throw new FileNotFoundException(message, logConfig.Name);
                }
                else
                {
                    Console.Error.WriteLine(message);
                }
            }
            else
            {
                XmlConfigurator.Configure(Repository, logConfig);
            }
        }
    }
}