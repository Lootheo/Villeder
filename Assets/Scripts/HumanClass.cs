using UnityEngine;
using System.Collections;

[System.Serializable]
public class HumanClass{
    public bool TieneCasa;
    public bool TieneSalud;
    public bool TieneComida;
    public bool TieneEstudios;
    public int TiempoConHambre;
    public int TiempoEnfermo;
    public int InfectionChance;
    public CityManager CM;

    public HumanClass()
    {
        this.TieneCasa = false;
        this.TieneSalud = true;
        this.TieneComida = false;
        this.TieneEstudios = false;
        this.TiempoConHambre = 0;
        this.TiempoEnfermo = 0;
        this.InfectionChance = 2;
    }

    public void Schoolarship()
    {
        
    }

//    public void CheckInfection()
//    {
//        if (TieneSalud)
//        {
//            int x = Random.Range(0, 101);
//            if (x <= InfectionChance)
//            {
//                TieneSalud = false;
//                CM._ciudadanosEnfermos += 1;
//            }
//        }
//    }

    public void Hungry()
    {
        TiempoConHambre++;
        if (TiempoConHambre >= 12)
        {
            CM.habitantes.Remove(this);
        }
    }

    public void Sick()
    {
        TiempoEnfermo++;
        if (TiempoEnfermo >= 8)
        {
            CM.habitantes.Remove(this);
        }
    }
}
