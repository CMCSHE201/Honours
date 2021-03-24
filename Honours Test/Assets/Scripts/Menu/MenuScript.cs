using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.Rotate(new Vector3(0, 0, 0.2f));
    }

    public void PlayGameA()
    {
        SceneManager.LoadScene("Gameplay UAI");
    }

    public void PlayGameB()
    {
        SceneManager.LoadScene("Gameplay BT");
    }

    public void HelpMenu()
    {
        SceneManager.LoadScene("HelpMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
