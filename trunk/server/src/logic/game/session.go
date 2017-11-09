package game

import (
	"logic/prpc"

	"bytes"

	"encoding/binary"
	"jimny/logs"
	"jimny/network"
	"net"
	"sync"
)

type Session struct {
	sync.Mutex
	prpc.COM_ServerToClientStub

	username string
	player   *GamePlayer

	IncomingBuffer, OutgoingBuffer *bytes.Buffer
	recvChannel                    <-chan []byte
	sendChannel                    chan<- []byte
	Connection                     *network.Conn
}

func (this *Session) Login(info prpc.COM_LoginInfo) error {
	logs.Debug("Login ", info)
	infoext := prpc.COM_AccountInfo{}
	infoext.SessionCode = info.Username + info.Password

	this.username = info.Username

	this.player = FindPlayerByUsername(info.Username)

	if this.player == nil {
		p := prpc.SGE_DBPlayer{Username: info.Username}

		if QueryPlayer(&p) {
			this.player = &GamePlayer{}
			this.player.SetSession(this)
			this.player.SetPlayerSGE(p)
			if FindPlayerByUsername(info.Username) != nil {
				for k, n := range PlayerStore {
					if n == nil {
						continue
					}
					if n.Username == info.Username {
						PlayerStore[k] = this.player
					}
				}
			} else {
				PlayerStore = append(PlayerStore, this.player)
			}
			infoext.MyPlayer = p.COM_Player

			logs.Debug("Query player ", p)
		}
	} else {
		this.player.SetSession(this)
		infoext.MyPlayer = this.player.GetPlayerCOM()
	}

	this.LoginOK(infoext)
	if this.player != nil {
		this.player.PlayerLogin()
	}

	return nil
} // 0
func (this *Session) CreatePlayer(tempId int32, playerName string) error {

	if FindPlayerByInstName(playerName) != nil {
		return nil
	}

	this.player = CreatePlayer(tempId, playerName)
	this.player.SetSession(this)
	this.player.Username = this.username

	if this.player != nil {
		this.player.PlayerLogin()
	}

	r := this.player.GetPlayerSGE()

	InsertPlayer(r)

	this.CreatePlayerOK(r.COM_Player)

	logs.Debug("CreatePlayer ", r)

	return nil
} // 1
func (this *Session) AddBattleUnit(instId int64, groupId int32) error {
	logs.Info("SetBattleUnit", instId)
	this.player.SetBattleUnit(instId)

	r := this.player.GetBattleUnit(instId)

	this.SetBattleUnitOK(r.InstId)

	logs.Info("SetBattleUnitOK")

	return nil
} // 2

func (this *Session) PopBattleUnit(instId int64, groupId int32) error {
	logs.Info("SetBattleUnit", instId)
	this.player.SetBattleUnit(instId)

	r := this.player.GetBattleUnit(instId)

	this.SetBattleUnitOK(r.InstId)

	logs.Info("SetBattleUnitOK")

	return nil
} // 3

func (this *Session) JoinBattle() error {

	if this.player == nil {
		return nil
	}

	this.player.JoinBattle()

	return nil
} // 4

func (this *Session) SetupBattle(positionList []prpc.COM_BattlePosition, skillid int32) error {
	logs.Info("SetupBattle", positionList)
	r := this.player.SetupBattle(positionList, skillid)

	if r != nil {
		return nil
	}

	this.SetupBattleOK()

	return nil
} // 5

func (this *Session) RequestChapterData(chapterId int32) error {
	if this.player == nil {
		return nil
	}
	this.player.GetMyChapterDataById(chapterId)

	return nil
}

func (this *Session) ChallengeSmallChapter(smallChapterId int32) error {
	if this.player == nil {
		return nil
	}
	logs.Info("1")
	this.player.AttackChapter(smallChapterId)

	return nil
}

func (this *Session) SetBattleUnit(instId int64, groupId int32, isBattle bool) error {
	if this.player == nil {
		return nil
	}

	this.player.SetBattleUnitGroup(instId, groupId, isBattle)

	return nil
}

func (this *Session) DelUnitGroup(groupId int32) error {
	if this.player == nil {
		return nil
	}
	this.player.DeleteUnitGroup(groupId)
	return nil
}

func (this *Session) StartMatching(groupId int32) error {
	if this.player == nil {
		return nil
	}
	StartMatching(this.player, groupId)
	return nil
}

func (this *Session) StopMatching() error {
	if this.player == nil {
		return nil
	}
	StopMatching(this.player)
	return nil
}

func (this *Session) DeleteItem(instId int64, stack int32) error {
	if this.player == nil {
		return nil
	}
	this.player.DelItemByInstId(instId, stack)
	return nil
}

func (this *Session) PromoteUnit(instId int64) error {
	if this.player == nil {
		return nil
	}
	this.player.PromoteUnit(instId)
	return nil
}

func (this *Session) RequestChapterStarReward(chapterId int32, star int32) error {
	if this.player == nil {
		return nil
	}
	this.player.GetChapterStarReward(chapterId, star)
	return nil
}

func (this *Session) EquipSkill(skillinfo prpc.COM_LearnSkill) error {
	if this.player == nil {
		return nil
	}
	this.player.EquipSkill(skillinfo)
	return nil
}

func (this *Session) SkillUpdate(skillindex int32, skillId int32) error {
	if this.player == nil {
		return nil
	}
	this.player.SkillUpdate(skillindex, skillId)
	return nil
}

func (this *Session) BuyShopItem(shopId int32) error {
	if this.player == nil {
		return nil
	}
	this.player.BuyShopItem(shopId)
	return nil
}

func (this *Session) ResolveItem(instId int64, num int32) error {
	if this.player == nil {
		return nil
	}
	this.player.CardDebrisResolve(instId, num)
	return nil
}

func (this *Session) RefreshBlackMarkte() error {
	if this.player == nil {
		return nil
	}
	this.player.RefreshMyBlackMarket(true)
	return nil
}

func (this *Session) NewPlayerGuide(Step uint64) error {
	if this.player == nil {
		return nil
	}
	this.player.NewPlayerGuide(Step)
	return nil
}

func (this *Session) SendChat(content prpc.COM_Chat) error {
	if this.player == nil {
		return nil
	}
	this.player.SendChat(content)
	return nil
}

func (this *Session) RequestAudio(audioId int64) error {
	if this.player == nil {
		return nil
	}
	this.player.RequestAudio(audioId)
	return nil
}

func (this *Session) AllTopByPage() error {
	if this.player == nil {
		return nil
	}
	this.player.AllTopByPage()
	return nil
}

func (this *Session) FriendTopByPage() error {
	if this.player == nil {
		return nil
	}
	this.player.FriendTopByPage()
	return nil
}

func (this *Session) SerchFriendByName(name string) error {
	if this.player == nil {
		return nil
	}
	this.player.SerchFriendByName(name)
	return nil
}

func (this *Session) SerchFriendRandom() error {
	if this.player == nil {
		return nil
	}
	this.player.SerchFriendRandom()
	return nil
}

func (this *Session) ProcessingFriend(name string) error {
	if this.player == nil {
		return nil
	}
	this.player.ProcessingFriend(name)
	return nil
}

func (this *Session) ApplicationFriend(name string) error {
	if this.player == nil {
		return nil
	}
	this.player.ApplicationFriend(name)
	return nil
}

func (this *Session) DeleteFriend(instid int64) error {
	if this.player == nil {
		return nil
	}
	this.player.DeleteFriend(instid)
	return nil
}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

func (this *Session) Tick() {

	select {
	case b := <-this.recvChannel:
		this.IncomingBuffer.Write(b)
		//logs.Debug(len(b))
	default:

		for this.IncomingBuffer.Len() >= 2 {
			//logs.Debug("Tick sessions 4")
			err := prpc.COM_ClientToServerDispatch(this.IncomingBuffer, this)
			if err != nil {
				logs.Error(err.Error())

			}

		}

		this.IncomingBuffer.Reset()

	}

	//do clean

	//if this.player != nil {
	//	this.player.Logout()
	//	this.player.SetSession(nil)
	//	this.player = nil
	//	this.Sender = nil
	//
	//	logs.Info("Socket close ")
	//}

}

//////////////////////////////////////////////////////////////
func (this *Session) MethodBegin() *bytes.Buffer {

	return this.OutgoingBuffer
}

func (this *Session) MethodEnd() error {
	logs.Debug("Methed end %d", this.OutgoingBuffer.Len())
	buffer := bytes.NewBuffer(nil)

	binary.Write(buffer, binary.LittleEndian, int16(this.OutgoingBuffer.Len()+2))
	binary.Write(buffer, binary.LittleEndian, this.OutgoingBuffer.Bytes())
	this.sendChannel <- buffer.Bytes()
	this.OutgoingBuffer.Reset()

	return nil
}

var sessionList = make([]*Session, 0)

func NewClient(conn *network.Conn) {
	logs.Debug("Player connected")
	c := Session{}
	c.Connection = conn
	conn.Handle().(*net.TCPConn).SetNoDelay(true)
	c.sendChannel, c.recvChannel = conn.Open()
	c.OutgoingBuffer = bytes.NewBuffer(nil)
	c.IncomingBuffer = bytes.NewBuffer(nil)
	c.Sender = &c
	sessionList = append(sessionList, &c)
}

func TickClient() {

	for _, s := range sessionList {
		s.Tick()
	}
}
