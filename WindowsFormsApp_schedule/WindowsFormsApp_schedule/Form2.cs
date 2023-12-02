using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp_schedule
{
    public partial class Form2 : Form
    {
        private IScheduler scheduler;

        public Form2()
        {
            InitializeComponent();
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            //초기화
            await InitQuartz();

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            scheduler.Shutdown();
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

        // 10초 마다 실행
        private async void button1_Click(object sender, EventArgs e)
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
        private async void button2_Click(object sender, EventArgs e)
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
        private async void button3_Click(object sender, EventArgs e)
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
        private async void button4_Click(object sender, EventArgs e)
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

    }
}
