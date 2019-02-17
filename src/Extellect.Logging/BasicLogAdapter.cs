using System;
using log4net;

namespace Extellect.Logging
{
    public class BasicLogAdapter : IBasicLog
    {
        private readonly ILog _log;

        public BasicLogAdapter(ILog log)
        {
            _log = log;
        }

        public bool IsFatalEnabled => _log.IsFatalEnabled;

        public bool IsWarnEnabled => _log.IsWarnEnabled;

        public bool IsInfoEnabled => _log.IsInfoEnabled;

        public bool IsDebugEnabled => _log.IsDebugEnabled;

        public bool IsErrorEnabled => _log.IsErrorEnabled;

        public void Debug(string message)
        {
            _log.Debug(message);
        }

        public void Error(string message)
        {
            _log.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void Fatal(string message)
        {
            _log.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        public void Info(string message)
        {
            _log.Info(message);
        }

        public void Warn(string message)
        {
            _log.Warn(message);
        }
    }
}
