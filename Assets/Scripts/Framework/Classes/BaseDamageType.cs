﻿using System.Collections;
using System.Collections.Generic;

public class BaseDamageType
{
    public string verb = "damages";
    public bool Instakill = false;
    public bool CausedByWorld = false;
    public bool IsPointDamage = false;
    public bool IsRadialDamage = false;
    public bool DoesFireDamage = false;
    public bool DoesExplosiveDamage = false;
    public bool DoesPiercingDamage = false;

    public BaseDamageType()
	{
        Initialize();
	}

    public virtual void Initialize()
	{

	}
}
