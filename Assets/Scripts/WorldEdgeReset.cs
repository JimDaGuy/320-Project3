using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEdgeReset : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.x <= 0)
            transform.position = new Vector3(1, transform.position.y, transform.position.z);
        else if (transform.position.x >= 1000)
            transform.position = new Vector3(999, transform.position.y, transform.position.z);
        if (transform.position.z <= 0)
            transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        else if (transform.position.z >= 1000)
            transform.position = new Vector3(transform.position.x, transform.position.y, 999);
    }
}
