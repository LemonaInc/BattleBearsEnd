using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager : MonoBehaviour {

    //Singleton pattern
    public static UIManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    private static UIManager _instance;

    //The key in this Dictionary is a Type. 
    //Type is an interesting variable Type, e.g. when I have an instance with a Car script, the Type is 'Car'
    private Dictionary<Type, UIScreen> screens = new Dictionary<Type, UIScreen>();

    private UIScreen currentScreen;

	// Use this for initialization
	void Start () {
        //We search for all children with a UIScreen component (including the inactive ones)
		foreach(UIScreen screen in GetComponentsInChildren<UIScreen>(true))
        {
            screen.gameObject.SetActive(false);
            //We can get the Types (e.g. 'MainMenu', 'GameScreen', 'ExitPopup') from these instances
            screens.Add(screen.GetType(), screen);
        }

        Show(typeof(MainMenu));
	}

    //The chevron/angular brackets are used for Generic methods
    //The 'T' parameter refers to a type
    public void Show <T> ()
    {
        //We call the private Show method
        Show(typeof(T));
    }

    void Show (Type screenType)
    {
        if (currentScreen != null)
        {
            currentScreen.gameObject.SetActive(false);
        }

        //We use the type-parameter, as a key in our Dictionary
        UIScreen newScreen = screens[screenType];
        newScreen.gameObject.SetActive(true);
        currentScreen = newScreen;
    }
	
	
}
