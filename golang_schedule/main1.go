package main

import (
	"fmt"
	"os"
	"os/exec"
	"os/signal"
	"sync"

	"github.com/robfig/cron"
)

func runExeFile(exePath string) error {
	cmd := exec.Command(exePath)
	err := cmd.Run()
	if err != nil {
		return fmt.Errorf("%s 실행 중 오류 발생: %v", exePath, err)
	}
	return nil
}

func job1() {
	fmt.Println("job1: 10초마다 실행")
}

func job2() {
	fmt.Println("job2: 20초마다 실행")
}

func job3() {
	fmt.Println("job3: 1분마다 실행")
}

func job4() {
	fmt.Println("job4: 월요일 10:30마다 실행")
	path := "C:\\mybatch.exe"
	if err := runExeFile(path); err != nil {
		fmt.Println(err)
	}
}

func job5() {
	fmt.Println("job5: 매일 2시 실행")
}

func runScheduler(wg *sync.WaitGroup) {
	defer wg.Done()

	c := cron.New()

	c.AddFunc("@every 10s", func() {
		job1()
	})

	c.AddFunc("@every 20s", func() {
		job2()
	})

	c.AddFunc("@every 1m", func() {
		job3()
	})

	c.AddFunc("0 10:30 * * MON", func() {
		job4()
	})

	c.AddFunc("0 2 * * *", func() {
		job5()
	})

	c.Start()
	defer c.Stop()

	select {}
}

func main() {
	var wg sync.WaitGroup
	wg.Add(1)

	go func() {
		defer wg.Done()
		runScheduler(&wg)
	}()

	// 종료키 Ctrl+C
	c := make(chan os.Signal, 1)
	signal.Notify(c, os.Interrupt)
	<-c

	fmt.Println("프로그램 종료")
	wg.Wait()
}
