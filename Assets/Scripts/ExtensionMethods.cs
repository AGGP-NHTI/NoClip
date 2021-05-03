using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
	//Source: https://forum.unity.com/threads/get-ui-components-by-name-with-getcomponent.408004/
	public static T GetComponentInChildrenByName<T>(this GameObject GO, string name) where T : Component
	{
		foreach (T component in GO.GetComponentsInChildren<T>(true))
		{
			if (component.gameObject.name == name)
			{
				return component;
			}
		}
		return null;
	}

	public static BaseEffect NewInstance(this BaseEffect effect, BaseEffect Reference, GameObject Object)
	{
		BaseEffect newEffect = Object.AddComponent<BaseEffect>(); // <BaseEffect> was <Effects> -- Check for Error
		newEffect.Name = Reference.Name;
		newEffect.Amount = Reference.Amount;
		newEffect.typeOfBuff = Reference.typeOfBuff;
		newEffect.CurrentDuration = Reference.CurrentDuration - 0.75f;
		newEffect.MaxDuration = Reference.MaxDuration;

		return newEffect;
	}
}
