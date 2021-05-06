using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDoor : MonoBehaviour
{

    public GameObject door;
	public List<EnemyPawn> enemies;

	int length;

	private void Awake()
	{
		for(int i = 0; i < length; i++)
		{
			enemies[i].canMove = false;
		}
	}

	private void Start()
	{
		length = enemies.Count;
	}

	private void OnTriggerExit(Collider other)
	{
		PlayerPawn pc = other.GetComponent<PlayerPawn>();

		if(pc)
		{
			for (int i = 0; i < length; i++)
			{
				enemies[i].canMove = true;
			}

			Instantiate(door, gameObject.transform.position, gameObject.transform.rotation);
			Destroy(gameObject);
		}
	}
}
