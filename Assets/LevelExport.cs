using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Xml.Linq;


public class LevelExport : EditorWindow {

	[MenuItem("Custom Editor/Export Level")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow (typeof(LevelExport));
	}

	Vector2 scrollPosition = Vector2.zero;
	int noOfEnemies;
	int initialMoney;
	int MinCarrotSpawnTime, MaxCarrotSpawnTime;
	string filename;
	int waypointsCount;
	int pathsCount;

	void OnGUI()
	{
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition);
		EditorGUILayout.LabelField("Total Rounds created :" + rounds.Count);
		for (int i = 0; i < rounds.Count; ++i) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Round " + (i + 1));
			EditorGUILayout.LabelField("Number of enemies" + rounds[i].NoOfEnemies);
			if (GUILayout.Button ("Delete"))
			{
				rounds.RemoveAt(i);
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndScrollView ();

		EditorGUILayout.LabelField ("Add a new round", EditorStyles.boldLabel);
		noOfEnemies = EditorGUILayout.IntSlider ("Number of enemies", noOfEnemies, 1, 20);

		if (GUILayout.Button ("Add new round"))
		{
			rounds.Add(new Round() { NoOfEnemies = noOfEnemies });
		}
		initialMoney = EditorGUILayout.IntSlider ("Initial Money", initialMoney, 200, 400);
		MinCarrotSpawnTime = EditorGUILayout.IntSlider ("MinCarrotSpawnTime", MinCarrotSpawnTime, 1, 10);
		MaxCarrotSpawnTime = EditorGUILayout.IntSlider ("MaxCarrotSpawnTime", MaxCarrotSpawnTime, 1, 10);
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("Filename: ");
		filename = EditorGUILayout ("LevelX.xml");
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.LabelField ("Export Level", EditorStyles.boldLabel);
		if (GUILayout.Button ("Export")) 
		{
			Export();
		}
	}

	void Export()
	{
		//Create a new output filestream
		doc = new XDocument ();
		doc.Add (new XElement ("Elements"));
		XElement elements = doc.Element ("Elements");

		XElement pathPiecesXML = new XElement ("PathPieces");
		var paths = GameObject.FindGameObjectWithTag ("Path");

		foreach (var item in paths) {
			XElement path = new XElement("Path");
			XAttribute attrX = new XAttribute("X",item.transform.position.x);
			XAttribute attrY = new XAttribute("Y",item.transform.position.y);
			path.Add (attrX,attrY);
			pathPiecesXML.Add (path);
		}
		pathsCount = paths.Length;
		elements.Add (pathPiecesXML);

		XElement waypointsXML = new XElement ("Waypoints");
		var waypoints = GameObject.FindGameObjectsWithTag ("Waypoint");
		if (!WaypointsAreValid(waypoints))
		{
			return;
		}

		//Order by user selected order
		waypoints = waypoints.OrderBy (x => x.GetComponent<OrderedWaypointForEditor> ().Order).ToArray ();
		foreach (var item in waypoints) {
			XElement waypoint = new XElement ("Waypoint");
			XAttribute attrX = new XAttribute ("X", item.transform.position.x);
			XAttribute attrY = new XAttribute ("Y", item.transform.position.y);
			waypoint.Add (attrX, attrY);
			waypointsXML.Add (waypoint);
		}
		waypointsCount = waypoints.Length;
		elements.Add (waypointsXML);

		XElement roundsXML = new XElement ("Rounds");
		foreach (var item in roundsXML) {
			XElement round = new XElement ("Round");
			XAttribute NoOfEnemies = new XAttributes ("NoOfEnemies", item.NoOfEnemies);
			round.Add (NoOfEnemies);
			roundsXML.Add (round);
		}
		elements.Add (roundsXML);

		XElement towerXML = new XElement ("Tower");
		var tower = GameObject.FindGameObjectWithTag ("Tower");
		if (tower == null) {
			ShowErrorForNull ("Tower");
			return;
		}
		XAttribute towerX = new XAttribute ("X", tower.transform.position.x);
		XAttribute towerY = new XAttribute ("Y", tower.transform.position.y);
		towerXML.Add (towerX, towerY);
		elements.Add (towerXML);

		if (!InputIsValid ())
			return;

		if (EditorUtility.DisplayDialog ("Save confirmation",
		                                "Are you sure you want to save level " + filename, "OK", "Cancel")) {
			doc.Save ("Assets/" + filename);
			EditorUtility.DisplayDialog ("Saved", filename + " saved!", "OK");
		} else {
			EditorUtility.DisplayDialog ("NOT Saved", filename + "not saved!", "OK");
		}
	}
}
