package prpc
import(
  "bytes"
  "sync"
  "encoding/json"
  "suzuki/prpc"
)
type COM_Player struct{
  sync.Mutex
  InstId int64  //0
  Name string  //1
  Unit COM_Unit  //2
  Employees []COM_Unit  //3
  Chapters []COM_Chapter  //4
  UnitGroup []COM_UnitGroup  //5
  TianTiVal int32  //6
  SkillBase []COM_SkillBase  //7
}
func (this *COM_Player)SetInstId(value int64) {
  this.Lock()
  defer this.Unlock()
  this.InstId = value
}
func (this *COM_Player)GetInstId() int64 {
  this.Lock()
  defer this.Unlock()
  return this.InstId
}
func (this *COM_Player)SetName(value string) {
  this.Lock()
  defer this.Unlock()
  this.Name = value
}
func (this *COM_Player)GetName() string {
  this.Lock()
  defer this.Unlock()
  return this.Name
}
func (this *COM_Player)SetUnit(value COM_Unit) {
  this.Lock()
  defer this.Unlock()
  this.Unit = value
}
func (this *COM_Player)GetUnit() COM_Unit {
  this.Lock()
  defer this.Unlock()
  return this.Unit
}
func (this *COM_Player)SetEmployees(value []COM_Unit) {
  this.Lock()
  defer this.Unlock()
  this.Employees = value
}
func (this *COM_Player)GetEmployees() []COM_Unit {
  this.Lock()
  defer this.Unlock()
  return this.Employees
}
func (this *COM_Player)SetChapters(value []COM_Chapter) {
  this.Lock()
  defer this.Unlock()
  this.Chapters = value
}
func (this *COM_Player)GetChapters() []COM_Chapter {
  this.Lock()
  defer this.Unlock()
  return this.Chapters
}
func (this *COM_Player)SetUnitGroup(value []COM_UnitGroup) {
  this.Lock()
  defer this.Unlock()
  this.UnitGroup = value
}
func (this *COM_Player)GetUnitGroup() []COM_UnitGroup {
  this.Lock()
  defer this.Unlock()
  return this.UnitGroup
}
func (this *COM_Player)SetTianTiVal(value int32) {
  this.Lock()
  defer this.Unlock()
  this.TianTiVal = value
}
func (this *COM_Player)GetTianTiVal() int32 {
  this.Lock()
  defer this.Unlock()
  return this.TianTiVal
}
func (this *COM_Player)SetSkillBase(value []COM_SkillBase) {
  this.Lock()
  defer this.Unlock()
  this.SkillBase = value
}
func (this *COM_Player)GetSkillBase() []COM_SkillBase {
  this.Lock()
  defer this.Unlock()
  return this.SkillBase
}
func (this *COM_Player)Serialize(buffer *bytes.Buffer) error {
  this.Lock()
  defer this.Unlock()
  //field mask
  mask := prpc.NewMask1(1)
  mask.WriteBit(this.InstId!=0)
  mask.WriteBit(len(this.Name) != 0)
  mask.WriteBit(true) //Unit
  mask.WriteBit(len(this.Employees) != 0)
  mask.WriteBit(len(this.Chapters) != 0)
  mask.WriteBit(len(this.UnitGroup) != 0)
  mask.WriteBit(this.TianTiVal!=0)
  mask.WriteBit(len(this.SkillBase) != 0)
  {
    err := prpc.Write(buffer,mask.Bytes())
    if err != nil {
      return err
    }
  }
  // serialize InstId
  {
    if(this.InstId!=0){
      err := prpc.Write(buffer,this.InstId)
      if err != nil{
        return err
      }
    }
  }
  // serialize Name
  if len(this.Name) != 0{
    err := prpc.Write(buffer,this.Name)
    if err != nil {
      return err
    }
  }
  // serialize Unit
  {
    err := this.Unit.Serialize(buffer)
    if err != nil{
      return err
    }
  }
  // serialize Employees
  if len(this.Employees) != 0{
    {
      err := prpc.Write(buffer,uint(len(this.Employees)))
      if err != nil {
        return err
      }
    }
    for _, value := range this.Employees {
      err := value.Serialize(buffer)
      if err != nil {
        return err
      }
    }
  }
  // serialize Chapters
  if len(this.Chapters) != 0{
    {
      err := prpc.Write(buffer,uint(len(this.Chapters)))
      if err != nil {
        return err
      }
    }
    for _, value := range this.Chapters {
      err := value.Serialize(buffer)
      if err != nil {
        return err
      }
    }
  }
  // serialize UnitGroup
  if len(this.UnitGroup) != 0{
    {
      err := prpc.Write(buffer,uint(len(this.UnitGroup)))
      if err != nil {
        return err
      }
    }
    for _, value := range this.UnitGroup {
      err := value.Serialize(buffer)
      if err != nil {
        return err
      }
    }
  }
  // serialize TianTiVal
  {
    if(this.TianTiVal!=0){
      err := prpc.Write(buffer,this.TianTiVal)
      if err != nil{
        return err
      }
    }
  }
  // serialize SkillBase
  if len(this.SkillBase) != 0{
    {
      err := prpc.Write(buffer,uint(len(this.SkillBase)))
      if err != nil {
        return err
      }
    }
    for _, value := range this.SkillBase {
      err := value.Serialize(buffer)
      if err != nil {
        return err
      }
    }
  }
  return nil
}
func (this *COM_Player)Deserialize(buffer *bytes.Buffer) error{
  this.Lock()
  defer this.Unlock()
  //field mask
  mask, err:= prpc.NewMask0(buffer,1);
  if err != nil{
    return err
  }
  // deserialize InstId
  if mask.ReadBit() {
    err := prpc.Read(buffer,&this.InstId)
    if err != nil{
      return err
    }
  }
  // deserialize Name
  if mask.ReadBit() {
    err := prpc.Read(buffer,&this.Name)
    if err != nil{
      return err
    }
  }
  // deserialize Unit
  if mask.ReadBit() {
    err := this.Unit.Deserialize(buffer)
    if err != nil{
      return err
    }
  }
  // deserialize Employees
  if mask.ReadBit() {
    var size uint
    err := prpc.Read(buffer,&size)
    if err != nil{
      return err
    }
    this.Employees = make([]COM_Unit,size)
    for i,_ := range this.Employees{
      err := this.Employees[i].Deserialize(buffer)
      if err != nil{
        return err
      }
    }
  }
  // deserialize Chapters
  if mask.ReadBit() {
    var size uint
    err := prpc.Read(buffer,&size)
    if err != nil{
      return err
    }
    this.Chapters = make([]COM_Chapter,size)
    for i,_ := range this.Chapters{
      err := this.Chapters[i].Deserialize(buffer)
      if err != nil{
        return err
      }
    }
  }
  // deserialize UnitGroup
  if mask.ReadBit() {
    var size uint
    err := prpc.Read(buffer,&size)
    if err != nil{
      return err
    }
    this.UnitGroup = make([]COM_UnitGroup,size)
    for i,_ := range this.UnitGroup{
      err := this.UnitGroup[i].Deserialize(buffer)
      if err != nil{
        return err
      }
    }
  }
  // deserialize TianTiVal
  if mask.ReadBit() {
    err := prpc.Read(buffer,&this.TianTiVal)
    if err != nil{
      return err
    }
  }
  // deserialize SkillBase
  if mask.ReadBit() {
    var size uint
    err := prpc.Read(buffer,&size)
    if err != nil{
      return err
    }
    this.SkillBase = make([]COM_SkillBase,size)
    for i,_ := range this.SkillBase{
      err := this.SkillBase[i].Deserialize(buffer)
      if err != nil{
        return err
      }
    }
  }
  return nil
}
func (this *COM_Player)String() string{
  b, e := json.Marshal(this)
  if e != nil{
    return e.Error()
  }else{
    return string(b)
  }
}
