using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDoor : MonoBehaviour
{

    public GameObject door;

	private void OnTriggerExit(Collider other)
	{
		Instantiate(door, gameObject.transform.position, gameObject.transform.rotation);
	}
}
