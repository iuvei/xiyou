package prpc
import(
  "bytes"
  "sync"
  "encoding/json"
)
type COM_UnitGroup struct{
  sync.Mutex
  GroupId int32  //0
  UnitList []int64  //1
}
func (this *COM_UnitGroup)SetGroupId(value int32) {
  this.Lock()
  defer this.Unlock()
  this.GroupId = value
}
func (this *COM_UnitGroup)GetGroupId() int32 {
  this.Lock()
  defer this.Unlock()
  return this.GroupId
}
func (this *COM_UnitGroup)SetUnitList(value []int64) {
  this.Lock()
  defer this.Unlock()
  this.UnitList = value
}
func (this *COM_UnitGroup)GetUnitList() []int64 {
  this.Lock()
  defer this.Unlock()
  return this.UnitList
}
func (this *COM_UnitGroup)Serialize(buffer *bytes.Buffer) error {
  this.Lock()
  defer this.Unlock()
  //field mask
  mask := NewMask1(1)
  mask.WriteBit(this.GroupId!=0)
  mask.WriteBit(len(this.UnitList) != 0)
  {
    err := Write(buffer,mask.Bytes())
    if err != nil {
      return err
    }
  }
  // serialize GroupId
  {
    if(this.GroupId!=0){
      err := Write(buffer,this.GroupId)
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
      err := Write(buffer,value)
      if err != nil {
        return err
      }
    }
  }
  return nil
}
func (this *COM_UnitGroup)Deserialize(buffer *bytes.Buffer) error{
  this.Lock()
  defer this.Unlock()
  //field mask
  mask, err:= NewMask0(buffer,1);
  if err != nil{
    return err
  }
  // deserialize GroupId
  if mask.ReadBit() {
    err := Read(buffer,&this.GroupId)
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
    this.UnitList = make([]int64,size)
    for i,_ := range this.UnitList{
      err := Read(buffer,&this.UnitList[i])
      if err != nil{
        return err
      }
    }
  }
  return nil
}
func (this *COM_UnitGroup)String() string{
  b, e := json.Marshal(this)
  if e != nil{
    return e.Error()
  }else{
    return string(b)
  }
}
