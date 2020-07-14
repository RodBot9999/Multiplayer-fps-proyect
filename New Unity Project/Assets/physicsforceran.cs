using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class physicsforceran : MonoBehaviour, IpooledObject
{
    public float upForce = 1f;
    public float sideforce = .1f;
    // Start is called before the first frame update
    public void OnObjectSpawn()
    {
        float xForce = Random.Range(-sideforce, sideforce);
        float yForce = Random.Range(upForce / 2f, upForce);
        float zForce = Random.Range(-sideforce, sideforce);

        Vector3 force = new Vector3(xForce, yForce, zForce);

        GetComponent<Rigidbody>().velocity = force;
    }

    
}
