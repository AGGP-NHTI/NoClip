using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardArea : MonoBehaviour
{
    public GameObject Guardian;
    Guard g;

    void Start()
    {
        g = Guardian.gameObject.GetComponent<Guard>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            g.willMove = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            g.willMove = false;
        }
    }
}
