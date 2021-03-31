using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
	public Controller cont;
	
	void LateUpdate()
	{
		Vector3 pos = cont.GetPawn().gameObject.transform.position;
		Quaternion rot = cont.GetPawn().gameObject.transform.rotation;
		pos.y = transform.position.y;
		transform.position = pos;
		transform.rotation = rot;
	}
}
