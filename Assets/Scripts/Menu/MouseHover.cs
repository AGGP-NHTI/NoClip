using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseHover : MonoBehaviour
{
    public Sprite OnHoverSprite;
    private Sprite OnExitSprite;

	private void Awake()
	{
		OnExitSprite = this.gameObject.GetComponent<Image>().sprite;
	}

	private void OnMouseOver()
	{
		Debug.Log("Mouse Over");
		this.gameObject.GetComponent<Image>().sprite = OnHoverSprite;
	}

	private void OnMouseExit()
	{
		Debug.Log("Mouse Exit");
		this.gameObject.GetComponent<Image>().sprite = OnExitSprite;
	}
}
