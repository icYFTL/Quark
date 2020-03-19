using System.Collections.Generic;

namespace Quark.source
{
    public static class Globals
    {
        public static Utils.WebSocketClient socketClient;
        public static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static string AppDataPath;
        public static Dictionary<string, string> User;
    }
}
