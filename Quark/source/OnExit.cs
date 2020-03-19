using System.Windows;

namespace Quark
{
    public partial class App : Application
    {
        void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                source.Globals.socketClient.Exit();
            }
            catch
            {

            }
           
        }
      
    }
}
