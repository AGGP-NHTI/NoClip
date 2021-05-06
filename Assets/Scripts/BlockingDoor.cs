using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingDoor : MonoBehaviour
{
    public List<EnemyPawn> enemies;

    int length;


    // Start is called before the first frame update
    void Start()
    {
        length = enemies.Count;
    }

    // Update is called once per frame
    void Update()
    {
		

        if(length == 0)
		{
            Destroy(gameObject);
		}
    }
}
