import asyncio
import schedule
import subprocess


class Schedule1:
    def __init__(self):
        self.schedule = schedule.Scheduler()

    def run_exe_file(self, exe_path):
        try:
            subprocess.run(exe_path, check=True)
        except subprocess.CalledProcessError as e:
            print(f"{exe_path} 실행 중 오류 발생: {e}")

    def job1(self):
        print("스케줄1 실행")

    def job2(self, message):
        print(f"스케줄2: {message}")

    def job3(self):
        path = r"C:\mybatch.exe"
        self.run_exe_file(path)

    def schedule1(self):
        self.schedule.every(10).seconds.do(self.job2, message="schedule1 - 매 10초마다 실행")
        self.schedule.every(1).minutes.do(self.job2, message="schedule1 - 매 1분마다 실행")

    def schedule2(self):
        self.schedule.every().monday.at("10:30").do(self.job1)

    def schedule3(self):
        self.schedule.every().day.at("02:00").do(self.job3)

    def schedule4(self):
        self.schedule.every(10).minutes.do(self.job2, message="schedule4 - 매 10분마다 실행")

    async def runschedule(self):
        try:
            # 스케줄 초기화
            self.schedule.clear()

            self.schedule1()
            self.schedule2()
            self.schedule3()
            self.schedule4()

            while True:
                # 1초 대기
                await asyncio.sleep(1)

                # 대기 중인 스케줄 실행
                self.schedule.run_pending()
        except KeyboardInterrupt:
            print("프로그램 종료")
