using System;

namespace BlockTech;

public static class Logger
{
    private static LoggerLevel Level = LoggerLevel.Info;

    public static void SetLevel(LoggerLevel level)
    {
        Level = level;
    }

    public static void Debug(string message) {
        if (Level <= LoggerLevel.Debug)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\x1b[1m[DEBUG]\x1b[0m " + message);
            Console.ResetColor();
        }
    } 
    public static void Info(string message) {
        if (Level <= LoggerLevel.Info)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
        
            Console.WriteLine("\x1b[1m[INFO]\x1b[0m " + message);
            Console.ResetColor();
        }
    } 
    public static void Warn(string message) {
        if (Level <= LoggerLevel.Warn)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        
            Console.WriteLine("\x1b[1m[WARN]\x1b[0m " + message);
            Console.ResetColor();
        }
    } 
    public static void Error(string message) {
        if (Level <= LoggerLevel.Error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        
            Console.WriteLine("\x1b[1m[ERROR]\x1b[0m " + message);
            Console.ResetColor();
        }
    } 

    // This will crash the program
    public static void FatalError(string message, int exitCode) {
        Console.ForegroundColor = ConsoleColor.Red;
    
        Console.WriteLine("\x1b[1m[ERROR]\x1b[0m " + message);
        Console.ResetColor();
        Environment.Exit(exitCode);
    } 
    // This will crash the program
    public static void FatalError(string message) {
        Console.ForegroundColor = ConsoleColor.Red;
    
        Console.WriteLine("\x1b[1m[ERROR]\x1b[0m " + message);
        Console.ResetColor();
        Environment.Exit(-1);
    } 

}

public enum LoggerLevel
{
    Debug,
    Info,
    Warn,
    Error,
    FatalError
}
