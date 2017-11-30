Global.setGlobalString("C_DefaultChatper", "1,2,3");			--//默认开启关卡
Global.setGlobalInt("C_BlackMarkteRefreshSpeed", 1);			--//黑市手动刷新花费
Global.setGlobalInt("C_BlackMarkteRefreshNum", 1);				--//黑市手动刷新次数

Global.setGlobalInt("C_CreatGuildLevel", 1);				--//创建帮派所需等级
Global.setGlobalInt("C_CreatGuildGold", 1);					--//创建帮派所需金币
Global.setGlobalInt("C_CreatGuildMemberMax", 50);			--//帮派成员和申请列表都为50
Global.setGlobalInt("C_VicePremierMaxNum", 2);				--/副帮主数量

--/////////////////////////////////////////////////////黄金分割线///////////////////////////////////////////////////////////////

--[[
// six columns mean：
//       second：0-59
//       minute：0-59
//       hour：1-23
//       day：1-31
//       month：1-12
//       week：0-6（0 means Sunday）

// SetCron some signals：
//       *： any time
//       ,：　 separate signal
//　　    －：duration
//       /n : do as n times of time duration
/////////////////////////////////////////////////////////
//	0/30 * * * * *                        every 30s
//	0 43 21 * * *                         21:43
//	0 15 05 * * * 　　                     05:15
//	0 0 17 * * *                          17:00
//	0 0 17 * * 1                           17:00 in every Monday
//	0 0,10 17 * * 0,2,3                   17:00 and 17:10 in every Sunday, Tuesday and Wednesday
//	0 0-10 17 1 * *                       17:00 to 17:10 in 1 min duration each time on the first day of month
//	0 0 0 1,15 * 1                        0:00 on the 1st day and 15th day of month
//	0 42 4 1 * * 　 　                     4:42 on the 1st day of month
//	0 0 21 * * 1-6　　                     21:00 from Monday to Saturday
//	0 0,10,20,30,40,50 * * * *　           every 10 min duration
//	0 */10 * * * * 　　　　　　            every 10 min duration
//	0 * 1 * * *　　　　　　　　               1:00 to 1:59 in 1 min duration each time
//	0 0 1 * * *　　　　　　　　               1:00
//	0 0 */1 * * *　　　　　　　               0 min of hour in 1 hour duration
//	0 0 * * * *　　　　　　　　               0 min of hour in 1 hour duration
//	0 2 8-20/3 * * *　　　　　　             8:02, 11:02, 14:02, 17:02, 20:02
//	0 30 5 1,15 * *　　　　　　              5:30 on the 1st day and 15th day of month
]]--
Global.setGlobalString("C_SaveDataToDB","0 */2 * * * *");
Global.setGlobalString("C_PassZeroHour","0 0 0 * * *");
Global.setGlobalString("C_BlackMarkteRefresh","0 0 0,6,12,18 * * *");
Global.setGlobalString("C_TopListRefresh","0 */5 * * * *");
Global.setGlobalString("C_EveryWeekRefresh","0 0 1 * * 1");
Global.setGlobalString("C_EveryMonthRefresh","0 */1 * * * *");			--//天梯赛季更新  "0 0 1 1 * *"


