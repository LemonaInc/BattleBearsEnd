using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //This is the Singleton design pattern
    //A singleton is a script, with a static variable, that refers to an instance of that script in the Scene
    //We HAVE TO MAKE SURE, there is ONLY ONE instance of this type in our Scene
    public static GameManager instance
    {
        get
        {
            //Only the first time anyone 'gets' instance, we search for it. After that _instance is no longer null and we simply return it
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    private static GameManager _instance;

    //The unit gets their teamcolor by using the team as an index in the array
    public Color[] teamColors;

    //internal variables are accessible by other classes, but not visible in the Unity inspector
    internal Outpost[] outposts;



	// Use this for initialization
	void Awake () {
        //We assign this variable in Awake, because otherwise the 'Start' in the AIController might be called before the 'Start' in the GameManager
        //Awake is always called before any Start method is called.
        outposts = FindObjectsOfType<Outpost>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
