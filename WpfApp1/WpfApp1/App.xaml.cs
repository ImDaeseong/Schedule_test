using System;
using System.Linq;
using System.Threading;
using System.Windows;

namespace WpfApp1
{
    public partial class App : Application
    {
        private static Mutex mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            string sMmutexName = "WpfApp1_Application";
            bool bCreatedNew = false;
            try
            {
                mutex = new Mutex(true, sMmutexName, out bCreatedNew);
                if (bCreatedNew)
                {
                    base.OnStartup(e);
                }
                else
                {
                    MessageBox.Show("이미 다른 인스턴스가 실행 중입니다.", "알림", MessageBoxButton.OK, MessageBoxImage.Information);
                    Current.Shutdown();
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        private void Application_Activated(object sender, EventArgs e)
        {
            Console.WriteLine("애플리케이션이 활성화될 때");

            MainWindow main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            
        }

        private void Application_Deactivated(object sender, EventArgs e)
        {
            Console.WriteLine("애플리케이션이 비활성화될 때");

            MainWindow main = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // 여기에서 예외 처리 또는 로깅을 수행
            MessageBox.Show($"예외가 발생했습니다: {e.Exception.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);

            // 예외가 처리되었다고 표시
            e.Handled = true;
        }
    }
}
