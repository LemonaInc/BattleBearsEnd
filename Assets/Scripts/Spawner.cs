using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public int team;
    public AIController unitPrefab;
    public float spawnInterval;

	// Use this for initialization
	void Start () {
        //We call SpawnUnit after 0 seconds, and then again every 'spawnInterval' seconds
        InvokeRepeating("SpawnUnit", 0, spawnInterval);
	}
	
	void SpawnUnit ()
    {
        AIController unitClone = Instantiate(unitPrefab, transform.position, transform.rotation);
        unitClone.team = team;
    }
}
