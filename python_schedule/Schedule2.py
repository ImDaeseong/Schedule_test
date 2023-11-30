import asyncio
import schedule
import subprocess


class Schedule2:
    def __init__(self):
        self.schedule = schedule.Scheduler()

    async def run_exe_file(self, exe_path):
        try:
            subprocess.run(exe_path, check=True)
        except subprocess.CalledProcessError as e:
            print(f"Error executing {exe_path}: {e}")

    async def job1(self):
        print("스케줄1 실행")

    async def job2(self, message):
        print(f"스케줄2: {message}")

    async def job3(self):
        path = r"C:\mybatch.exe"
        await self.run_exe_file(path)

    def schedule1(self):
        self.schedule.every(10).seconds.do(lambda: asyncio.create_task(self.job2("schedule1 - 매 10초마다 실행")))
        self.schedule.every(1).minutes.do(lambda: asyncio.create_task(self.job2("schedule1 - 매 1분마다 실행")))

    def schedule2(self):
        self.schedule.every().monday.at("10:30").do(lambda: asyncio.create_task(self.job1()))

    def schedule3(self):
        self.schedule.every().day.at("02:00").do(lambda: asyncio.create_task(self.job3()))

    def schedule4(self):
        self.schedule.every(10).minutes.do(lambda: asyncio.create_task(self.job2("schedule4 - 매 10분마다 실행")))

    async def runschedule(self):
        try:
            while True:
                # 스케줄 초기화
                self.schedule.clear()

                self.schedule1()
                self.schedule2()
                self.schedule3()
                self.schedule4()

                while True:
                    self.schedule.run_pending()
                    await asyncio.sleep(1)
        except KeyboardInterrupt:
            print("프로그램 종료")

