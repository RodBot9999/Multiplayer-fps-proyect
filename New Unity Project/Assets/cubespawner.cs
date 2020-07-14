using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubespawner : MonoBehaviour
{
    objectpooler objectpool;


    private void Start()
    {
        objectpool = objectpooler.Instance;
    }

    void FixedUpdate ()
    {
        objectpool.SpawnFromPool("Sphere", transform.position, Quaternion.identity);
    }
}
