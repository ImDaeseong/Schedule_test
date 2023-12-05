using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private IScheduler scheduler;

        public MainWindow()
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

        // 스케줄러 시작
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if (!scheduler.IsStarted)
            {
                scheduler.Start();
            }
        }

        //스케줄러에 대한 모든 리소스 정리 및 해제
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            scheduler.Shutdown();
        }

        //현재 실행 중인 모든 작업 일시 중지
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            scheduler.PauseAll();
        }

        // 중지된 모든 작업 다시 시작
        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            scheduler.ResumeAll();
        }

        // 현재 실행 중인 스케줄 목록
        private async void Button_Click5(object sender, RoutedEventArgs e)
        {
            // 그룹 리스트
            IReadOnlyCollection<string> grouplist = await scheduler.GetJobGroupNames();
            foreach (string group in grouplist)
            {
                //Console.WriteLine($"그룹명: {group}");

                // Job 그룹 내의 Job 목록 가져오기
                IReadOnlyCollection<JobKey> joblist = await scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(group));
                foreach (JobKey jobKey in joblist)
                {
                    //Console.WriteLine($"일: {jobKey.Name}, 그룹: {jobKey.Group}");

                    // JobDetail
                    JobDetailImpl jobDetail = (JobDetailImpl)await scheduler.GetJobDetail(jobKey);
                    if (jobDetail != null)
                    {
                        Console.WriteLine($"Description: {jobDetail.Description}, JobDataMap: {jobDetail.JobDataMap}");
                    }
                }
            }
        }

        //스케줄추가
        private async void Button_Click6(object sender, RoutedEventArgs e)
        {
            //10초마다
            await ScheduleJob<QuartzJob1>("QuartzJob1", "Group1", "0/10 * * * * ?");

            //1분마다
            await ScheduleJob<QuartzJob2>("QuartzJob2", "Group2", "0 */1 * * * ?");

            //5분마다
            await ScheduleJob<QuartzJob3>("QuartzJob3", "Group3", "0 */5 * * * ?");

            // 매주 월요일 10:30
            await ScheduleJob<QuartzJob4>("QuartzJob4", "Group4", "0 30 10 ? * MON");

            // 매일 오전 2시
            await ScheduleJob<QuartzJob5>("QuartzJob5", "Group5", "0 0 2 * * ?");
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

        // 스케줄 추가 함수
        private async Task ScheduleJob<T>(string jobName, string jobGroup, string cronExpression) where T : IJob
        {
            try
            {
                IJobDetail jobDetail = JobBuilder.Create<T>()
                    .WithIdentity(jobName, jobGroup)
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity($"{jobName}Trigger", jobGroup)
                    .WithCronSchedule(cronExpression)
                    .Build();

                await scheduler.ScheduleJob(jobDetail, trigger);

                Console.WriteLine($"스케줄: {jobName} 등록");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error {jobName}: {ex.Message}");
            }
        }

    }


    public class QuartzJob1 : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("QuartzJob1 10초마다 실행");
            return Task.CompletedTask;
        }
    }

    public class QuartzJob2 : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("QuartzJob2 1분마다 실행");
            return Task.CompletedTask;
        }
    }

    public class QuartzJob3 : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("QuartzJob3 5분마다 실행");

            try
            {
                string sPath = @"C:\batch.exe";
                System.Diagnostics.Process.Start(sPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
            }

            return Task.CompletedTask;
        }
    }

    public class QuartzJob4 : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("QuartzJob4 매주 월요일 10시30분 실행");
            return Task.CompletedTask;
        }
    }

    public class QuartzJob5 : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("QuartzJob5 매일 오전 2시 실행");
            return Task.CompletedTask;
        }
    }

}
