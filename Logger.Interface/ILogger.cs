using System;

namespace Logger
{
    [Flags]
    public enum LogLevel
    {
        Non = 0,
        Log = 1,
        Warring = 2,
        Error = 4
    }

    public interface ILogger
    {
        public LogLevel Level { get; set; }

        public void Log(string info)
        {
            if (Level.HasFlag(LogLevel.Log))
            {
                Log_Internal(info);
            }
        }

        public void LogFormat(string format, params object[] args)
        {
            if (Level.HasFlag(LogLevel.Log))
            {
                LogFormat_Internal(format, args);
            }
        }

        public void Warring(string info)
        {
            if (Level.HasFlag(LogLevel.Warring))
            {
                Warring_Internal(info);
            }
        }

        public void WarringFormat(string format, params object[] args)
        {
            if (Level.HasFlag(LogLevel.Warring))
            {
                WarringFormat_Internal(format, args);
            }
        }

        public void Error(string info, Exception? e = null)
        {
            if (Level.HasFlag(LogLevel.Error))
            {
                Error_Internal(info, e);
            }
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (Level.HasFlag(LogLevel.Error))
            {
                ErrorFormart_Internal(format, args);
            }
        }

        protected void Log_Internal(string info);

        protected void LogFormat_Internal(string format, params object[] args);

        protected void Warring_Internal(string info);

        protected void WarringFormat_Internal(string format, params object[] args);

        protected void Error_Internal(string info, Exception? e);

        protected void ErrorFormart_Internal(string format, params object[] args);
    }
}
