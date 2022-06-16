using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseActions : MonoBehaviour {
    
    public void resume() {
        State.paused = false;
        Debug.Log("resume");
    }

    public void quit() {
        Application.Quit();
        Debug.Log("quit");
    }
}
