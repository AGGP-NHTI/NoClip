using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingDoor : MonoBehaviour
{
    public List<EnemyPawn> enemies;

    public int length;


    // Start is called before the first frame update
    void Awake()
    {
        length = enemies.Count;
    }

    // Update is called once per frame
    void Update()
    {
		for(int i = 0; i < length; i++)
		{
            if(enemies[i].isDead)
			{
                length -= 1;
			}
		}

        if(length <= 0)
		{
            Destroy(gameObject);
		}
    }
}
