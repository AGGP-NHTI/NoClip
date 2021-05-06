using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingDoor : MonoBehaviour
{
    public List<EnemyPawn> enemies;

	public void Awake()
	{
		foreach(EnemyPawn e in enemies)
		{
			e.door = this;
		}
	}

	// Update is called once per frame
	void Update()
    {
        if (enemies.Count == 0)
		{
            Destroy(gameObject);
		}
    }

    public void RemoveEnemy(EnemyPawn enemy)
	{
        enemies.Remove(enemy);
	}
}
