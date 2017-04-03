using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPopup : UIScreen
{
    //This runs every time this script/gameObject gets enabled/activated
    void OnEnable ()
    {
        Time.timeScale = 0;
    }

    //This runs every time this script/gameObject gets disabled/deactivated
    void OnDisable ()
    {
        Time.timeScale = 1;
    }

	public void OnYesButton ()
    {
        UIManager.instance.Show<MainMenu>();
        //We unload the game
        //Asynchronous means that it's unloaded through a different thread
        SceneManager.UnloadSceneAsync("GameScene");
    }

    public void OnNoButton ()
    {
        UIManager.instance.Show<GameScreen>();
    }
}
