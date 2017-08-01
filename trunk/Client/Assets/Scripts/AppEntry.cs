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
        CopyPastePatch.Apply();
        GRoot.inst.SetContentScaleFactor(1920, 1080);

        Application.logMessageReceived += (condition, stackTrace, type) => {
            context = condition + "\n" + stackTrace + "\n" + type;
        };

        DataLoader.Init();
        UIManager.Init();
//
        //init network
        if (NetWoking.Open("10.10.10.188", 10999))
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
        Battle.Update();
        SceneLoader.Update();
	}
}
