require "FairyGUI"

jinengxiangqing = fgui.window_class(WindowBase)
local Window;

local iconLoader;
local iconLv;
local skillType;
local skillName;
local skillDesc;
local skillCoolDown;

function jinengxiangqing:OnEntry()
	Window = jinengxiangqing.New();
	Window:Show();
end

function jinengxiangqing:GetWindow()
	return Window;
end

function jinengxiangqing:OnInit()
	self.contentPane = UIPackage.CreateObject("jinengxiangqing", "jineng_Component").asCom;
	self:Center();
	self.modal = true;
	self.closeButton = self.contentPane:GetChild("n13").asButton;

	local iconGroup = self.contentPane:GetChild("n14").asCom;
	iconLoader = iconGroup:GetChild("n8").asLoader;
	iconLv = iconGroup:GetChild("n7").asTextField;
	iconLv.visible =false;
	iconGroup:GetChild("n5").visible =false;
	skillType = self.contentPane:GetChild("n7").asTextField;
	skillName = self.contentPane:GetChild("n8").asTextField;
	skillDesc = self.contentPane:GetChild("n12").asTextField;
end

function jinengxiangqing:OnUpdate()
	if UIManager.IsDirty("jinengxiangqing") then
		jinengxiangqing_FlushData();
		UIManager.ClearDirty("jinengxiangqing");
	end
end

function jinengxiangqing:OnTick()
	
end

function jinengxiangqing:isShow()
	return Window.isShowing;
end

function jinengxiangqing:OnDispose()
	Window:Dispose();
end

function jinengxiangqing:OnHide()
	Window:Hide();
end

function jinengxiangqing_FlushData()
	local skillId = UIParamHolder.Get("jinengxiangqing");
	local sData = SkillData.GetData(skillId);
	if sData ~= nil then
		iconLoader.url = "ui://" .. sData._Icon;
		iconLv.text = sData._Level;
		if sData._Type == 0 then
			skillType.text = "被动";
		else
			skillType.text = "主动";
		end
		skillName.text = sData._Name;
		skillDesc.text = sData._Desc;
	end
end