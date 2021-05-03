using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BaseEffect : MonoBehaviour
{
	public string Name;
	public Actor Source;
	public float Amount;
	public float MaxDuration;
	public float CurrentDuration;
	public enum buffType { Buff, Debuff };
	public buffType typeOfBuff;

	public delegate void StateEventHandler(Pawn p);
	public delegate void UpdateEventHandler(Pawn p, float deltaTime);

	// For things that apply a constent effect
	public StateEventHandler OnBeginEffect;
	public StateEventHandler OnEndEffect;

	// For things that need an update
	public UpdateEventHandler OnUpdate;

	public void OnEnable()
	{
		Source = this.gameObject.GetComponent<Actor>();
	}

	public delegate void EffectInfoHandler(float damage, float duration);

	public event EffectInfoHandler overTime;

	public void EffectUpdate(float damage, float duraiton)
	{
		//If the effect has a update over time, call delegate
		if (overTime != null)
		{
			overTime(damage, duraiton);
		}
	}
}