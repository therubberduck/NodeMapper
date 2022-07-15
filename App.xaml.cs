using System;
using System.Windows;
using System.Windows.Threading;

namespace NodeMapper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            var file = new System.IO.StreamWriter("errorlog.log", true);
            var dateStamp = DateTime.Now.ToString("yy/MM/dd HH:mm:ss");
            file.Write($"Log Line({dateStamp}):\n{e.Exception}\n\n\n");
            file.Close();
            
            e.Handled = true;
        }
        
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