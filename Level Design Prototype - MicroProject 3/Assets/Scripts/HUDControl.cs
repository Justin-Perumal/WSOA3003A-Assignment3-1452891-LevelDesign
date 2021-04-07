using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDControl : MonoBehaviour
{

    public TextMeshProUGUI HPText;
    public Image HPSlider;

    public void SetHUD(Unit unit)
    {
        HPText.text = "HP: " + unit.CurrentHP.ToString() + "/" + unit.MaxHP.ToString();
        HPSlider.fillAmount = unit.CurrentHP/unit.MaxHP;
    }

}
