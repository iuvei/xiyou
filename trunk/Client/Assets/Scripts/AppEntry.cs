﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using FairyGUI;

public class AppEntry : MonoBehaviour {

    string context;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        GRoot.inst.SetContentScaleFactor(1920, 1080, UIContentScaler.ScreenMatchMode.MatchHeight);
        UIConfig.defaultFont = "方正楷体_GBK";

        Application.logMessageReceived += (condition, stackTrace, type) => {
            context += condition + "\n" + stackTrace + "\n" + type;
        };

        DataLoader.Init();
        UIManager.Init();
        Define.Init();
        CameraEffect.Init();
//
        //init network
        if (NetWoking.Open("127.0.0.1", 10999))
        {
            UIManager.Show("denglu");
            DataLoader.BeginLoad();
        }

        //UIObjectFactory.SetLoaderExtension(typeof(MyGLoader));
	}

	// Update is called once per frame
	void Update () {
        NetWoking.SetupNetFPS();
        TimerManager.Update();
        DataLoader.Update();
        UIManager.Update();
        World.Update();
        Battle.Update();
        SceneLoader.Update();
        CameraEffect.Update();
	}
}
