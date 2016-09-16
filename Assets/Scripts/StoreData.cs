using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class StoreData{
	//public static Achivements Names = new List<Achivements>();

	public static void Save(StatsData cham) {
		//FileStream file = File.Open(Application.persistentDataPath + "/TestData.txt", FileMode.OpenOrCreate);
		string json = JsonUtility.ToJson(cham);
		StreamWriter sw = File.CreateText(Application.persistentDataPath+ "/StatsData.txt");
		sw.WriteLine (json);
		sw.Close();
	}

	public static StatsData Load() {
		if(File.Exists(Application.persistentDataPath + "/StatsData.txt")) {
			string newJson = "";
			StreamReader sr = File.OpenText(Application.persistentDataPath+ "/StatsData.txt");
			newJson = sr.ReadLine ();
			StatsData cham2 = JsonUtility.FromJson<StatsData>(newJson);
			sr.Close ();
			return cham2;
		}
		else
			return new StatsData();
	}

	public static void Delete()
	{
		if(File.Exists(Application.persistentDataPath + "/StatsData.txt")) {
			File.Delete(Application.persistentDataPath + "/StatsData.txt");
		}
	}
}

