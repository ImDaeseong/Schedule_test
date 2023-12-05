using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow1 : Window
    {
        private IScheduler scheduler;

        public MainWindow1()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Window_Loaded");

            //초기화
            await InitQuartz();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine("Window_Closing");

            scheduler.Shutdown();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("Window_Closed");
        }

        // 10초 마다 실행
        private async void Button_Click1(object sender, RoutedEventArgs e)
        {
            try
            {
                IJobDetail jobDetail = JobBuilder.Create<QuartzJob1>()
                    .WithIdentity("QuartzJob1", "Group1")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("QuartzJob1Trigger", "group1")
                    .WithSchedule(SimpleScheduleBuilder.Create()
                        .WithIntervalInSeconds(10)
                        .RepeatForever())
                    .Build();

                await scheduler.ScheduleJob(jobDetail, trigger);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // 1분 마다 실행
        private async void Button_Click2(object sender, RoutedEventArgs e)
        {
            try
            {
                IJobDetail jobDetail = JobBuilder.Create<QuartzJob2>()
                    .WithIdentity("QuartzJob2", "Group2")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("QuartzJob2Trigger", "group2")
                    .WithSchedule(SimpleScheduleBuilder.Create()
                        .WithIntervalInMinutes(1)
                        .RepeatForever())
                    .Build();

                await scheduler.ScheduleJob(jobDetail, trigger);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // 매주 월요일 10:30
        private async void Button_Click3(object sender, RoutedEventArgs e)
        {
            try
            {
                IJobDetail jobDetail = JobBuilder.Create<QuartzJob4>()
                    .WithIdentity("QuartzJob4", "Group4")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("QuartzJob4Trigger", "group4")
                    .WithSchedule(CronScheduleBuilder.CronSchedule("0 30 10 ? * MON"))
                    .Build();

                await scheduler.ScheduleJob(jobDetail, trigger);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // 매일 오전 2시
        private async void Button_Click4(object sender, RoutedEventArgs e)
        {
            try
            {
                IJobDetail jobDetail = JobBuilder.Create<QuartzJob5>()
                    .WithIdentity("QuartzJob5", "Group5")
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("QuartzJob5Trigger", "group5")
                    .WithSchedule(CronScheduleBuilder.CronSchedule("0 0 2 * * ?"))
                    .Build();

                await scheduler.ScheduleJob(jobDetail, trigger);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task InitQuartz()
        {
            try
            {
                scheduler = await StdSchedulerFactory.GetDefaultScheduler();

                // 스케줄러 시작
                await scheduler.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
