-- buff测试用脚本 加特殊效果 如百分比减伤 眩晕等特殊效果
-- buff格式 buff_id_update, buff_id_delete
-- 参数暂定为 battleid targetid data
sys.log("buff164")

function buff_164_add(battleid, unitid, buffinstid,data) 
	Player.ChangeSpecial(battleid, unitid, buffinstid,"BF_WEAK")  --加增伤
	--Player.ChangeSpecial(battleid, unitid, buffinstid,"BF_STRONG")  --加输出伤害

	
	--sys.log("buff_110_add "..","..battleid..","..buffinstid..","..unitid)
	sys.log("buff_164_add  添加增伤buff"..",battleid是"..battleid..",buffid是"..buffinstid..",目标"..unitid..",数据是"..data)
end

function buff_164_update(battleid, buffinstid, unitid)	
	buff_id = 164 --配置表中的buffid
	
	-- Battle.BuffMintsHp(battleid, unitid, buffinstid)
	
	--sys.log("buff_110_update "..","..battleid..","..buffinstid..","..unitid)
	sys.log("buff_164_update  更新增伤buff"..",battleid是"..battleid..",buffid是"..buffinstid..",目标"..unitid)
	
end

function buff_164_delete(battleid, unitid, buffinstid,data)

	Player.PopSpec(battleid, unitid, buffinstid,"BF_WEAK")   --减增伤
	--Player.PopSpec(battleid, unitid, buffinstid,"BF_STRONG")   --减输出伤害
	
	--sys.log("buff_110_delete "..","..battleid..","..buffinstid..","..unitid)
	sys.log("buff_164_delete  删除增伤buff"..",battleid是"..battleid..",buffid是"..buffinstid..",目标"..unitid..",数据是"..data)

end

