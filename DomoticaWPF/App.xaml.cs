using System.Diagnostics;
using System.Windows;

namespace DomoticaWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Config.GetInstance().Logger.TraceEvent(TraceEventType.Information, 1, "Aplicación DomoticaWPF iniciada.");
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            Config.GetInstance().Logger.TraceEvent(TraceEventType.Information, 2, "Aplicación DomoticaWPF finalizada (Exit).");
        }
    }
}
