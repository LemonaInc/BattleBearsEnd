using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : UIScreen {

	// Update is called once per frame
	void Update () {
        //We lock and hide our cursor
        Cursor.lockState = CursorLockMode.Locked;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            //We enable the Cursor for the exitpopup
            Cursor.lockState = CursorLockMode.None;
            UIManager.instance.Show<ExitPopup>();
        }
	}
}
