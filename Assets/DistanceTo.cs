using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTo : MonoBehaviour
{

    public Transform target;
    float distance;


    // Start is called before the first frame update
    void Start()
    {
        target = gameObject.GetComponent<Dum>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(target.position, transform.position);

        Debug.Log(Mathf.FloorToInt(distance).ToString());
    }
}
