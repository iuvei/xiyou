require "FairyGUI"

BattlePanel = fgui.window_class(WindowBase)
local Window;

local autoBtn;
local stateIcon;

local cards = {};

function BattlePanel:OnEntry()
	Window = BattlePanel.New();
	Window:Show();
end

function BattlePanel:OnInit()
	UIPackage.AddPackage("UI/UI_Fairy/export/Battle");
	self.contentPane = UIPackage.CreateObject("Battle", "BattlePanel").asCom;
	self:Center();

	local returnBtn = self.contentPane:GetChild("n8").asButton;
	returnBtn.onClick:Add(OnReturnBtn);

	autoBtn = self.contentPane:GetChild("n12").asButton;
	autoBtn.onClick:Add(OnAutoBtn);

	stateIcon = self.contentPane:GetChild("n16").asLoader;
	stateIcon.onClick:Add(OnTurnOver);

	for i=1, 5 do
		cards[i] = {};
		cards[i]["card"] = self.contentPane:GetChild("n" .. (16 + i)).asCom;
		cards[i]["power"] = cards[i]["card"]:GetChild("power");
		cards[i]["cost"] = cards[i]["card"]:GetChild("cost");
	end

	FlushData();
end

function BattlePanel:OnUpdate()
	if UIManager.IsDirty("BattlePanel") then
		FlushData();
		UIManager.ClearDirty("BattlePanel");
	end
end

function BattlePanel:OnTick()
	
end

function BattlePanel:isShow()
	return Window.isShowing;
end

function BattlePanel:OnDispose()
	Window:Dispose();
end

function FlushData()
	if Battle._CurrentState == Battle.BattleState.BS_Oper then
		stateIcon.url = UIPackage.GetItemURL("Battle", "battle_jieshuhuihe");
		stateIcon.touchable = true;
	else
		stateIcon.url = UIPackage.GetItemURL("Battle", "battle_dengdaizhong");
		stateIcon.touchable = false;
	end

	local cardNum = Battle._LeftCardNum;
	for i=1, 5 do
		if i <= cardNum then
			cards[i]["card"].data = i;
			cards[i]["card"].onClick:Add(OnCardClick);
			cards[i]["power"].text = i;
			cards[i]["cost"].text = i;
			cards[i]["card"].visible = true;
			if Battle._Turn == 1 and not Battle.IsSelfCard(i-1) then
				cards[i]["card"].enabled = false;
			else
				cards[i]["card"].enabled = true;
			end
		else
			cards[i]["card"].visible = false;
		end
	end
end

function OnCardClick(context)
	print(context.sender.data);
	Proxy4Lua.SelectCard4Ready(context.sender.data - 1);
end

function OnReturnBtn()
	print("OnReturnBtn");
	SceneLoader.LoadScene("main");
end

function OnTurnOver()
	print("OnTurnOver");
	Proxy4Lua.BattleSetup();
end

function OnAutoBtn()
	print("OnAutoBtn");
	GamePlayer._IsAuto = not GamePlayer._IsAuto;
	if GamePlayer._IsAuto then
		autoBtn:GetController("icon").selectedIndex = 1;
	else
		autoBtn:GetController("icon").selectedIndex = 0;
	end
end