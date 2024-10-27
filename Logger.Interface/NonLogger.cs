using System;

namespace Logger
{
    public class NonLogger : ILogger
    {
        LogLevel ILogger.Level { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        void ILogger.ErrorFormart_Internal(string format, params object[] args) {
            // do nothing
        }

        void ILogger.Error_Internal(string info, Exception? e) {
            // do nothing
        }

        void ILogger.LogFormat_Internal(string format, params object[] args) {
            // do nothing
        }

        void ILogger.Log_Internal(string info) {
            // do nothing
        }

        void ILogger.WarringFormat_Internal(string format, params object[] args) {
            // do nothing
        }

        void ILogger.Warring_Internal(string info) {
            // do nothing
        }
    }
}
