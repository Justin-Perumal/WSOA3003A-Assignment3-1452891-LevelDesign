using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public Sprite[] CharacterSprites;
    public string UnitName;
    public float Damage;
    public float MaxHP;
    public float CurrentHP; 
    public float HealAmount;

    void Update()
    {
        if(UnitName == "Beserker" && CurrentHP < (MaxHP/3))
        {
            Damage = 5;
        }
    }
}
