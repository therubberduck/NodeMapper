using System.Windows;
using System.Windows.Threading;

namespace NodeMapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(
                "An unhandled exception just occured: " + e.Exception.Message,
                e.Exception.GetType().ToString(),
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }
    }
}