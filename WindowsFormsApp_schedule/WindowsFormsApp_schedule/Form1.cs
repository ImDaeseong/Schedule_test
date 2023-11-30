using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp_schedule
{
    public partial class Form1 : Form
    {
        private IScheduler scheduler;

        public Form1()
        {
            InitializeComponent();
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            //초기화
            await InitQuartz();

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


        // 스케줄러 시작
        private void button1_Click(object sender, EventArgs e)
        {
            if (!scheduler.IsStarted)
            {
                scheduler.Start();
            }
        }


        //스케줄러에 대한 모든 리소스 정리 및 해제
        private void button2_Click(object sender, EventArgs e)
        {
            scheduler.Shutdown();
        }


        //현재 실행 중인 모든 작업 일시 중지
        private void button3_Click(object sender, EventArgs e)
        {
            scheduler.PauseAll();
        }


        // 중지된 모든 작업 다시 시작
        private void button4_Click(object sender, EventArgs e)
        {
            scheduler.ResumeAll();
        }


        // 현재 실행 중인 스케줄 목록
        private async void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (scheduler.IsStarted)
                {
                    IReadOnlyCollection<IJobExecutionContext> jobs = await scheduler.GetCurrentlyExecutingJobs();
                    IReadOnlyList<IJobExecutionContext> jobList = jobs.ToList();

                    if (jobList.Count > 0)
                    {
                        foreach (IJobExecutionContext jobContext in jobList)
                        {
                            IJobDetail jobDetail = jobContext.JobDetail;
                            string jobName = jobDetail.Key.Name;
                            string jobGroup = jobDetail.Key.Group;
                            Console.WriteLine($"Name: {jobName}, Group: {jobGroup}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }


        //스케줄추가
        private async void button6_Click(object sender, EventArgs e)
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