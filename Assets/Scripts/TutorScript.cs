using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class TutorScript : MonoBehaviour {
//	public AndroidConnection _link;
	public RectTransform TimerArea;
	public GameObject _TimeObject;
	public Text PresupuestoT;
	public Text PoblacionT;
	public Text Hambrientos;
	public Text ComidaT;
	public Text TiempoJugado;
	public Text WarningTxt;
	public Image FelicidadT;
	public float Timer;
	public int _ciudadanosDisponibles;
	public int _ciudadanosEnfermos;
	public int _ciudadanosTrabajando;
	public int _ciudadanosSinComida;
	public int _presupuesto = 500;

	public GameObject GranjaInstance;
	public AudioClip createBuildingSound;
	public AudioClip buttonSound;
	//AudioSource soundControl;

	public int _poblacion;
	public int _comida;
	public int numeroViviendas;
	public int numeroGranjas;
	public int PorcentajeDeIncrementoBancario = 10;
	public int _tazaimpuestos;
	int _row, _column;

	public GameObject blockSpace;
	public GameObject MainGUI;
	public GameObject SelectionGUI;
	public GameObject RemoveGUI;
	public GameObject CreationGUI;
	public GameObject LoadingUI;
	public GameObject selected;
	public Transform CamObject;
	public GameObject glowObj, glowbObj2, granjaGlow;
	public GameObject[] tutScreens;
	public bool UIenabled;
	public bool CreationMode, BuildMode, canTouch;
	public List<GameObject> _areas = new List<GameObject>();

	string _buildingOnHold;


	void Start () {
		ComidaT.text = "0";
		canTouch = true;
		UIenabled = false;
		float timeLerped = 0; 
		_ciudadanosDisponibles = _poblacion;
		_tazaimpuestos = 10;
		PresupuestoT.text = _presupuesto.ToString();
		PoblacionT.text = _poblacion.ToString();
		ComidaT.text = _comida.ToString();
	}

	public void InfoTxt()
	{
		WarningTxt.text = "No cumples los requisitos para realizar esta accion";
		WarningTxt.color = Color.red;
	}

	public void SetNewBuilding()
	{
		BuildMode = true;
		CreationMode = false;
		tutScreens [18].SetActive (true);
		MainGUI.SetActive (false);
		SelectionGUI.SetActive (false);
	}
		
	// Update is called once per frame
	void Update () 
	{
		if (canTouch) 
		{
			if ((Input.GetMouseButtonDown (0) || (Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Began)) && !UIenabled) 
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit, 100.0f)) {
					selected = hit.transform.gameObject;
					if (CreationMode && selected.tag == "Building") 
					{
						ShowCreationMenu ();
					}
					if (BuildMode && selected.tag == "Area") 
					{
						WaitForTimer ();
						StartCoroutine (CreaGranja());
					}
					if (!CreationMode && selected.name == "Granja(Clone)") 
					{
						hit.transform.SendMessage("ConsumeCharge");
					}
				}
			}
		}
	}

	public void WaitForTimer()
	{
		BuildMode = false;
		tutScreens [18].SetActive (false);
		MainGUI.SetActive (false);
	}

	public void CheckCharge1()
	{
		tutScreens [19].SetActive (false);
		tutScreens [20].SetActive (true);
	}

	public void FinalTutScreen(int opt)
	{
		switch (opt) 
		{
			case 1:
				SceneManager.LoadScene("GameDefault");
				break;
			case 2:
				SceneManager.LoadScene("TutorialLevel");
				break;
			case 3:
				SceneManager.LoadScene("MainMenu");
				break;
		}
	}

	public void CheckCharge2()
	{
		canTouch = true;
		tutScreens [20].SetActive (false);
		tutScreens [21].SetActive (true);
		MainGUI.SetActive (true);
		granjaGlow.SetActive (true);
	}

	IEnumerator CreaGranja()
	{
		int[] createdPos = new int[2]{_row, _column};
		//		soundControl.clip = createBuildingSound;
		//		soundControl.Play();
		PresupuestoT.text = "100";
		Vector3 myPosition = selected.transform.position;
		yield return StartCoroutine(ConstruyeTimer(4.0f));
		tutScreens [19].SetActive (true);
		GameObject Granja = Instantiate(GranjaInstance, new Vector3(myPosition.x, myPosition.y + 0.5f, myPosition.z), Quaternion.identity) as GameObject;
		granjaGlow = Instantiate(glowbObj2, new Vector3(myPosition.x, myPosition.y + 0.5f, myPosition.z), Quaternion.identity) as GameObject;
		granjaGlow.SetActive (false);
		ChargeScript cs = Granja.GetComponent<ChargeScript>();
		cs.parentTransform = TimerArea.transform;
		cs.TM = this;
		canTouch = false;
	}

	void ShowCreationMenu()
	{
		//SelectionGUI.SetActive (true);
		GoToNextScreem (17);
		tutScreens [16].SetActive (true);
		glowObj.SetActive (false);
	}

	public void ShowOptions()
	{
		tutScreens [16].SetActive (false);
		SelectionGUI.SetActive (true);
	}

	public void EnableGlow(bool _glow)
	{
		glowObj.SetActive (_glow);
		CreationMode = true;
	}

	void GoToNextScreem(int _screen)
	{
		tutScreens [_screen].SetActive (true);
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
		RemoveGUI.SetActive(false);
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (true);
	}

	public void RemueveEdificio()
	{
		UIenabled = false;
		RemoveGUI.SetActive(true);
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (false);
	}

	public void CancelCreate()
	{
		UIenabled = false;
		CreationGUI.SetActive(false);
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (true);
	}

	public void CreaEdificio()
	{
		UIenabled = false;
		CreationGUI.SetActive(true);
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (false);
	}

	public void PauseStart()
	{
		UIenabled = true;
		MainGUI.SetActive (false);
	}

	public void PauseEnd()
	{
		UIenabled = false;
		MainGUI.SetActive (true);
	}

	IEnumerator ConstruyeTimer(float _waitTime)
	{
		GameObject _TO = Instantiate(_TimeObject) as GameObject;
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
		Destroy(_TO);
	} 

	public void SeleccionaEdificio(string Tipo)
	{
		UIenabled = false;
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (true);
		_buildingOnHold = Tipo;
	}
		
		

	public void RecibeCargaGranja(int Cantidad)
	{
		canTouch = false;
		_comida += Cantidad;
		ComidaT.text = _comida.ToString();
		StartCoroutine (showNext ());
	}

	IEnumerator showNext()
	{
		yield return new WaitForSeconds (2);
		tutScreens [21].SetActive (false);
		tutScreens [22].SetActive (true);
		MainGUI.SetActive (false);
	}

	public void RecibeCargaMercado(int Cantidad)
	{
		_comida += Cantidad;
		_presupuesto -= Cantidad * 10;
		ComidaT.text = _comida.ToString();
		PresupuestoT.text = _presupuesto.ToString();
	}
		
	public void CancelBtn()
	{
		UIenabled = false;
		SelectionGUI.SetActive(false);
		MainGUI.SetActive (true);
	}
}
