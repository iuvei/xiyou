#ifndef __LOGIC_H__
#define __LOGIC_H__

#include "config.h"
#include "prpc.h"
#include "prpc.gen.h"
#include "csv.h"


//#define BOOST_USE_WINAPI_VERSION WINVER


enum {
	SKILL_MAX = 4, //���������
	BATTLE_UNIT_MAX = 6, //ս�����������
	GAME_PLAYER_BATTLE_UNIT_MAX = 10
};


class Logic{
public:
	static void Init();
	static void Fini();
	static void Update();
private:
	static boost::shared_ptr<class Script> script_;
};


#endif