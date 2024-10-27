using System;

namespace Logger
{
    public class ConsoleLogger : ILogger
    {
        public LogLevel Level { get; set; }

        public ConsoleLogger() {
            Level = LogLevel.Log | LogLevel.Warring | LogLevel.Error;
        }

        void ILogger.Error_Internal(string info, Exception? e){
            Console.Error.WriteLine($"{info}: {e?.Message}{Environment.NewLine}{e}");
        }

        void ILogger.Log_Internal(string info){
            Console.WriteLine(info);
        }

        void ILogger.Warring_Internal(string info){
            Console.WriteLine("Warring: " + info);
        }

        void ILogger.LogFormat_Internal(string format, params object[] args) {
            Console.WriteLine(format, args);
        }

        void ILogger.WarringFormat_Internal(string format, params object[] args) {
            Console.WriteLine("Warring: " + format, args);
        }

        void ILogger.ErrorFormart_Internal(string format, params object[] args){
            Console.WriteLine(format, args);
        }
    }
}
