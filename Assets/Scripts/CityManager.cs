using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class CityManager : MonoBehaviour {
//	public AndroidConnection _link;
	public enum GameStates {UIEnabled, Creation, Destruction, Update, None};
	public GameObject levelStats;
	public GameStates gs;
	public LayerMask ignoreLayer;
	public List<BuildingsCreated> creations = new List<BuildingsCreated>();
	public bool[,] AreaMatrix;
	public List<HumanClass> habitantes = new List<HumanClass>();
	public List<ObjectArray> MatrixPro = new List<ObjectArray>(); 
	public ObjectArray[] GameMatrix;
	public RectTransform TimerArea;
	public GameObject _TimeObject;
	public Text PresupuestoT;
	public Text PoblacionT;
	public Text Hambrientos;
	public Text ComidaT;
	public Text TiempoJugado;
	public Text EnfermosT;
	public Text WarningTxt;
	public Image FelicidadT;
	public float Timer;
	public int _ciudadanosDisponibles;
	public int _ciudadanosEnfermos;
	public int _ciudadanosTrabajando;
	public int _ciudadanosSinComida;
	public int _presupuesto = 500;


	public AudioClip createBuildingSound;
	public AudioClip buttonSound;
	//AudioSource soundControl;

	public int _poblacion;
	public int _comida;
	public int numeroHospitales;
	public int numeroViviendas;
	public int numeroGranjas;
	public int numeroBancos;
	public int numeroMercados;
	public int PorcentajeDeIncrementoBancario = 10;
	public int _tazaimpuestos;
	int _row, _column;

	public delegate void CurrentConstruction();
	CurrentConstruction buildNew;

	public BuildingClass[] _buildings;

	public GameObject selected;
	public GameObject MainGUI;
	public GameObject SelectionGUI;
	public GameObject RemoveGUI;
	public GameObject CreationGUI;
	public GameObject PauseGUI;
	public GameObject LoadingUI;
	public Transform CamObject;
	public bool UIenabled;
	public bool CreationMode;
	public bool _lose;

	public List<GameObject> currentTimers = new List<GameObject>();

	string _buildingOnHold;

	void Awake () 
	{
		_lose = false;
		StartCoroutine (stillPlaying ());
		MatrixPro = GameMatrix.ToList();
		AreaMatrix = new bool[9, 9];
		AreaMatrix[4, 4] = true;
	}

	IEnumerator stillPlaying()
	{
		while (!_lose) 
		{
			Timer += Time.deltaTime;
			TiempoJugado.text = Timer.ToString ("0");
			yield return null;
		}
	}

//	public void SetNextDisaster()
//	{
//		_link.CallDisaster ();
//	}

	void LoadIfExists()
	{
		DataStorage _ds = GameObject.FindObjectOfType<DataStorage> ();
		if (_ds != null) {
//			if (!_ds._usingEvents) {
//				Debug.Log ("No invocar eventos");
//				_link.CancelAllNotification();
//			} else 
//			{
//				Debug.Log ("Invocar Eventos");
//				_link.CallDisaster ();
//			}
			if (_ds._dataStored != null) {
				StatsData sd = _ds._dataStored;
				BuildingsCreated[] bc = sd._totalBuildings;
				_poblacion = sd.aldeanosTotal;
				_presupuesto = sd.dinero;
				_comida = sd.comida;
				_ciudadanosSinComida = sd.aldeanosHambrientos;
//				_ciudadanosEnfermos = sd.aldeanosEnfermos;
				Hambrientos.text = _ciudadanosSinComida.ToString ();
//				EnfermosT.text = _ciudadanosEnfermos.ToString ();;
				for (int i = 0; i < _poblacion; i++) {
					HumanClass habitante = new HumanClass ();
					habitante.CM = this;
					habitantes.Add (habitante);
				}
//				for (int i = 0; i < _ciudadanosEnfermos; i++) 
//				{
//					habitantes [i].TieneSalud = false;
//				}
				for (int x = 0; x < bc.Count (); x++) {
					GameObject parent = MatrixPro [bc [x].PosX].ObjectRow [bc [x].PosY];
					Vector3 myPosition = parent.transform.position;
					GameObject building = Instantiate (_buildings [bc [x].BuildType]._instance, new Vector3 (myPosition.x, myPosition.y + 0.5f, myPosition.z), Quaternion.identity) as GameObject;
					AreaMatrix [bc [x].PosX, bc [x].PosY] = true;
					switch (bc [x].BuildType) {
					case 0:
//						numeroHospitales++;
						break;
					case 1:
						numeroGranjas++;
						break;
					case 2:
						break;
					case 3:
						numeroViviendas++;
						break;
					case 4:
						numeroBancos++;
						break;
					case 5:
						numeroBancos++;
						break;
					}
					if (bc [x].BuildType != 3) {
						ChargeScript cs = building.GetComponent<ChargeScript> ();
						cs.parentTransform = TimerArea.transform;
						cs.CM = this;
					}
				}
			} else {
				_poblacion = 100;
				_presupuesto = 1000;
				_comida = 1000;
				Hambrientos.text = "0";
//				EnfermosT.text = "0";
				for (int i = 0; i < _poblacion; i++) {
					HumanClass habitante = new HumanClass ();
					habitante.CM = this;
					habitantes.Add (habitante);
				}
			}
			Destroy (_ds.gameObject);
		} 
		else 
		{
			_poblacion = 100;
			_presupuesto = 1000;
			_comida = 1000;
			Hambrientos.text = "0";
			//		EnfermosT.text = "0";
			for (int i = 0; i < _poblacion; i++)
			{
				HumanClass habitante = new HumanClass();
				habitante.CM = this;
				habitantes.Add(habitante);
			}
		}
	}

	void Start () {
		LoadIfExists ();
		gs = GameStates.None;
		UIenabled = false;
		float timeLerped = 0; 
		_ciudadanosDisponibles = _poblacion;
		_tazaimpuestos = 10;
		PresupuestoT.text = _presupuesto.ToString();
		PoblacionT.text = _poblacion.ToString();
		ComidaT.text = _comida.ToString();
		InvokeRepeating("GeneraComida", 10f, 5f);
		InvokeRepeating("GeneraPresupuesto", 0f, 5f);
		InvokeRepeating("MantenimientoInstalaciones", 10f, 10f);
		InvokeRepeating("ConsumeComida", 10f, 5f);
//		InvokeRepeating("ActualizaEnfermos", 10f, 10f);
	}

	public void InfoTxt()
	{
		WarningTxt.text = "No cumples los requisitos para realizar esta accion";
		WarningTxt.color = Color.red;
	}
	float timeLerped = 0; 

	IEnumerator ConterSpeed(string Type, int From, int To)
	{
		float timeToLerp = 2f; //lerp for two seconds.
		switch(Type)
		{
		case "poblacion":
			while (_poblacion != To)
			{
				timeLerped += Time.deltaTime;
				_poblacion = Mathf.RoundToInt(Mathf.Lerp(From, To, timeLerped / timeToLerp));
				PoblacionT.text = _poblacion.ToString();
				yield return null;
			}
			break;
		case "presupuesto":
			while (_presupuesto != To)
			{
				timeLerped += Time.deltaTime;
				_presupuesto = Mathf.RoundToInt(Mathf.Lerp(From, To, timeLerped / timeToLerp));
				PresupuestoT.text = _presupuesto.ToString();
				yield return null;
			}
			break;
		case "comida":
			while (_comida != To)
			{
				timeLerped += Time.deltaTime;
				_comida = Mathf.RoundToInt(Mathf.Lerp(From, To, timeLerped / timeToLerp));
				ComidaT.text = _comida.ToString();
				yield return null;
			}
			break;
//		case "enfermos":
//			while (_ciudadanosEnfermos != To)
//			{
//				timeLerped += Time.deltaTime;
//				_ciudadanosEnfermos = Mathf.RoundToInt(Mathf.Lerp(From, To, timeLerped / timeToLerp));
//				EnfermosT.text = _ciudadanosEnfermos.ToString();
//				yield return null;
//			}
//			break;
		}
		yield return null;
	}

	public void FindInDimensions(GameObject searchTerm)
	{
		for (int row = 0; row < MatrixPro.Count; row++)
		{
			for (int col = 0; col < MatrixPro[row].ObjectRow.Count; col++)
			{
				if(MatrixPro[row].ObjectRow[col] == searchTerm)
				{
					_row = row;
					_column = col;
					break;
				}
			}
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if ((Input.GetMouseButtonDown (0) || (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Began)) && !UIenabled) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 100.0f ,ignoreLayer)) {
				FindInDimensions(hit.transform.gameObject);
				selected = hit.transform.gameObject;
				switch (gs) {
				case GameStates.Creation:
					if (!AreaMatrix [_row, _column] && hit.transform.name != "CentroUrbano") {
						ConstruyeEdificio ();
						AreaMatrix [_row, _column] = true;
					} else {
						Debug.Log ("Cuadro ocupado");
					}
					gs = GameStates.None;
						CreationGUI.SetActive (false);
						MainGUI.gameObject.SetActive (true);
						break;

					case GameStates.Destruction:
						if (hit.transform.tag == "Building")
							{
								GameObject _build = hit.transform.gameObject;
								BuildingsCreated created = null;
								if ((created = creations.Find (x => x._instance == _build))!=null) 
								{
									hit.transform.SendMessage("CancelInvokes");
									AreaMatrix[created.PosX, created.PosY] = false;
									RemoveGUI.SetActive(false);
									MainGUI.gameObject.SetActive (true);
									creations.Remove (created);
									Destroy(hit.transform.gameObject);
								}
							}
						gs = GameStates.None;
						MainGUI.SetActive(true);
						break;

					case GameStates.None:
						switch (hit.transform.name)
						{
							case "CentroUrbano":
								EnableSelectionUI();
	//							soundControl.clip = buttonSound;
	//							soundControl.Play();
								break;
//							case "Hospital(Clone)":
//								hit.transform.SendMessage("ConsumeCharge");
//								break;
							case "Granja(Clone)":
								hit.transform.SendMessage("ConsumeCharge");
								break;
							case "Mercado(Clone)":
								hit.transform.SendMessage("ConsumeCharge");
								break;
							default:
								break;
							}
						break;
				}
			}
		}
	}

	void EnableSelectionUI()
	{
		UIenabled = true;
		SelectionGUI.SetActive(true);
		MainGUI.SetActive (false);
	}

	public void CancelRemove()
	{
		UIenabled = false;
		gs = GameStates.None;
		RemoveGUI.SetActive(false);
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (true);
	}

	public void RemueveEdificio()
	{
		UIenabled = false;
		gs = GameStates.Destruction;
		RemoveGUI.SetActive(true);
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (false);
	}

	public void CancelCreate()
	{
		UIenabled = false;
		gs = GameStates.None;
		CreationGUI.SetActive(false);
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (true);
	}

	public void CreaEdificio()
	{
		UIenabled = false;
		gs = GameStates.Creation;
		CreationGUI.SetActive(true);
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (false);
	}

	public void PauseStart()
	{
		UIenabled = true;
		PauseGUI.SetActive(true);
		MainGUI.SetActive (false);
	}

	public void PauseEnd()
	{
		UIenabled = false;
		PauseGUI.SetActive(false);
		MainGUI.SetActive (true);
	}

	IEnumerator ConstruyeTimer(float _waitTime)
	{
		GameObject _TO = Instantiate(_TimeObject) as GameObject;
		currentTimers.Add(_TO);
		_TO.transform.LookAt(Camera.main.transform.position);
		_TO.transform.SetParent(TimerArea.transform);
		_TO.GetComponent<RectTransform>().position = new Vector3(selected.transform.position.x, selected.transform.position.y + 1, selected.transform.position.z);
		_TO.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
		Image[] Timer = _TO.GetComponentsInChildren<Image>();
		float ConsTime = _waitTime;
		Timer[1].fillAmount = 0;
		while (Timer[1].fillAmount < 1)
		{
			Timer[1].fillAmount += Time.deltaTime/ConsTime;
			yield return null;
		}
		currentTimers.Remove(_TO);
		Destroy(_TO);
	}
		

	void GeneraPresupuesto()
	{
		int totalPoblacion = _poblacion + _ciudadanosTrabajando;
		_presupuesto += Mathf.FloorToInt(totalPoblacion * (10 * (_tazaimpuestos * 0.01f)));
		if (numeroBancos > 0)
		{
			_presupuesto = _presupuesto + Mathf.FloorToInt(_presupuesto*(numeroBancos * PorcentajeDeIncrementoBancario * 0.01f));
		}
		PresupuestoT.text = _presupuesto.ToString();
	}


	void GeneraComida()
	{
//		_comida += (numeroGranjas * 10);
//		ComidaT.text = _comida.ToString();
	}
		

	void ConsumeComida()
	{
		if (_comida < habitantes.Count) 
		{
			int hambrientos = habitantes.Count - _comida;
			Debug.Log (hambrientos);
			int _hambre = 0;
			for (int x = habitantes.Count - hambrientos; x < habitantes.Count; x++) 
			{
				habitantes [x].TieneComida = false;
				habitantes [x].Hungry ();
				_hambre++;
			}
			Debug.Log (_hambre);
			_comida = 0;
			ComidaT.text = _comida.ToString ();
			Hambrientos.text = _hambre.ToString ();
			FelicidadT.fillAmount -= 0.1f;
			if (FelicidadT.fillAmount <= 0) {
				GameObject data = Instantiate (levelStats) as GameObject;
				DataLevel dl = data.GetComponent<DataLevel> ();
				dl.TotalTime = TiempoJugado.text;
				Scene current = SceneManager.GetActiveScene ();
				if (current.name == "GameDefault") {
					dl.LevelType = 1;
				} else if (current.name == "GameDessert") {
					dl.LevelType = 2;
				}
				Scene loseScene = SceneManager.GetSceneByName ("LoseScene");
				//SceneManager.MoveGameObjectToScene (data, loseScene);
				SceneManager.LoadScene ("LoseScene");
			}
		} 
		else 
		{
			_comida -= _poblacion;
			ComidaT.text = _comida.ToString ();
			int _hambre = 0;
			for (int x = 0; x < habitantes.Count; x++) 
			{
				habitantes [x].TieneComida = true;
				habitantes [x].TiempoConHambre = 0;
			}
			Hambrientos.text = _hambre.ToString ();
		}
	}

//	void ActualizaEnfermos()
//	{
//		for (int i = 0; i < habitantes.Count; i++)
//		{
//			habitantes[i].CheckInfection();
//		}
//		EnfermosT.text = _ciudadanosEnfermos.ToString();
//	}

	void MantenimientoInstalaciones()
	{
		_presupuesto -= 
			(numeroGranjas * _buildings[1].CostoMantenimiento) + 
			(numeroHospitales * _buildings[0].CostoMantenimiento) +
			(numeroViviendas * _buildings[3].CostoMantenimiento) +
			(numeroBancos * _buildings[4].CostoMantenimiento) +
			(numeroMercados * _buildings[5].CostoMantenimiento);
	}    

	public void SeleccionaEdificio(string Tipo)
	{
		gs = GameStates.Creation;
		UIenabled = false;
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (true);
		_buildingOnHold = Tipo;
	}

	void ConstruyeEdificio()
	{
		if (selected != null)
		{
			switch (_buildingOnHold)
			{
			case "vivienda":
				if (_presupuesto >= _buildings [3].CostoConstruccion && _ciudadanosDisponibles >= _buildings [3].PersonalRequerido) {
					StartCoroutine (CreaVivienda ());
				} else {
					InfoTxt ();
				}
				break;
//			case "hospital":
//				if (_presupuesto >= _buildings [0].CostoConstruccion && _ciudadanosDisponibles >= _buildings [0].PersonalRequerido) 
//				{
//					StartCoroutine(CreaHospital());
//				}else {
//					InfoTxt ();
//				}
//				break;
			case "granja":
				if (_presupuesto >= _buildings [1].CostoConstruccion && _ciudadanosDisponibles >= _buildings [1].PersonalRequerido) 
				{
					StartCoroutine(CreaGranja());
				}else {
					InfoTxt ();
				}
				break;
			case "banco":
				if (_presupuesto >= _buildings [4].CostoConstruccion && _ciudadanosDisponibles >= _buildings [4].PersonalRequerido) 
				{
					StartCoroutine(CreaBanco());
				}else {
					InfoTxt ();
				}
				break;
			case "mercado":
				if (_presupuesto >= _buildings [5].CostoConstruccion && _ciudadanosDisponibles >= _buildings [5].PersonalRequerido) 
				{
					StartCoroutine(CreaMercado());
				}else {
					InfoTxt ();
				}
				break;
			}
			_buildingOnHold = "";
		}
	}


//	IEnumerator CreaEscuela()
//	{
//		int[] createdPos = new int[2]{_row, _column};
////		soundControl.clip = createBuildingSound;
////		soundControl.Play();
//		_ciudadanosDisponibles -= _buildings[2].PersonalRequerido;
//		_presupuesto -= _buildings[2].CostoConstruccion;
//		Vector3 myPosition = selected.transform.position;
//		PresupuestoT.text = _presupuesto.ToString();
//		yield return StartCoroutine(ConstruyeTimer(_buildings[2].TiempoConstruccion));
//		GameObject Escuela = Instantiate(_buildings[2]._instance, new Vector3(myPosition.x, myPosition.y + 0.5f, myPosition.z), Quaternion.identity) as GameObject;
//		ChargeScript cs = Escuela.GetComponent<ChargeScript>();
//		cs.parentTransform = TimerArea.transform;
//		cs.CM = this;
//		_ciudadanosDisponibles += _buildings[2].PersonalRequerido;
//		numeroEscuelas += 1;
//		creations.Add (new BuildingsCreated() { PosX = createdPos[0], PosY = createdPos[1], _instance = Escuela , BuildType = 2});
//	}

	IEnumerator CreaBanco()
	{
		int[] createdPos = new int[2]{_row, _column};
//		soundControl.clip = createBuildingSound;
//		soundControl.Play();
		_ciudadanosDisponibles -= _buildings[4].PersonalRequerido;
		_presupuesto -= _buildings[4].CostoConstruccion;
		Vector3 myPosition = selected.transform.position;
		PresupuestoT.text = _presupuesto.ToString();
		yield return StartCoroutine(ConstruyeTimer(_buildings[4].TiempoConstruccion));
		GameObject Banco = Instantiate(_buildings[4]._instance, new Vector3(myPosition.x, myPosition.y + 0.5f, myPosition.z), Quaternion.identity) as GameObject;
		ChargeScript cs = Banco.GetComponent<ChargeScript>();
		cs.parentTransform = TimerArea.transform;
		cs.CM = this;
		_ciudadanosDisponibles += _buildings[4].PersonalRequerido;
		numeroBancos += 1;
		creations.Add (new BuildingsCreated() { PosX = createdPos[0], PosY = createdPos[1], _instance = Banco , BuildType = 4});
	}

	IEnumerator CreaMercado()
	{
		int[] createdPos = new int[2]{_row, _column};
//		soundControl.clip = createBuildingSound;
//		soundControl.Play();
		_ciudadanosDisponibles -= _buildings[5].PersonalRequerido;
		_presupuesto -= _buildings[5].CostoConstruccion;
		Vector3 myPosition = selected.transform.position;
		PresupuestoT.text = _presupuesto.ToString();
		yield return StartCoroutine(ConstruyeTimer(_buildings[5].TiempoConstruccion));
		GameObject Mercado = Instantiate(_buildings[5]._instance, new Vector3(myPosition.x, myPosition.y + 0.5f, myPosition.z), Quaternion.identity) as GameObject;
		ScriptMercado cs = Mercado.GetComponent<ScriptMercado>();
		cs.parentTransform = TimerArea.transform;
		cs.CM = this;
		_ciudadanosDisponibles += _buildings[5].PersonalRequerido;
		numeroMercados += 1;
		creations.Add (new BuildingsCreated() { PosX = createdPos[0], PosY = createdPos[1], _instance = Mercado , BuildType = 5});
	}

	IEnumerator CreaVivienda()
	{
		int[] createdPos = new int[2]{_row, _column};
//		soundControl.clip = createBuildingSound;
//		soundControl.Play();
		_ciudadanosDisponibles -= _buildings[3].PersonalRequerido;
		Vector3 myPosition = selected.transform.position;
		_presupuesto -= _buildings[3].CostoConstruccion;
		PresupuestoT.text = _presupuesto.ToString();
		yield return StartCoroutine(ConstruyeTimer(_buildings[3].TiempoConstruccion));
		GameObject building = Instantiate(_buildings[3]._instance, new Vector3(myPosition.x, myPosition.y + 0.5f, myPosition.z), Quaternion.identity) as GameObject;
		_ciudadanosDisponibles += _buildings[3].PersonalRequerido;
		numeroViviendas += 1;
		timeLerped = 0.0f;
		StartCoroutine(ConterSpeed("poblacion", habitantes.Count, habitantes.Count + 20));
		for (int i = 0; i < 20; i++)
		{
			HumanClass habitante = new HumanClass();
			habitante.CM = this;
			habitantes.Add(habitante);
		}
		_ciudadanosDisponibles += 20;
		PoblacionT.text = _poblacion.ToString();
		creations.Add (new BuildingsCreated() { PosX = createdPos[0], PosY = createdPos[1], _instance = building , BuildType = 3});
	}

	IEnumerator CreaGranja()
	{
		int[] createdPos = new int[2]{_row, _column};
//		soundControl.clip = createBuildingSound;
//		soundControl.Play();
		_ciudadanosDisponibles -= _buildings[1].PersonalRequerido;
		_presupuesto -= _buildings[1].CostoConstruccion;
		PresupuestoT.text = _presupuesto.ToString();
		Vector3 myPosition = selected.transform.position;
		yield return StartCoroutine(ConstruyeTimer(_buildings[1].TiempoConstruccion));
		GameObject Granja = Instantiate(_buildings[1]._instance, new Vector3(myPosition.x, myPosition.y + 0.5f, myPosition.z), Quaternion.identity) as GameObject;
		ChargeScript cs = Granja.GetComponent<ChargeScript>();
		cs.parentTransform = TimerArea.transform;
		cs.CM = this;
		_ciudadanosDisponibles += _buildings[1].PersonalRequerido;
		numeroGranjas += 1;
		creations.Add (new BuildingsCreated() { PosX = createdPos[0], PosY = createdPos[1], _instance = Granja, BuildType = 1 });
	}

//	IEnumerator CreaHospital()
//	{
//		int[] createdPos = new int[2]{_row, _column};
////		soundControl.clip = createBuildingSound;
////		soundControl.Play();
//		_ciudadanosDisponibles -= _buildings[0].PersonalRequerido;
//		_presupuesto -= _buildings[0].CostoConstruccion;
//		PresupuestoT.text = _presupuesto.ToString();
//		Vector3 myPosition = selected.transform.position;
//		yield return StartCoroutine(ConstruyeTimer(_buildings[0].TiempoConstruccion));
//		GameObject Hospital = Instantiate(_buildings[0]._instance, new Vector3(myPosition.x, myPosition.y + 0.5f, myPosition.z), Quaternion.identity) as GameObject;
//		ChargeScript cs = Hospital.GetComponent<ChargeScript>();
//		cs.parentTransform = TimerArea.transform;
//		cs.CM = this;
//		numeroHospitales += 1;
//		_ciudadanosDisponibles += _buildings[0].PersonalRequerido;
//		creations.Add (new BuildingsCreated() { PosX = createdPos[0], PosY = createdPos[1], _instance = Hospital, BuildType = 0 });
//	}

	public void RecibeCargaGranja(int Cantidad)
	{
		_comida += Cantidad;
		ComidaT.text = _comida.ToString();
	}

//	public void RecibeCargaHospital(int Cantidad)
//	{
//		_ciudadanosEnfermos--;
//		for (int i = 0; i < habitantes.Count; i++)
//		{
//			if (habitantes[i].TieneSalud == false)
//			{
//				habitantes[i].TieneSalud = true;
//				habitantes[i].TiempoEnfermo = 0;
//				Debug.Log(i);
//				break;
//			}
//		}
////		EnfermosT.text = _ciudadanosEnfermos.ToString();
//	}

//	public void RecibeCargaEscuela(int Cantidad)
//	{
//		_ciudadanosConEstudios++;
//		for (int i = 0; i < habitantes.Count; i++)
//		{
//			if (habitantes[i].TieneEstudios == false)
//			{
//				habitantes[i].TieneSalud = true;
//				break;
//			}
//		}
//		EstudiosT.text = (_poblacion - _ciudadanosConEstudios).ToString ();
//	}

	public void RecibeCargaMercado(int Cantidad)
	{
		_comida += Cantidad;
		_presupuesto -= Cantidad * 10;
		ComidaT.text = _comida.ToString();
		PresupuestoT.text = _presupuesto.ToString();
	}

	public void Impuestos(int Type)
	{
		switch (Type)
		{
		case 0:
			_tazaimpuestos += 5;
			break;
		case 1:
			if (_tazaimpuestos >= 10)
			{
				_tazaimpuestos -= 5;
			}
			break;
		}
		UIenabled = false;
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (true);
	}

	public void CancelBtn()
	{
		UIenabled = false;
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (true);
		gs = GameStates.None;
	}
}
