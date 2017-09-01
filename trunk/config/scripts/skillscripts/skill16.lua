sys.log(" skill 16 start")

-- 技能释放 传入战斗ID和释放者的ID
-- 通过释放者和battleid取得对应的目标 单体或者多个
-- 循环/直接使用接口操控战斗 类似 战斗.攻击(战斗id, 释放者id, 承受者ID, 伤害数值, 是否暴击)
-- 
-- 
-- 所需接口
--	取得目标 （GetTarget（）  单   GetTargets（）  复）
--  取得对应属性 GetUnitProperty()
--  计算伤害数值 demage
--  计算是否暴击
--  攻击
-- 观音一号技能 对一个敌方目标造成法术强度的伤害，并降低目标40%的物理强度和法术强度，将其转移给一个友方目标。

-- 法术强度视作buff  Battle.buff

function SK_115_Action(battleid, casterid)
	Battle.TargetOn(battleid)

	local skillid = 115		-- 技能id
	
	local  t = Player.GetTarget(battleid,casterid)  --获取目标 
	
	local  caster_attack = Player.GetUnitAtk(battleid,casterid)  --获取攻击者属性  物理
	
	local  caster_magic = Player.GetUnitMtk(battleid,casterid)  --获取攻击者属性   法术
	
	local defender_def = Player.GetCalcMagicDef(battleid, t)  --获取防御属性
	
	
	local  damage  = caster_magic-defender_def    --伤害 公式（）
	
	--判断伤害
	if damage <= 0 then 
	
		damage = 1
	
	end
	
	local crit = Battle.GetCrit(skillid)   --是否暴击
	
	Battle.Attack(battleid,casterid,t,damage,crit)   --调用服务器   （伤害）(战斗者，释放者，承受者，伤害，暴击）
	
	Battle.AddBuff(battleid,casterid, t,6, -caster_magic*0.4)     --降低目标40%法术强度
	
	Battle.AddBuff(battleid,casterid, t, 3,-caster_attack*0.4)     --降低目标40%物理强度
	
	Battle.TargetOver(battleid)
	
	sys.log("skill16 对id为"..t.."的目标造成"..damage.."点伤害")
	
	return  true
	 
end

sys.log( "skill 16 end")