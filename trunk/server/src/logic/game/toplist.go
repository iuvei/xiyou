package game

import (
	"logic/prpc"
	"strconv"

	"sort"
	"logic/log"
)

var TrueTopList []prpc.COM_TopUnit
var TMPTopList []prpc.COM_TopUnit

const (
	show_num = 5		//每页显示五个人
	Testpaiming = "测试用"
	num = 199
	tian = 10000
)


type TopList []prpc.COM_TopUnit

func (a TopList) Len() int {    // 重写 Len() 方法
	return len(a)
}
func (a TopList) Swap(i, j int){     // 重写 Swap() 方法
	a[i], a[j] = a[j], a[i]
}
func (a TopList) Less(i, j int) bool {    // 重写 Less() 方法， 从大到小排序
	return a[j].TianTi < a[i].TianTi
}

func InitTopList(){
	if len(TrueTopList) == 0{
		for i := 0; i < num; i++ {
			p := prpc.COM_TopUnit{}
			p.Name = Testpaiming + strconv.Itoa(i)
			if i / 2 == 0 {
				p.DisplayID = 1
			} else {
				p.DisplayID = 2
			}
			p.Level = num - int32(i)
			p.TianTi = tian - int32(i) * 2

			TrueTopList = append(TrueTopList, p)
		}
	}
	TMPTopList = TrueTopList

}

func isSame(t1 []prpc.COM_TopUnit, t2 []prpc.COM_TopUnit) bool {

	for idx, t := range t1 {
		if t == t2[idx]{
			continue
		}
		return false
	}

	return true
}

func RefreshAllTopList(){
	sort.Sort(TopList(TMPTopList))		// 重新排名

	tmp := TrueTopList

	TrueTopList = TMPTopList[:num]

	if isSame(tmp, TrueTopList) {
		return
	}
	for _, p := range PlayerStore {
		if p == nil || p.session == nil {
			continue
		}
		p.TianTiRank = p.FindMyTianTiRank()
		p.session.RecvTopList(TrueTopList, p.TianTiRank)
	}
}

func RefreshFriendTopList(){
	sort.Sort(TopList(TMPTopList))		// 重新排名

	tmp := TrueTopList

	TrueTopList = TMPTopList[:num]

	if isSame(tmp, TrueTopList) {
		return
	}
	for _, p := range PlayerStore {
		if p == nil || p.session == nil {
			continue
		}
		p.TianTiRank = p.FindMyTianTiRank()
		p.session.RecvFriendTopList(TrueTopList, p.TianTiRank)
	}
}


func (this *GamePlayer) AllTopByPage()  {

	this.TianTiRank = this.FindMyTianTiRank()

	this.session.RecvTopList(TrueTopList, this.TianTiRank)

	return
}


func (this *GamePlayer) FriendTopByPage(page int32) {

	this.TianTiRank = this.FindMyTianTiRank()

	this.session.RecvTopList(TrueTopList, this.TianTiRank)
}


func (this *GamePlayer) FindMyTianTiRank() int32 {
	for i, t := range TrueTopList {
		if t.Name == this.MyUnit.InstName {
			return int32(i)
		}
	}

	return -1
}

func (this *GamePlayer) UpdateTianTiVal() {		//只更新不操作

	if this.TianTiRank == -1 {		// 无排名

		idx := -1

		for i, t := range TMPTopList{
			if t.Name == this.MyUnit.InstName {
				idx = i
				break
			}
		}
		log.Println("UpdateTianTiVal -100, ", idx)
		if idx == -1 {
			top := prpc.COM_TopUnit{}
			top.TianTi = this.TianTiVal
			top.Level = this.MyUnit.Level
			top.Name = this.MyUnit.InstName
			top.DisplayID = this.MyUnit.UnitId

			TMPTopList = append(TMPTopList, top)
		} else {
			TMPTopList[idx].TianTi = this.TianTiVal
		}
		log.Println("UpdateTianTiVal -1, ", TMPTopList)
	} else {
		my_top := TMPTopList[this.TianTiRank]

		my_top.TianTi = this.TianTiVal

		TMPTopList[this.TianTiRank] = my_top
	}
}