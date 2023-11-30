import asyncio
from Schedule1 import Schedule1
from Schedule2 import Schedule2


async def run1():
    scheduler = Schedule1()
    await scheduler.runschedule()


async def run2():
    scheduler2 = Schedule2()
    await scheduler2.runschedule()


async def run3():
    scheduler1 = Schedule1()
    scheduler2 = Schedule2()

    # 2개 동시 실행
    await asyncio.gather(
        scheduler1.runschedule(),
        scheduler2.runschedule()
    )


if __name__ == '__main__':
    # 한개씩 실행
    asyncio.run(run1())
    # asyncio.run(run2())

    # 동시에 실행
    # asyncio.run(run3())
