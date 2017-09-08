﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;

public class Skill {

    public bool _IsCasting;
    public bool _IsCasted;

    // 技能静态数据
    public SkillData _SkillData;

    // 技能释放者
    public Actor _Caster;

    // 技能目标，可能有多个
    public Actor[] _Targets;

    // 释放者的原始位置(有位移时回归位置用)
    Vector3 _OriginPos;

    // 该单元战报(取需要的数据用)
    COM_BattleActionTarget[] _Actions;

    // 技能释放后自身增删的buff
    COM_BattleBuff[] _SkillBuff;

    // 释放特效缓存
    GameObject _CastEff;

    // 技能特效缓存
    GameObject[] _SkillEff;

    // 受击特效缓存
    GameObject[] _BeattackEff;

    int crtTargetIdx;

    bool IsSec;

    LuaState _Lua;
    LuaFunction _InitFunc;
    LuaFunction _CastFunc;

    public Skill(int skillId, Actor caster, Actor[] targets, COM_BattleActionTarget[] actionTargets, COM_BattleBuff[] skillBuffs)
    {
        _IsCasting = true;
        // get skilldata by id

        _SkillData = SkillData.GetData(skillId);
        if (_SkillData == null)
        {
            Clear();
            return;
        }

        if (caster == null)
        {
            Clear();
            return;
        }

        // 根据技能类型初始化特效
        if (!string.IsNullOrEmpty(_SkillData._CastEffect))
        {
            _CastEff = AssetLoader.LoadAsset(_SkillData._CastEffect);
            if (_CastEff != null)
            {
                _CastEff.transform.parent = caster._ActorObj.transform;
                _CastEff.transform.localPosition = Vector3.zero;
                _CastEff.transform.localScale = Vector3.one;
                _CastEff.SetActive(false);
            }
        }

        if (_SkillData._Single)
        {
            _SkillEff = new GameObject[1];
            _SkillEff [0] = AssetLoader.LoadAsset(_SkillData._SkillEffect);
            if (_SkillEff [0] != null)
            {
                if (_SkillData._Motion == SkillData.MotionType.MT_Target)
                {
                    _SkillEff [0].transform.parent = targets[0]._ActorObj.transform;
                }
                else
                {
                    _SkillEff [0].transform.parent = caster._ActorObj.transform;
                }
                _SkillEff [0].transform.localPosition = Vector3.zero;
                _SkillEff [0].transform.localScale = Vector3.one;
                _SkillEff [0].SetActive(false);
            }
        }
        else
        {
            _SkillEff = new GameObject[targets.Length];
            if (_SkillData._Motion == SkillData.MotionType.MT_Target)
            {
                for (int i = 0; i < targets.Length; ++i)
                {
                    _SkillEff [i] = AssetLoader.LoadAsset(_SkillData._SkillEffect);
                    if (_SkillEff [i] != null)
                    {
                        _SkillEff [i].transform.parent = targets [i]._ActorObj.transform;
                        _SkillEff [i].transform.localPosition = Vector3.zero;
                        _SkillEff [i].transform.localScale = Vector3.one;
                        _SkillEff [i].SetActive(false);
                    }
                }
            }
            else if(_SkillData._Motion == SkillData.MotionType.MT_Self)
            {
                _SkillEff = new GameObject[1];
                _SkillEff[0] = AssetLoader.LoadAsset(_SkillData._SkillEffect);
                _SkillEff[0].transform.parent = caster._ActorObj.transform;
                _SkillEff[0].transform.localPosition = Vector3.zero;
                _SkillEff[0].transform.localScale = Vector3.one;
                _SkillEff[0].transform.rotation = caster._ActorObj.transform.rotation;
                _SkillEff[0].SetActive(false);
            }
            else if(_SkillData._Motion == SkillData.MotionType.MT_Fly)
            {
                for (int i = 0; i < targets.Length; ++i)
                {
                    _SkillEff [i] = AssetLoader.LoadAsset(_SkillData._SkillEffect);
                    if (_SkillEff [i] != null)
                    {
                        _SkillEff [i].transform.position = caster._ActorObj.transform.position;
                        _SkillEff [i].SetActive(false);
                    }
                }
            }
        }

        _BeattackEff = new GameObject[targets.Length];
        for (int i = 0; i < targets.Length; ++i)
        {
            _BeattackEff[i] = AssetLoader.LoadAsset(_SkillData._BeattackEffect);
            if (_BeattackEff [i] != null)
            {
                if (targets [i] == null)
                    continue;
                
                _BeattackEff[i].transform.parent = targets[i]._ActorObj.transform;
                _BeattackEff[i].transform.localPosition = Vector3.zero;
                _BeattackEff[i].transform.localScale = Vector3.one;
                _BeattackEff[i].SetActive(false);
            }
        }
        _OriginPos = caster._ActorObj.transform.position;
        _Caster = caster;
        _Targets = targets;
        _Actions = actionTargets;
        _SkillBuff = skillBuffs;

        IsSec = _SkillData._Motion == SkillData.MotionType.MT_Sec;

//        _Lua = new LuaState();
//        LuaBinder.Bind(_Lua);
//        _Lua.Start();
//        _Lua.DoFile("skill_client.lua");
//
//        _InitFunc = _Lua.GetFunction("Init");
//        _CastFunc = _Lua.GetFunction("Cast");
//
//        if (_InitFunc != null)
//            _InitFunc.Call(this);
//
//        _Lua.CheckTop();
    }

    public bool Cast()
    {
//        if (_CastFunc != null)
//            _CastFunc.Call();
//
//        _Lua.CheckTop();

        if (_Caster == null)
        {
            Clear();
            return false;
        }

        if (_Targets == null || _Targets.Length == 0)
        {
            Clear();
            return false;
        }

        if (_SkillData._IsMelee)
            Melee();
        else
            Range();
        return true;
    }

    void Melee()
    {
        Play(_Caster, _SkillData._CastAnim);

        CastEffect();

        OnTimeDo(_SkillData._CastTime, Melee_BeforeMove);
        crtTargetIdx = 0;
    }

    void Melee_BeforeMove()
    {
        if (crtTargetIdx < 0 || crtTargetIdx >= _Targets.Length)
        {
            Melee_End();
            return;
        }

        Play(_Caster, Define.ANIMATION_PLAYER_ACTION_RUN);

        MoveTo(_Caster, _Targets[crtTargetIdx].Forward(_Caster.ForwardAjax), Melee_Moved);
    }

    void Melee_Moved()
    {
        _Caster._ActorObj.transform.LookAt(_Targets[crtTargetIdx]._ActorObj.transform);

        Stop(_Caster);

        Play(_Caster, _SkillData._AttackAnim);

        if (IsSec)
        {
            if(_Caster._RealPosInScene >= 0 && _Caster._RealPosInScene < 6)
                Battle._BattleCamera.Feature(_Targets[crtTargetIdx]._ActorObj, _SkillData._Camera);
            SkillEffect(crtTargetIdx);
            BeattackEffect(crtTargetIdx);
            EmitNum(crtTargetIdx);
        }
        else
        {
            if(_Caster._RealPosInScene >= 0 && _Caster._RealPosInScene < 6)
                Battle._BattleCamera.Feature(_Targets[0]._ActorObj, _SkillData._Camera);
            SkillEffect();
            BeattackEffect();
            EmitNum();
        }

        float attackTime = GetClipLength(_Caster, _SkillData._AttackAnim);
        OnTimeDo(attackTime, Melee_EndMove);
    }

    void Melee_EndMove()
    {
        if (IsSec)
        {
            crtTargetIdx = crtTargetIdx + 1;
            Melee_BeforeMove();
        }
        else
            Melee_End();
    }

    void Melee_End()
    {
        if(_Caster._RealPosInScene >= 0 && _Caster._RealPosInScene < 6)
            Battle._BattleCamera.Reset();
        Play(_Caster, Define.ANIMATION_PLAYER_ACTION_RUN);
        CheckBuffGoBackToOrigin();
    }

    void Range()
    {
        if(_Caster._RealPosInScene >= 0 && _Caster._RealPosInScene < 6)
            Battle._BattleCamera.Feature(_Caster._ActorObj, _SkillData._Camera);
        Play(_Caster, _SkillData._CastAnim);
        PlayQueue(_Caster, Define.ANIMATION_PLAYER_ACTION_IDLE);
        CastEffect();
        OnTimeDo(_SkillData._CastTime, Range_BeforeCast);
        crtTargetIdx = 0;
    }

    void Range_BeforeCast()
    {
        if (IsSec)
        {
            if (crtTargetIdx < 0 || crtTargetIdx > _Targets.Length)
            {
                Range_End();
                return;
            }
            SkillEffect(crtTargetIdx);
            BeattackEffect(crtTargetIdx);
            EmitNum(crtTargetIdx);
            HandleTrack(crtTargetIdx);
        }
        else
        {
            SkillEffect();
            BeattackEffect();
            EmitNum();
        }
        if (_SkillData._Motion == SkillData.MotionType.MT_Fly)
        {
            HandleTrack();
        }
        OnTimeDo(_SkillData._TotalTime, Range_EndCast);
        if(_Caster._RealPosInScene >= 0 && _Caster._RealPosInScene < 6)
            Battle._BattleCamera.Reset();
    }

    void Range_EndCast()
    {
        if (IsSec)
        {
            crtTargetIdx = crtTargetIdx + 1;
            OnTimeDo(_SkillData._CastTime, Range_BeforeCast);
        }
        else
        {
            Range_End();
        }
    }

    void Range_End()
    {
        Play(_Caster, Define.ANIMATION_PLAYER_ACTION_IDLE);
        CheckBuffGoBackToOrigin();
    }

    void HandleBuff()
    {
        Actor target = null;
        for(int i=0; i < _Actions.Length; ++i)
        {
            target = Battle.GetActor(_Actions[i].InstId);
            if (target != null)
            {
                if (_Actions [i].BuffAdd == null)
                {
                    Debug.LogWarning(" _Actions [i].BuffAdd is null!");
                }
                else
                {
                    for(int j=0; j < _Actions[i].BuffAdd.Length; ++j)
                    {
                        if (_Actions [i].BuffAdd [j].Change == 1)
                            target.AddBuff(_Actions [i].BuffAdd [j].BuffId);
                        else if(_Actions [i].BuffAdd [j].Change == 0)
                            target.RemoveBuff(_Actions [i].BuffAdd [j].BuffId);
                    }
                }

                if (_Actions [i].Dead)
                {
                    target.Play(Define.ANIMATION_PLAYER_ACTION_DEAD);
                    new Timer().Start(target.ClipLength(Define.ANIMATION_PLAYER_ACTION_DEAD) + 1f, (object actor) => {
                        Battle.DelActor(((Actor)actor)._RealPosInScene);
                    }, target);
                }
            }
        }

        if (_SkillBuff != null)
        {
            for(int i=0; i < _SkillBuff.Length; ++i)
            {
                if (_SkillBuff [i].Change == 1)
                    _Caster.AddBuff(_SkillBuff [i].BuffId);
                else if(_SkillBuff [i].Change == 0)
                    _Caster.RemoveBuff(_SkillBuff [i].BuffId);
            }
        }
    }

    public int TargetCount
    {
        get
        {
            if (_Targets == null)
                return 0;
            
            return _Targets.Length;
        }
    }

    public void MoveTo(Actor actor, Vector3 position, Actor.CallBackHandler callback)
    {
        if (actor != null)
            actor.MoveTo(position, callback);
    }

    public void Play(Actor actor, string anim)
    {
        if (actor != null)
            actor.Play(anim);
    }

    public void PlayQueue(Actor actor, string anim)
    {
        if (actor != null)
            actor.PlayQueue(anim);
    }

    public void Stop(Actor actor)
    {
        if (actor != null)
            actor.Stop();
    }

    public float GetClipLength(Actor actor, string anim)
    {
        if (actor != null)
            return actor.ClipLength(anim);
        return 0f;
    }

    public void CastEffect()
    {
        if (_CastEff != null)
        {
            _CastEff.SetActive(false);
            _CastEff.SetActive(true);
        }
    }

    public void SkillEffect(int idx)
    {
        if (idx < 0 || idx >= _SkillEff.Length)
            return;

        if (_SkillEff [idx] != null)
        {
            _SkillEff [idx].SetActive(false);
            _SkillEff [idx].SetActive(true);
        }
    }

    public void SkillEffect()
    {
        for (int i = 0; i < _SkillEff.Length; ++i)
        {
            if (_SkillEff [i] != null)
            {
                _SkillEff [i].SetActive(false);
                _SkillEff [i].SetActive(true);
            }
        }
    }

    public void BeattackEffect(int idx)
    {
        if (idx < 0 || idx >= _BeattackEff.Length)
            return;
        
        for (int i = 0; i < _SkillData._BeattackTime.Length; ++i)
        {
            new Timer().Start(new TimerParam(_SkillData._BeattackTime[i], delegate
            {
                if(idx < 0 || idx >= _Targets.Length)
                    return;

                if(_Actions[idx].ActionParam < 0)
                {
                    _Targets[idx].Play(Define.ANIMATION_PLAYER_ACTION_BEATTACK);
                    _Targets[idx].PlayQueue(Define.ANIMATION_PLAYER_ACTION_IDLE);
                }

                //beattack effect
                if (_BeattackEff[idx] != null)
                {
                    _BeattackEff[idx].SetActive(false);
                    _BeattackEff[idx].SetActive(true);
                }
            }));
        }
    }

    public void BeattackEffect()
    {
        for(int i=0; i < _SkillData._BeattackTime.Length; ++i)
        {
            new Timer().Start(new TimerParam(_SkillData._BeattackTime[i], delegate
            {
                for (int j = 0; j < _Targets.Length; ++j)
                {
                    if(_Actions[j].ActionParam < 0)
                    {
                        if(_Targets[j] == null)
                            continue;
                        
                        _Targets[j].Play(Define.ANIMATION_PLAYER_ACTION_BEATTACK);
                        _Targets[j].PlayQueue(Define.ANIMATION_PLAYER_ACTION_IDLE);
                    }

                    //beattack effect
                    if (_BeattackEff[j] != null)
                    {
                        _BeattackEff[j].SetActive(false);
                        _BeattackEff[j].SetActive(true);
                    }
                }
            }));
        }
    }

    public void EmitNum(int idx)
    {
        if (idx < 0 || idx >= _Actions.Length)
            return;
        
        for(int i=0; i < _SkillData._EmitNumTime.Length; ++i)
        {
            new Timer().Start(new TimerParam(_SkillData._EmitNumTime[i], (Timer.TimerCallBack1)delegate
            {
                if(idx < 0 || idx >= _Targets.Length)
                    return;
                
                if(_Targets[idx] == null)
                    return;

                if(_Actions[idx].ActionParam == 0)
                    return;

                _Targets[idx].UpdateValue(_Actions[idx].ActionParam, -1);
                int disValue = _Actions[idx].ActionParam / _SkillData._EmitNumTime.Length;
                if(disValue == 0)
                {
                    disValue = _Actions[idx].ActionParam > 0? 1: -1;
                }
                _Targets[idx].PopContent(disValue, _Actions[idx].ActionParamExt);
            }));
        }

        if(_SkillData._EmitNumTime.Length > 0)
        {
            new Timer().Start(new TimerParam(_SkillData._EmitNumTime[0], delegate
            {
                if(idx < 0 || idx >= _Targets.Length)
                    return;

                if(_Targets[idx] == null)
                    return;

                if(_Actions[idx].ActionParam == 0)
                    return;

                _Targets[idx].UpdateValue(_Actions[idx].ActionParam, -1);
            }));
        }
    }

    public void EmitNum()
    {
        for(int i=0; i < _SkillData._EmitNumTime.Length; ++i)
        {
            new Timer().Start(new TimerParam(_SkillData._EmitNumTime[i], (Timer.TimerCallBack1)delegate
            {
                for (int j = 0; j < _Targets.Length; ++j)
                {
                    if(_Targets[j] == null)
                        continue;

                    if(_Actions[j].ActionParam == 0)
                        continue;

                    int disValue = _Actions[j].ActionParam / _SkillData._EmitNumTime.Length;
                    if(disValue == 0)
                    {
                        disValue = _Actions[j].ActionParam > 0? 1: -1;
                    }
                    _Targets[j].PopContent(disValue, _Actions[j].ActionParamExt);
                }
            }));
        }

        if(_SkillData._EmitNumTime.Length > 0)
        {
            new Timer().Start(new TimerParam(_SkillData._EmitNumTime[0], delegate
            {
                for (int j = 0; j < _Targets.Length; ++j)
                {
                    if(_Targets[j] == null)
                        continue;

                    if(_Actions[j].ActionParam == 0)
                        continue;

                    _Targets[j].UpdateValue(_Actions[j].ActionParam, -1);
                }
            }));
        }
    }

    public void HandleTrack(int idx)
    {
        if (idx < 0 || idx >= _Targets.Length)
            return;
        for (int i = 0; i < _SkillEff.Length; ++i)
        {
            _SkillEff[i].transform.LookAt(_Targets[idx]._ActorObj.transform, Vector3.up);
            iTween.MoveTo(_SkillEff[i], iTween.Hash("time", _SkillData._BeattackTime[i], "position", _Targets[idx]._ActorObj.transform.position, "easetype", iTween.EaseType.linear));
        }
    }

    public void HandleTrack()
    {
        for (int i = 0; i < _SkillEff.Length; ++i)
        {
            _SkillEff[i].transform.LookAt(_Targets[i]._ActorObj.transform, Vector3.up);
            iTween.MoveTo(_SkillEff[i], iTween.Hash("time", _SkillData._BeattackTime[0], "position", _Targets[i]._ActorObj.transform.position, "easetype", iTween.EaseType.linear));
        }
    }

    public void OnTimeDo(float delay, Timer.TimerCallBack1 callback)
    {
        new Timer().Start(delay, callback);
    }

    public void CheckBuffGoBackToOrigin()
    {
        HandleBuff();
        if (_SkillData._IsMelee)
        {
            _Caster.MoveTo(_OriginPos, (Actor.CallBackHandler)delegate
            {
                Clear();
                _Caster.Stop();
                _Caster.Reset();
            });
        }
        else
        {
            Clear();
            _Caster.Stop();
            _Caster.Reset();
        }
    }

    public int GetMotionTypeInt
    {
        get
        {
            if (_SkillData == null)
                return 0;
            return (int)_SkillData._Motion;
        }
    }

    public void Clear()
    {
        _IsCasting = false;
        _IsCasted = true;

        if (_CastEff != null)
        {
            GameObject.Destroy(_CastEff);
            AssetLoader.UnloadAsset(_SkillData._CastEffect);
        }

        if (_SkillEff != null)
        {
            for(int i=0; i < _SkillEff.Length; ++i)
            {
                GameObject.Destroy(_SkillEff[i]);
                AssetLoader.UnloadAsset(_SkillData._SkillEffect);
            }
        }

        if (_BeattackEff != null)
        {
            for(int i=0; i < _BeattackEff.Length; ++i)
            {
                GameObject.Destroy(_BeattackEff[i]);
                AssetLoader.UnloadAsset(_SkillData._BeattackEffect);
            }
        }
    }
}
