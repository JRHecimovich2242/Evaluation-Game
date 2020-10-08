using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private bool _paused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }   
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        GetComponent<Canvas>().enabled = true;
    }

    public void Resume()
    {
        GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1;

    }
}
