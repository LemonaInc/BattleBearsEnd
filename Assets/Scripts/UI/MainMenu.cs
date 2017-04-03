using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : UIScreen
{
    public void OnStartGameButton ()
    {
        //I can show a different screen, without having a direct reference to the instance of that screen
        //By simply providing the type of screen, the UIManager can enable the instance of that type
        UIManager.instance.Show<GameScreen>();
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
    }
}
