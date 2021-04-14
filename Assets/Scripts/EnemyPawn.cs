using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPawn : Pawn
{
<<<<<<< HEAD
    // Start is called before the first frame update
    void Start()
    {
        
    }
=======
<<<<<<< Updated upstream
    Rigidbody rb;
>>>>>>> Local

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        
=======
        rb = gameObject.GetComponent<Rigidbody>();
=======
    public Rigidbody rb;
    public Ray raycast;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
>>>>>>> Stashed changes
>>>>>>> Local
    }
}
