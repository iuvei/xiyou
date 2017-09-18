
--主入口函数。从这里开始lua逻辑
function Main()					
	 		
end

--场景切换通知
function OnLevelWasLoaded(level)
	collectgarbage("collect")
	Time.timeSinceLevelLoad = 0
end

function ExcuteNpc(id)
	if id == 2 then
		Proxy4Lua.FocusNpcObject(2);
		return;
	end

	if id == 11 then
		UIManager.Show("jiehun");
		return;
	end

	if id == 12 then
		UIManager.Show("qiecuo");
		return;
	end

	local MessageBox = UIManager.ShowMessageBox();
	MessageBox:SetData("提示", "暂未开放,敬请期待", true);
end

function NetWorkException(errCode)
	local MessageBox = UIManager.ShowMessageBox();
	if errCode == 10061 then
		MessageBox:SetData("提示", "服务器连接不上，请检查网络或服务器状态", true);
	else
		MessageBox:SetData("提示", "网络连接已断开", true);
	end
end

function NetWorkReconnect()
	Proxy4Lua.ReconnectServer();
	UIManager.HideMessageBox();
end

function ErrorMessage(errCode)
	
end

function RegGlobalValue()
	Define.Set("DebugServerAddress", "127.0.0.1");
	Define.Set("DebugServerPort", 10999);
	Define.Set("UIModelScale", 200);
	Define.Set("MaxFee", 5);
	Define.Set("MoveSpeed_InBattle", 8);
	Define.Set("MoveSpeed_InWorld", 2.5);
	Define.Set("PointLight", "Effect/chushengdian");
	Define.Set("DestLight", "Effect/dianjiguangquan");
	Define.Set("BornPos", "8.55,-14.34,4");
	Define.Set("BattleCamera_plus", "5,5,5"); 		--偏移坐标
	Define.Set("WorldCamera_focusPlus", "0,1,9");	--主场景公告牌偏移坐标
	Define.Set("WeatherCheckTime", 60); --天气监测间隔 秒
end

function RegUIResMap()
	UIManager.RegUIResMap("playerInfoPanel", "xiangxiziliao");
end