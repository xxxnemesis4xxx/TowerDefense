using UnityEngine;
using System.Collections;
using UnityEditor;

public class LevelExport : EditorWindow {

	[MenuItem("Custom Editor/Export Level")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow (typeof(LevelExport));
	}

	Vector2 scrollPosition = Vector2.zero;
	int noOfEnemies;
	int initialMoney;
	int MinCarroSpawnTime, MaxCarrotSpawnTime;
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
}
