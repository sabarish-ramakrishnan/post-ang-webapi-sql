using NLog;

namespace post_ang_webapi_sql.Services
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
 
        public LoggerManager()
        {
        }
 
        public void LogDebug(string message)
        {
            logger.Debug(message);
        }
 
        public void LogError(string message)
        {
            logger.Error(message);
        }
 
        public void LogInfo(string message)
        {
            logger.Info(message);
        }
 
        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }

    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogWarn(string message);
        void LogDebug(string message);
        void LogError(string message);
    }
}