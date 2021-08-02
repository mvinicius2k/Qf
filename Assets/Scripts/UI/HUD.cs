using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    [HideInInspector]
    public bool paused;
    public Canvas canvasHUD;

    public void Pause()
    {
        if(canvasHUD != null)
        { 
            paused = true;
            canvasHUD.gameObject.SetActive(true);
        }
        

    }

    public void Resume()
    {
        if (canvasHUD != null)
        {
            paused = false;
            canvasHUD.gameObject.SetActive(false);
        }
    }

    public void Restart()
    {
        Debug.Log("Reiniciando");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (!paused)
                Pause();
            else
                Resume();
        }
    }
}
