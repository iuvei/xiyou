package prpc
import(
  "bytes"
  "sync"
  "encoding/json"
)
type COM_BattleReport struct{
  sync.Mutex
  BattleID int32  //0
  UnitList []COM_BattleUnit  //1
  ActionList []COM_BattleAction  //2
}
func (this *COM_BattleReport)SetBattleID(value int32) {
  this.Lock()
  defer this.Unlock()
  this.BattleID = value
}
func (this *COM_BattleReport)GetBattleID() int32 {
  this.Lock()
  defer this.Unlock()
  return this.BattleID
}
func (this *COM_BattleReport)SetUnitList(value []COM_BattleUnit) {
  this.Lock()
  defer this.Unlock()
  this.UnitList = value
}
func (this *COM_BattleReport)GetUnitList() []COM_BattleUnit {
  this.Lock()
  defer this.Unlock()
  return this.UnitList
}
func (this *COM_BattleReport)SetActionList(value []COM_BattleAction) {
  this.Lock()
  defer this.Unlock()
  this.ActionList = value
}
func (this *COM_BattleReport)GetActionList() []COM_BattleAction {
  this.Lock()
  defer this.Unlock()
  return this.ActionList
}
func (this *COM_BattleReport)Serialize(buffer *bytes.Buffer) error {
  this.Lock()
  defer this.Unlock()
  //field mask
  mask := NewMask1(1)
  mask.WriteBit(this.BattleID!=0)
  mask.WriteBit(len(this.UnitList) != 0)
  mask.WriteBit(len(this.ActionList) != 0)
  {
    err := Write(buffer,mask.Bytes())
    if err != nil {
      return err
    }
  }
  // serialize BattleID
  {
    if(this.BattleID!=0){
      err := Write(buffer,this.BattleID)
      if err != nil{
        return err
      }
    }
  }
  // serialize UnitList
  if len(this.UnitList) != 0{
    {
      err := Write(buffer,uint(len(this.UnitList)))
      if err != nil {
        return err
      }
    }
    for _, value := range this.UnitList {
      err := value.Serialize(buffer)
      if err != nil {
        return err
      }
    }
  }
  // serialize ActionList
  if len(this.ActionList) != 0{
    {
      err := Write(buffer,uint(len(this.ActionList)))
      if err != nil {
        return err
      }
    }
    for _, value := range this.ActionList {
      err := value.Serialize(buffer)
      if err != nil {
        return err
      }
    }
  }
  return nil
}
func (this *COM_BattleReport)Deserialize(buffer *bytes.Buffer) error{
  this.Lock()
  defer this.Unlock()
  //field mask
  mask, err:= NewMask0(buffer,1);
  if err != nil{
    return err
  }
  // deserialize BattleID
  if mask.ReadBit() {
    err := Read(buffer,&this.BattleID)
    if err != nil{
      return err
    }
  }
  // deserialize UnitList
  if mask.ReadBit() {
    var size uint
    err := Read(buffer,&size)
    if err != nil{
      return err
    }
    this.UnitList = make([]COM_BattleUnit,size)
    for i,_ := range this.UnitList{
      err := this.UnitList[i].Deserialize(buffer)
      if err != nil{
        return err
      }
    }
  }
  // deserialize ActionList
  if mask.ReadBit() {
    var size uint
    err := Read(buffer,&size)
    if err != nil{
      return err
    }
    this.ActionList = make([]COM_BattleAction,size)
    for i,_ := range this.ActionList{
      err := this.ActionList[i].Deserialize(buffer)
      if err != nil{
        return err
      }
    }
  }
  return nil
}
func (this *COM_BattleReport)String() string{
  b, e := json.Marshal(this)
  if e != nil{
    return e.Error()
  }else{
    return string(b)
  }
}
