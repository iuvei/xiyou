sys.log(" skill 9 start")

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
-- 哪吒3号技能 突刺敌方单体目标5次，每次造成20%物理强度的伤害，并减少目标10%的物理防御，持续1回合。

-- 物理强度视作buff Battle.buff

function SK_108_Action(battleid, casterid)
	local skillid = 108		-- 技能id

	local  p = Player.GetTarget(battleid,casterid)  --获取目标
	
	local  _property = Player.GetUnitProperty(battleid,casterid,"CPT_ATK")  --获取攻击者属性

	 -- add_buff = add_buff+add_buff*0.1
	
		--local  del_buff = Battle.AddBuff(1)  --敌对方物理强度
		
		local defender_def = Player.GetUnitProperty(battleid,p, "CPT_DEF")
	
		--local  damage  = del_buff*0.2*5-defender_def*0.1*5       --伤害 公式（20%的物理伤害   加减少10%的防御）
		
		local  damage  = 9 --测试
	
		--判断伤害
		if damage <= 0 then 
		
			damage = 1
		
		end
		local crit = Battle.GetCrit(skillid)   --是否暴击
		Battle.Attack(battleid,casterid,p,damage,crit)   --调用服务器 （伤害）(战斗者，释放者，承受者，伤害，暴击）
		sys.log("skill9 对id为"..p.."的目标造成"..damage.."点伤害")
	
	
	return  true
	 
end

sys.log( "skill 9 end")