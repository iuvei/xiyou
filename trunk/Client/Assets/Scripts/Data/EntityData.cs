﻿using UnityEngine;
using System;
using System.Collections.Generic;

public class EntityData {

    public int _UnitId;
    public int _DisplayId;
    public int _Cost;
    public int _Type;
    public string _Name;
    public int[] _Skills;
    public string _Desc;
	public int CPT_HP;
	public int CPT_ATK;
	public int CPT_DEF;
	public int CPT_MAGIC_ATK;
	public int CPT_MAGIC_DEF;
	public int CPT_AGILE;
	public int CPT_KILL;
	public int CPT_CRIT;
	public int CPT_COUNTER_ATTACK;
	public int CPT_SPUTTERING;
	public int CPT_DOUBLE_HIT;
	public int CPT_RECOVERY;
	public int CPT_REFLEX;
	public int CPT_SUCK_BLOOD;
	public int CPT_INCANTER;
	public int CPT_RESISTANCE;
	public int IPT_LEVEL;


    static Dictionary<int, EntityData> metaData;

    static public void ParseData(string content, string fileName)
    {
        metaData = new Dictionary<int, EntityData> ();

        CSVParser parser = new CSVParser ();
        if(!parser.Parse (content))
        {
            Debug.LogError("EntityData 解析错误");
            return;
        }

        int recordCounter = parser.GetRecordCounter();
        EntityData data = null;
        for(int i=0; i < recordCounter; ++i)
        {
            data = new EntityData ();
            data._UnitId = parser.GetInt (i, "UnitId");
            data._DisplayId = parser.GetInt(i, "DisplayId");
            data._Cost = parser.GetInt(i, "Cost");
            data._Type = parser.GetInt(i, "type");
            data._Name = parser.GetString(i, "Name");
            data._Desc = parser.GetString(i, "Desc");

			data.IPT_LEVEL = parser.GetInt(i, "IPT_LEVEL");
			data.CPT_HP = parser.GetInt(i, "CPT_HP");
			data.CPT_ATK = parser.GetInt(i, "CPT_ATK");
			data.CPT_DEF = parser.GetInt(i, "CPT_DEF");
			data.CPT_MAGIC_ATK = parser.GetInt(i, "CPT_MAGIC_ATK");
			data.CPT_MAGIC_DEF = parser.GetInt(i, "CPT_MAGIC_DEF");
			data.CPT_AGILE = parser.GetInt(i, "CPT_AGILE");
			data.CPT_KILL = parser.GetInt(i, "CPT_KILL");
			data.CPT_CRIT = parser.GetInt(i, "CPT_CRIT");
			data.CPT_COUNTER_ATTACK = parser.GetInt(i, "CPT_COUNTER_ATTACK");
			data.CPT_SPUTTERING = parser.GetInt(i, "CPT_SPUTTERING");
			data.CPT_DOUBLE_HIT = parser.GetInt(i, "CPT_DOUBLE_HIT");
			data.CPT_RECOVERY = parser.GetInt(i, "CPT_RECOVERY");
			data.CPT_REFLEX = parser.GetInt(i, "CPT_REFLEX");
			data.CPT_SUCK_BLOOD = parser.GetInt(i, "CPT_SUCK_BLOOD");
			data.CPT_INCANTER = parser.GetInt(i, "CPT_INCANTER");
			data.CPT_RESISTANCE = parser.GetInt(i, "CPT_RESISTANCE");

            data._Skills = new int[4];
            for(int j=0; j < 4; ++j)
            {
                data._Skills[j] = parser.GetInt(i, "Skill" + (j + 1).ToString());
            }

            if(metaData.ContainsKey(data._UnitId))
            {
                Debug.LogError("EntityData ID重复");
                return;
            }
            metaData[data._UnitId] = data;
        }
        parser.Dispose ();
        parser = null;
    }

    static public EntityData GetData(int id)
    {
        if (!metaData.ContainsKey(id))
            return null;

        return metaData[id];
    }
}
