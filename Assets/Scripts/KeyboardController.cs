using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {
    
    [SerializeField]
    private GameObject pauseMenu;

    void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            State.paused = !State.paused;
        }
        pauseMenu.gameObject.SetActive(State.paused);
    }
}
