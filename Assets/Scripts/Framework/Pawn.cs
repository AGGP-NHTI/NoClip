using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pawn, Can be Possesed by Controller.
/// BY DESIGN, this implementation does not have any default Input methods.
/// Inherit from Pawn into your own class to implement those functions. 
/// </summary>
public class Pawn : Actor 
{

	public float StartingHealth = 100.0f;
	public float Health = 100.0f;
	public float moveSpeed = 5; // Movement Speed
	public bool IsSpectator = true;
	public Transform groundCheck;
	protected float groundDistance = 0.4f;
	public LayerMask groundMask;
	protected bool isGrounded;

	protected bool radMenuOpen = false;

	public List<BaseEffect> OngoingEffects;
	public List<GameObject> ParticleGenerators;

	public bool abilityMenuOpen
	{
		get { return radMenuOpen; }
		set { radMenuOpen = value; }
	}

	/// <summary>
	/// Controler who is in charge of this object. 
	/// </summary>
	protected Controller _controller;

	/// <summary>
	/// READY ONLY, access to the Controller (Player or AI) for this pawn
	/// </summary>
	public Controller controller
	{
		get { return _controller;  }
	}

	/// <summary>
	/// Method access to the Controller (Player or AI) for this pawn
	/// </summary>
	public Controller GetController()
	{
		return _controller; 
	}

	/// <summary>
	/// Sets controller and Owner Variables 
	/// </summary>
	/// <param name="c">Controller to take control of this pawn</param>
	/// <returns></returns>
	public bool BecomeControlledBy (Controller c)
	{
		_controller = c;
		Owner = c; 
		OnControlled(); 
		return true;
	}

	/// <summary>
	/// Pawn is no longer possessed. Owner is Unchanged. 
	/// Calls OnUnPossession  
	/// </summary>
	public void BecomeReleased()
	{
		OnRelease();
		_controller = null;
	}

	/// <summary>
	/// Pawn is no longer possessed. This version resets Owner to null as well.
	/// Calls OnUnPossession
	/// </summary>
	/// <returns>Previous Controller</returns>
	public Controller ForceRelease()
	{
		OnRelease();
		Controller c = _controller; 
		_controller = null;
		Owner = null; 
	 
		return c;
	}

	public virtual void OnControlled()
	{

	}

	public virtual void OnRelease()
	{
		
	}

	public virtual void Update()
	{
		OnUpdateEffect();
	}

	public virtual void Look()
	{

	}

	public virtual void Move(float x, float z)
	{
	   
	}

	public virtual void Jump()
	{

	}

	public virtual void Fire1(bool value)
	{

	}

	public virtual void Fire2(bool value)
	{

	}

	public virtual void Fire3(bool value)
	{

	}

	public virtual void AbilityMenu(bool value)
	{

	}

	public virtual void AddEffectHUD(BaseEffect effect)
	{

	}

	public virtual void RemoveEffectHUD(BaseEffect effect)
	{

	}

	public virtual void UpdateEffectHUD(BaseEffect effect, float percentageFill)
	{

	}

	public void OnUpdateEffect()
	{
		List<BaseEffect> copyOngoingEffects = new List<BaseEffect>(OngoingEffects);

		foreach (BaseEffect effect in copyOngoingEffects)
		{
			//Do OnUpdate for each effect in list
			if (!IgnoreStatus)
			{
				effect.OnUpdate(this, Time.deltaTime);
			}
		}
	}

	public virtual void AddOngingEffect(BaseEffect effect)
	{
		if (!IgnoreStatus)
		{
			OngoingEffects.Add(effect);

			if (effect.OnBeginEffect != null)
			{
				effect.OnBeginEffect(this);
			}
		}
	}

	public virtual void RemoveOngingEffect(BaseEffect effect)
	{
		if (!IgnoreStatus)
		{
			OngoingEffects.Remove(effect);

			if (effect.OnEndEffect != null)
			{
				effect.OnEndEffect(this);
			}
		}
	}

	protected override bool ProcessDamage(Actor Source, float Value, DamageEventInfo EventInfo, Controller Instigator)
	{
		Health -= Value;

		//Debug.Log(gameObject.name + " Health - " + Value + " by " + Instigator);

		if (Health <= 0)
		{
			//Debug.Log(gameObject.name + " is Dead!");

			IgnoresDamage = true;

			if (controller.IsHuman)
			{
				controller.RequestSpectator();
			}

			OnDeath();
		}

		if (EventInfo.DamageType.DoesFireDamage)
		{
			Debug.Log(gameObject.name + " takes Fire Damage");
		}

		if (EventInfo.DamageType.DoesExplosiveDamage)
		{
			Debug.Log(gameObject.name + " takes Explosion Damage");
		}

		if (EventInfo.DamageType.DoesPiercingDamage)
		{
			Debug.Log(gameObject.name + " takes Piercing Damage");
		}

		return true;
	}

	protected virtual void OnDeath()
	{
		Destroy(gameObject);
	}
}
