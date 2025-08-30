using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //LookatCamera();

    }

    public void LookatCamera()
    {
        //transform.LookAt(Camera.main.transform);
        transform.LookAt(new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z));
    }
}
