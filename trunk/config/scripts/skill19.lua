sys.log(" skill 19 start")

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
-- 黄牙老象1号技能 为所有友方单位回复相当于法术强度50%的生命值。

-- 物理强度视作buff Battle.buff

function SK_118_Action(battleid, casterid)
	local skillid = 118	-- 技能id

	local  num = 2  --攻击个数

	local  p = Player.GetTargets(battleid,casterid,num)  --获取目标
	
	local  _property = Player.GetUnitProperty(battleid,casterid,"CPT_ATK")  --获取攻击者属性
	
	for i,v in ipairs(p) do
		
		--local  del_buff = Battle.AddBuff(1)  --敌对方物理强度
		
		local defender_def = Player.GetUnitProperty(battleid, v, "CPT_DEF")
	
		--local  demage  = del_buff-defender_def  --伤害 公式（物理伤害 减 防御 ）
		
		local  damage  = 8--测试
		
		--判断伤害
		if damage <= 0 then 
		
			damage = 1
		
		end
		local crit = Battle.GetCrit(skillid)   --是否暴击
		
		Battle.Attack(battleid,casterid,v,damage,crit)   --调用服务器 （伤害）(战斗者，释放者，承受者，伤害，暴击）
	
		sys.log("skill19 对id为"..v.."的目标减少"..recovery.."点伤害")
	end
	
	return  true
	 
end

sys.log( "skill 19 end")