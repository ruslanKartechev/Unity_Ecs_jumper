using UnityEngine;

namespace Helpers
{
    public class Dbg
    {
        public static bool EnableDebugs { get; set; } = true;
        
        public static void LogRed(string message)
        {
            Log("<color=red>" + message + "</color>");
        }
        
        public static void LogGreen(string message)
        {
            Log("<color=green>" + message + "</color>");
        }
        
        public static void LogBlue(string message)
        {
            Log("<color=blue>" + message + "</color>");
        }
        
        public static void LogYellow(string message)
        {
            Log("<color=yellow>" + message + "</color>");
        }

        public static void LogException(string message)
        {
            Log("<color=red> " + "CAUGHT: " + message + "</color>");
        }
        
        public static void LogStars(string message)
        {
            Log( "*****" + message);
        }

        public static void Log(string message)
        {
            if(EnableDebugs)
                Debug.Log($"{message}");
        }
    }
}