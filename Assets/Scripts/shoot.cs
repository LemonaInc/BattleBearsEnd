using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour {


	public float shootForce;

	// Use this for initialization
	void Start () {

		GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
