using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIHealthBar : MonoBehaviour
{


    public Slider slider;
    //asd


    public void SetMaxhealth(int health)
    {
      
        slider.maxValue = health;
        slider.value = health;


    }


    public void Sethealth(int health)
    {


        slider.value = health;


    }

}
