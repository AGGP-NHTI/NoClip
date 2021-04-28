using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : Info
{
    public static Game Instance;

    public GameObject SpawnerObject; //In the chance there are no Spawnpoints in world
    public Spawnpoint[] Spawnpoints; //Array of Spawnpoints already in world
    int spawnIndex;
    Spawnpoint ChosenSpawn; //Used in ReturnSpawn when Spectator is requesting a spawn location for new Pawn
    public int score = 0;
    public float scoreMX = 1.0f;
    public int kills = 0;
    public int streak = 0;

    //Singleton Instance of Class
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Spawnpoints = GameObject.FindObjectsOfType(typeof(Spawnpoint)) as Spawnpoint[];

        if (Spawnpoints.Length == 0)
        {
            Debug.LogError("No Spawnpoints found!");
            Debug.LogWarning("Creating Default Point at World Center");

            Factory(SpawnerObject, Vector3.zero, Rotation);
        }
    }

    public Spawnpoint ReturnSpawn()
    {
        spawnIndex = Random.Range(0, Spawnpoints.Length - 1);

        ChosenSpawn = Spawnpoints[spawnIndex];

        return ChosenSpawn;
    }
}
