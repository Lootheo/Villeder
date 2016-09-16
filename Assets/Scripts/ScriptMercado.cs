using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScriptMercado : MonoBehaviour {
	public int ChargeValue = 5;
    public Transform parentTransform;
    public Image TimerImage;
    public CityManager CM;

	// Use this for initialization
	void Start () {
        TimerImage.transform.parent.SetParent(parentTransform);
	}
        
	public void ConsumeCharge(){
        CM.RecibeCargaMercado(ChargeValue);
	}
}

