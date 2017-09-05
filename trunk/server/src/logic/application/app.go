package application

import (
	"fmt"
	"logic/game"

	"logic/socket"
	"net"
	"suzuki/logs"
)

type App struct {
	l net.Listener
}

func (this *App) Run() {
	var (
		err        error
		conn       net.Conn
		endRunning = make(chan bool, 1)
	)
	logs.Init()

	err = game.LoadUnitTable("../../../config/tables/entity.csv")
	if err != nil {
		fmt.Println("LoadUnitTable", err.Error())
		return
	}

	err = game.LoadSkillTable("../../../config/tables/skill.csv")
	if err != nil {
		fmt.Println("LoadSkillTable", err.Error())
		return
	}

	err = game.LoadBuffTable("../../../config/tables/buff.csv")
	if err != nil {
		fmt.Println("LoadBuffTable", err.Error())
		return
	}

	err = game.LoadBattleTable("../../../config/tables/Battle.csv")
	if err != nil {
		fmt.Println("LoadBattleTable", err.Error())
		return
	}

	err = game.LoadStoryChapterTable("../../../config/tables/HeroStroy.csv")
	if err != nil {
		fmt.Println("LoadStoryTable", err.Error())
		return
	}

	err = game.LoadSmallChapterTable("../../../config/tables/Checkpoint.csv")
	if err != nil {
		fmt.Println("LoadSmallChapterTable", err.Error())
		return
	}

	err = game.LoadItemTable("../../../config/tables/Item.csv")
	if err != nil {
		fmt.Println("LoadItemTable", err.Error())
		return
	}

	game.InitLua("../../../config/scripts/")

	//game.InitGlobalLuaState()
	game.InitTianTi()
	//game.TestPlayer()
	this.l, err = net.Listen("tcp", "0.0.0.0:10999")
	if err != nil {
		fmt.Println(err.Error())
		return
	}


	go func() {
		for {
			conn, err = this.l.Accept()
			if err != nil {
				fmt.Println(err.Error())
				endRunning <- true
			}
			fmt.Println("Has one connect ")
			peer := socket.NewPeer(conn)
			client := game.NewClient(peer)
			//
			go client.Update()
		}
	}()

	<-endRunning

	logs.Fini()
}

func NewApp() *App {
	return &App{}
}
