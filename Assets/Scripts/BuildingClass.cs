using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class BuildingClass{
    public string Name;
    public float TiempoConstruccion;
    public int CostoConstruccion;
    public int CostoMantenimiento;
    public int PersonalRequerido;
    public GameObject _instance;

    public BuildingClass()
    {
        this.Name = "";
        this.TiempoConstruccion = 0f;
        this.CostoConstruccion = 0;
        this.CostoMantenimiento = 0;
        this.PersonalRequerido = 0;
        this._instance = null;
    }
}

[System.Serializable]
public class ObjectArray{
    public List<GameObject> ObjectRow = new List<GameObject>();
}


[System.Serializable]
public class BuildingsCreated{
	public int BuildType;
	public GameObject _instance;
	public int PosX;
	public int PosY;

	public BuildingsCreated()
	{
		BuildType = 0;
		this._instance = null;
		this.PosX = 0;
		this.PosY = 0;
	}
}

[System.Serializable]
public class AreaMat{
	public bool[,] Area;

	public AreaMat()
	{
		Area = new bool[9, 9];
		Area[4, 4] = true;
	}
}
