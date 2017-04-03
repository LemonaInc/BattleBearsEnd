using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	private float lifeTime = 0.05f;

	public void Init (Vector3 startPos, Vector3 endPos)
	{
		LineRenderer line = GetComponent<LineRenderer>();
		line.SetPosition(0, startPos);
		line.SetPosition(1, endPos);
		//Destroys this gameobject after a certain delay
		Destroy(gameObject, lifeTime);
	}
}
