using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Windows;
using System.Collections.Generic;

namespace Quark.source.Utils
{
    /// <summary>
    /// This [Singleton] class Communicating with websocket server.
    /// 
    /// Method (GetSettings) sets server's ip and port. It takes it from Config.json.
    /// Method (Connect) trying to connect to a specific server. ip and port storage in vars ip and port.
    /// 
    /// </summary>
    public class WebSocketClient
    {
        private string ip;
        private int port;
        public WebsocketClient client;

        public static WebSocketClient instance;
        public static WebSocketClient get_instance()
        {
            if (instance == null)
                instance = new WebSocketClient();
            return instance;
            
        }
        private WebSocketClient() { }
        private void GetSettings()
        {
            try
            {
                JObject _obj = JObject.Parse(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.json")));
                this.ip = (string)_obj["host"]["ip"];
                this.port = (int)_obj["host"]["port"];
            }
            catch (Exception)
            {
                MessageBox.Show("Отсутсвует файл Config.json", "FATAL", MessageBoxButton.OK, MessageBoxImage.Error);
                Globals.Logger.Error("FATAL: Missing Config.json");
                Environment.Exit(-4);
            }

        }

        public async void Connect()
        {
            GetSettings();
            try
            {
                var exitEvent = new ManualResetEvent(false);
                var url = new Uri($"ws://{ip}:{port.ToString()}");

                this.client = new WebsocketClient(url);

                this.client.ReconnectTimeout = null;
                this.client.ReconnectionHappened.Subscribe(info =>
                    Console.WriteLine($"Reconnection happened, type: {info.Type}"));

                this.client.MessageEncoding = Encoding.Default;
                await client.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла фатальная ошибка.\nПерезапустите программу или обратитесь к администратору.", "FATAL", MessageBoxButton.OK, MessageBoxImage.Error);
                Globals.Logger.Error("FATAL: " + e.ToString());
            }

        }

        public void Send(JObject data, Delegate handler, List<object> obj)
        {
            try
            {
                Task.Run(() => this.client.Send(data.ToString()));
                Task.Run(() => this.client.MessageReceived.Subscribe(msg => { handler.DynamicInvoke(JObject.Parse(msg.Text), obj); }));
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла фатальная ошибка.\nПерезапустите программу или обратитесь к администратору.", "FATAL", MessageBoxButton.OK, MessageBoxImage.Error);
                Globals.Logger.Error("FATAL: " + e.ToString());
            }
        }

        public void Exit()
        {
            this.client.Stop(WebSocketCloseStatus.NormalClosure, "");
            this.client.Dispose();
        }

    }
}
