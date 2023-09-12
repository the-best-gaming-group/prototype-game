using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;//character name
    //public int unitLevel;

    //public int damage;

    public int maxHP;
    public int currentHP;
    //public int experience;

    public int TakeDamage(int damage)
    {
        return currentHP = (damage >= currentHP ? 0 : currentHP - damage);
    }
}
