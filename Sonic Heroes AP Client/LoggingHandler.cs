using System.Runtime.CompilerServices;

namespace Sonic_Heroes_AP_Client;

public enum LogLevel
{
    Error,
    APAction,
    GameAction,
    Info,
    Debug
}


public class LogEntry(string source, string message, LogLevel logLevel)
{
    public string Source { get; set; } = source;
    public string Message { get; set; } = message;
    public LogLevel LogLevel { get; set; } = logLevel;
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    
    //TODO fix this
    public bool shouldPrint = true;


    public override string ToString()
    {
        return $"{TimeStamp} - {LogLevel}: {Source} - {Message}";
    }


    public void PrintLog()
    {
        
    }
    
}


public class LoggingHandler
{
    

    public static void HandleLogEntry(string source, string message, LogLevel logLevel)
    {
        return;
    }
}