using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Windows;

namespace Quark.source.Utils
{
    public class WebSocketClient
    {
        private string ip;
        private int port;
        private WebsocketClient client;

        public WebSocketClient()
        {
            this.GetSettings();
        }
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
                Environment.Exit(-4);
            }

        }

        public async void Connect()
        {
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
                Globals.Logger.Error(e.ToString());
            }

        }

        public void Send(JObject data, Delegate handler)
        {

            this.client.MessageReceived.Subscribe(msg => { /*TODO*/});
            try
            {
                Task.Run(() => this.client.Send(data.ToString()));
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла фатальная ошибка.\nПерезапустите программу или обратитесь к администратору.", "FATAL", MessageBoxButton.OK, MessageBoxImage.Error);
                Globals.Logger.Error(e.ToString());
            }
        }

        public void Exit()
        {
            this.client.Stop(WebSocketCloseStatus.NormalClosure, "");
            this.client.Dispose();
        }

    }
}
