using System;
using CustomMalUpdaterDomain.Interfaces;

namespace CustomMalUpdaterServices
{
    public class TextLogger : ILogger
    {
        private string _logFilePath;

        public TextLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }
        
        public void Log(string message)
        {
            if(!System.IO.File.Exists(_logFilePath))
                System.IO.File.WriteAllText(_logFilePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm ddd") + " " + message + "\r\n");

            System.IO.File.AppendAllText(_logFilePath, DateTime.Now.ToString("yyyy-MM-dd HH:mm ddd") + " " + message + "\r\n");
        }
    }
}
