using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysLookAtCamera : MonoBehaviour 
{
	public Camera aRCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(new Vector3(aRCamera.transform.position.x, transform.position.y, aRCamera.transform.position.z));
    }
    
}
