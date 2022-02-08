using System;
using System.IO;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            Pathfinder fileLog = new Pathfinder(new FileLogWritter());
            fileLog.Find("пишет лог в файл.");

            Pathfinder consoleLog = new Pathfinder(new ConsoleLogWritter());
            consoleLog.Find("пишет лог в консоль.");

            Pathfinder fridayLogToFile = new Pathfinder(new SecureConsoleLogWritter(new FileLogWritter()));
            fridayLogToFile.Find("пишет лог в файл по пятницам.");

            Pathfinder fridayLogToConsole = new Pathfinder(new SecureConsoleLogWritter(new ConsoleLogWritter()));
            fridayLogToConsole.Find("пишет лог в консоль по пятницам.");

            Pathfinder consoleLogAndfridayToFile = new Pathfinder(LoggerChain.Create(new ConsoleLogWritter(), new SecureConsoleLogWritter(new FileLogWritter())));
            consoleLogAndfridayToFile.Find("пишет лог в консоль а по пятницам ещэ и в файл.");
        }
    }

    interface ILogger
    {
        void WriteError(string message);
    }

    class Pathfinder
    {
        private readonly ILogger _logger;

        public Pathfinder(ILogger logger)
        {
            _logger = logger;
        }

        public void Find(string message)
        {
            _logger.WriteError(message);
        }
    }

    class LoggerChain : ILogger
    {
        private readonly ILogger[] _loggers;

        public LoggerChain(ILogger[] loggers)
        {
            _loggers = loggers;
        }

        public void WriteError(string message)
        {
            foreach (ILogger logger in _loggers)
                logger.WriteError(message);
        }

        public static LoggerChain Create(params ILogger[] loggers)
        {
            return new LoggerChain(loggers);
        }
    }

    class ConsoleLogWritter : ILogger
    {
        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWritter : ILogger
    {
        public void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class SecureConsoleLogWritter : ILogger
    {
        private readonly ILogger _logger;

        public SecureConsoleLogWritter(ILogger logger)
        {
            _logger = logger;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                _logger.WriteError(message);
        }
    }
}