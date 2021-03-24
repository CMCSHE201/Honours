using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public float health = 100;
    public float hitTimer = 0;

    public Slider healthSlider;

    public bool healthRefill;
    public bool crouching = false;
    public bool hit = false;

    public GameObject deathText;
    public GameObject healthFillArea;

    public GameObject[] hitOutline;

    public Vector3 respawnLocation;
    // Start is called before the first frame update
    void Start()
    {
        respawnLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health;

        if (health <= 0)
        {
            SceneManager.LoadScene("MainMenu");
            //ResetPlayer();
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            crouching = true;
        }
        else
        {
            crouching = false;
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (healthRefill)
        {
            if (health < 100)
            {
                health++;
            }
        }

        if (hit)
        {
            hitTimer += Time.deltaTime;

            if (hitTimer > 0.5f)
            {
                hit = false;
            }

            for (int i = 0; i < hitOutline.Length; i++)
            {
                hitOutline[i].SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < hitOutline.Length; i++)
            {
                hitOutline[i].SetActive(false);
            }
        }
    }

    public void HitByEnemy()
    {
        health -= 10;
        hit = true;
        hitTimer = 0;
    }

    public void healPlayer()
    {
        health = 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HealthRefill")
        {
            healthRefill = true;

            other.GetComponentInParent<AudioSource>().Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HealthRefill")
        {
            healthRefill = false;
        }
    }

    private void ResetPlayer()
    {
        health = 100;
        transform.position = respawnLocation;
    }
}
