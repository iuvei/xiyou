﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ShopSystem
{
	public static int buyType; 	
	public static COM_ItemInst[] _buyItems;
	public static List<COM_ItemInst> _ShowBuyItems = new List<COM_ItemInst> ();
	public static COM_BlackMarket _BlackMarket;


	public static COM_ItemInst[] BuyItems
	{
		set
		{
			_buyItems = value;
			_ShowBuyItems.Clear ();
			for(int i=0;i<_buyItems.Length;i++)
			{
				_ShowBuyItems.Add(_buyItems[i]);
			}
		}
		get
		{ 
			return _buyItems;
		}
	}

	public static COM_BlackMarket BlackMarket
	{
		set
		{
			_BlackMarket = value;
			
		}
		get
		{ 
			return _BlackMarket;
		}
	}

	public static int GetBlackMarketId(int indx)
	{
		if (indx > _BlackMarket.ShopItems.Length)
			return 0;
		return _BlackMarket.ShopItems [indx];
	}

	public static int GetBlackRefreshNum()
	{
		return _BlackMarket.RefreshNum;
	}

	public static COM_ItemInst GetShowItem()
	{
		if (_ShowBuyItems.Count > 0) 
		{
			return _ShowBuyItems [0];
		}

		return null;
	}
		
	public static void DelShowItem()
	{
		if (_ShowBuyItems.Count > 0) 
		{
			_ShowBuyItems.Remove(_ShowBuyItems [0]);
		}
	}
}



