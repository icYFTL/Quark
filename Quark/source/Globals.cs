using System.Collections.Generic;

namespace Quark.source
{
    public static class Globals
    {
        public static Utils.WebSocketClient socketClient;
        public static NLog.Logger Logger;
        public static string AppDataPath;
        public static Dictionary<string, string> User;
    }
}
