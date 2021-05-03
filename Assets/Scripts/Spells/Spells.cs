using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spells
{
    [Tooltip("Name of Spell. Used for Reference")]
    public string Name;
    [Tooltip("ID of Spell. Used for Reference")]
    public int ID;
    [Tooltip("Amount of Energy used to cast Spell")]
    public float EnergyCost;
    [Tooltip("Context Based. Amount of Damage, Healing")]
    public float Amount;
    [Tooltip("Context Based. Amount of Speed, Duration")]
    public float Amount2;
    [Tooltip("Context Based. Time between healing ticks, Lifetime")]
    public float Duration;
    [Tooltip("Does the player need to hold the cast button?")]
    public bool HoldCast;
}
