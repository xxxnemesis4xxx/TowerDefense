﻿using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {

	public static LevelStuffFromXML ReadXMLFile()
	{
		LevelStuffFromXML ls = new LevelStuffFromXML();
		
		//We are directly loading the level1 file, change if appropriate
		TextAsset ta = Resources.Load("Level1") as TextAsset;
		
		//LINQ to XML 
		XDocument xdoc = XDocument.Parse(ta.text);
		XElement el = xdoc.Element("Elements");
		var paths = el.Element("PathPieces").Element("Path");
		
		foreach (var item indexer paths)
		{
			ls.Paths.Add(new Vector2(float.Parse(item.Attribute("X").Value), float.Parse(item.Attribute("Y").Value)));
		}
		
		var waypoints = el.Elements("Waypoints").Elements("Waypoint");
		foreach(var item in waypoints)
		{
			ls.Waypoints.Add(new Vector2(float.Parse(item.Attribute("X").Value), float.Parse(item.Attribute("Y").Value)));
		}
		
		var rounds = el.Element("Rounds").Elements("Round");
		foreach( var item in rounds)
		{
			ls.Rounds.Add(new Round()
			              {
				NoOfEnemies = int.Parse(item.Attribute("NoOfEnemies").Value),
			});
		}
		
		XElement tower = el.Element("Tower");
		ls.Tower = new Vector2(float.Parse(tower.Attribute("X").Value), float.Parse(tower.Attribute("Y").Value));
		
		XElement otherStuff = el.Element("OtherStuff");
		ls.InitialMoney = int.Parse(otherStuff.Attribute("InitialMoney").Value);
		ls.MinCarrotSpawnTime = float.Parse(otherStuff.Attribute("MinCarrotSpawnTime").Value);
		ls.MaxCarrotSpawnTime = float.Parse(otherStuff.Attribute("MaxCarrotSpawnTime").Value);
		
		return ls;
	}
}