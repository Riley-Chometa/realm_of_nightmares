using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StaminaBar : MonoBehaviour
{
    public Slider staminaSlider;

    public void setStamina(int stamina){
        staminaSlider.value = stamina;
    }

    public void SetMaxValue(int maxStamina){
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = maxStamina;
    }
}
