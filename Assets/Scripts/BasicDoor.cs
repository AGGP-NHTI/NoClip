using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDoor : MonoBehaviour
{

    public GameObject door;

	private void OnTriggerExit(Collider other)
	{
		PlayerPawn pc = other.GetComponent<PlayerPawn>();

		if(pc)
		{
			Instantiate(door, gameObject.transform.position, gameObject.transform.rotation);
			Destroy(gameObject);
		}
	}
}
