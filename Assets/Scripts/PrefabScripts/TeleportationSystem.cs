using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform destination;
    void Start()
    {

    }
    public void onTriggerEnter2D(Collider2D victim)
    {
        victim.transform.position = destination.transform.position;

    }
    // Update is called once per frame
    void Update()
    {

    }
}
