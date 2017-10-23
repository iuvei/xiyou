package prpc
import(
  "bytes"
  "sync"
  "encoding/json"
)
type COM_UnitSkill struct{
  sync.Mutex
  Pos int32  //0
  SkillId int32  //1
}
func (this *COM_UnitSkill)SetPos(value int32) {
  this.Lock()
  defer this.Unlock()
  this.Pos = value
}
func (this *COM_UnitSkill)GetPos() int32 {
  this.Lock()
  defer this.Unlock()
  return this.Pos
}
func (this *COM_UnitSkill)SetSkillId(value int32) {
  this.Lock()
  defer this.Unlock()
  this.SkillId = value
}
func (this *COM_UnitSkill)GetSkillId() int32 {
  this.Lock()
  defer this.Unlock()
  return this.SkillId
}
func (this *COM_UnitSkill)Serialize(buffer *bytes.Buffer) error {
  this.Lock()
  defer this.Unlock()
  //field mask
  mask := NewMask1(1)
  mask.WriteBit(this.Pos!=0)
  mask.WriteBit(this.SkillId!=0)
  {
    err := Write(buffer,mask.Bytes())
    if err != nil {
      return err
    }
  }
  // serialize Pos
  {
    if(this.Pos!=0){
      err := Write(buffer,this.Pos)
      if err != nil{
        return err
      }
    }
  }
  // serialize SkillId
  {
    if(this.SkillId!=0){
      err := Write(buffer,this.SkillId)
      if err != nil{
        return err
      }
    }
  }
  return nil
}
func (this *COM_UnitSkill)Deserialize(buffer *bytes.Buffer) error{
  this.Lock()
  defer this.Unlock()
  //field mask
  mask, err:= NewMask0(buffer,1);
  if err != nil{
    return err
  }
  // deserialize Pos
  if mask.ReadBit() {
    err := Read(buffer,&this.Pos)
    if err != nil{
      return err
    }
  }
  // deserialize SkillId
  if mask.ReadBit() {
    err := Read(buffer,&this.SkillId)
    if err != nil{
      return err
    }
  }
  return nil
}
func (this *COM_UnitSkill)String() string{
  b, e := json.Marshal(this)
  if e != nil{
    return e.Error()
  }else{
    return string(b)
  }
}
