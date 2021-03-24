using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoScript : MonoBehaviour
{
    public Text ammoText;
    public int ammoCount = 10;
    public int maxAmmo = 10;
    public float shotDelay = 1.5f;
    public float shotDelayTimer;

    public GameObject gun;
    public GameObject playerCamera;

    public GameObject[] bulletUI;

    public AudioSource gunShot;

    public Text hitText;

    public bool refillAmmo = false;
    public bool fired;

    // Start is called before the first frame update
    void Start()
    {
        gunShot = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (refillAmmo)
        {
            if (ammoCount < maxAmmo)
            {
                bulletUI[ammoCount].SetActive(true);
                ammoCount++;
            }
        }

        if (Input.GetMouseButtonDown(0) && !refillAmmo && !fired)
        {
            if (ammoCount > 0)
            {
                ammoCount--;
                bulletUI[ammoCount].SetActive(false);
                fired = true;

                gunShot.Play();

                Shoot();
            }
        }

        ammoText.text = "Ammo: " + ammoCount + "/" + maxAmmo;

        gun.transform.rotation = playerCamera.transform.rotation;

        if (fired)
        {
            if (shotDelayTimer < shotDelay)
            {
                shotDelayTimer += Time.deltaTime;
            }
            else
            {
                fired = false;
                shotDelayTimer = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ammoRefill")
        {
            refillAmmo = true;

            other.GetComponentInParent<AudioSource>().Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "ammoRefill")
        {
            refillAmmo = false;
        }
    }

    private void Shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponentInParent<SoldierScript>().takeDamage(30);
                hit.collider.gameObject.GetComponentInParent<SoldierScript>().hit = true;
            }
            else if (hit.collider.gameObject.tag == "EnemyHead")
            {
                hit.collider.gameObject.GetComponentInParent<SoldierScript>().takeDamage(80);
            }

            //hitText.text = hit.collider.gameObject.name;
        }
    }
}
