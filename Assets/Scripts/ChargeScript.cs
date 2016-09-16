using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChargeScript : MonoBehaviour {
	public AudioClip _sound;
	public GameObject Curar, Comida, Educacion;
	public int currentCharges = 0;
	public int ChargeValue = 5;
	public int maxCharges = 5;
    public float ChargeTimer = 5f;
    public Transform parentTransform;
	public Material NotSelectedMaterial;
	public Material ResourcesMaterial;
	public Material SelectedMaterial;
    public Text ChargesText;
    public Image TimerImage;
    public Button _chargeBtn;
    public bool isFarm;
    public bool isHospital;
    public CityManager CM;
	public TutorScript TM;
	public AudioSource soundControl;

	// Use this for initialization
	void Start () {
		soundControl = GameObject.FindGameObjectWithTag ("SoundControl").GetComponent<AudioSource>();

        currentCharges = 0;
        TimerImage.transform.parent.SetParent(parentTransform);
		TimerImage.transform.parent.transform.localScale = new Vector3 (TimerImage.transform.parent.transform.localScale.x * -1, TimerImage.transform.parent.transform.localScale.y, TimerImage.transform.parent.transform.localScale.z);
        ChargesText.text = currentCharges.ToString();
        Invoke("GenerateCharges", 0f);

        TimerImage.fillAmount = 0;
		if (TM == null) 
		{
			CM.currentTimers.Add(TimerImage.transform.parent.gameObject);
		}
		TimerImage.transform.parent.LookAt (Camera.main.transform);
	}

    public void CancelInvokes()
    {
        CancelInvoke("GenerateCharges");
		CM.currentTimers.Remove (TimerImage.transform.parent.gameObject);
        Destroy(TimerImage.transform.parent.gameObject);
        Destroy(this.gameObject);
    }

	public void ConsumeCharge(){
        if (currentCharges > 0)
        {
            currentCharges -= 1;
            ChargesText.text = currentCharges.ToString();
           // gameObject.GetComponent<Renderer>().material = NotSelectedMaterial;
            if (isFarm)
            {
				soundControl.clip = _sound;
				soundControl.Play ();
				if (CM == null)
					TM.RecibeCargaGranja (10);
				else
					CM.RecibeCargaGranja (10);
				Instantiate (Comida, this.transform.position, Quaternion.identity);

            }
//            else if (isHospital)
//            {
//				soundControl.clip = _sound;
//				soundControl.Play ();
//                CM.RecibeCargaHospital(3);
//				Instantiate (Curar, this.transform.position, Quaternion.identity);
//            }
        }
        StopAllCoroutines();
        GenerateCharges();
	}


	//Function to be replaced after
	public void GenerateCharges(){
//        if (currentCharges < maxCharges)
//        {
//            currentCharges++;
//            ChargesText.text = currentCharges.ToString();
//        }
        StartCoroutine(WaitForCharge());
	}

    public IEnumerator WaitForCharge()
    {
        if (currentCharges < maxCharges)
        {
            while (TimerImage.fillAmount < 1)
            {
                TimerImage.fillAmount += Time.deltaTime / ChargeTimer;
                yield return null;
            }

            TimerImage.fillAmount = 0;
            currentCharges++;
            ChargesText.text = currentCharges.ToString();
            StopAllCoroutines();
            GenerateCharges();
        }
        else
        {
            currentCharges = maxCharges;
        }
        yield return null;
    }
}

