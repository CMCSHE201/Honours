using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.Rotate(new Vector3(0, 0, 0.2f));
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

