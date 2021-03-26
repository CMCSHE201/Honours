using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierScript : MonoBehaviour
{
    public Animator animator;

    public float health = 100;

    public float moveTimer = 0;

    public GameObject[] barriers;

    public GameObject[] barriers1;
    public GameObject[] barriers2;
    public GameObject[] barriers3;

    public GameObject[] colliders;

    public Vector3 startLocation = new Vector3(0, 0, 0);
    public Vector3 movedLocation = new Vector3(0, 0, 0);
    public Vector3 stoppedLocation = new Vector3(0, 0, 0);
    public Vector3 playerLastPos = new Vector3(0, 0, 0);
    private Vector3 medLocation = new Vector3(20, 0, -20);

    public GameObject playerObject;

    public bool crouching;
    public bool canCrouch;
    public bool moving = false;

    public bool canSeePlayer;

    public bool shot = false;
    public bool stand = false;
    public bool crouch = false;

    public bool hit = false;
    public bool moved = false;

    private int barrier;
    private int barrierChoice;

    public int barrierLevel = 0;

    public ParticleSystem muzzleFlash;

    private NavMeshAgent myNavMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        barriers = GameObject.FindGameObjectsWithTag("EnemyBarrier");
        playerObject = GameObject.FindGameObjectWithTag("Player");

        stoppedLocation = transform.position;
        startLocation = transform.position;

        myNavMeshAgent = GetComponent<NavMeshAgent>();

        myNavMeshAgent.Warp(startLocation);

        for (int i = 0; i < 3; i++)
        {
            barriers1[i] = barriers[i];
            barriers2[i] = barriers[3 + i];
            barriers3[i] = barriers[6 + i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        Look();

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (Vector3.Distance(transform.position, medLocation) < 5)
        {
            health = 100;
        }

        if (moved)
        {
            moveTimer += Time.deltaTime;

            if (moveTimer > 5)
            {
                moved = false;
            }
        }
        else
        {
            moveTimer = 0;
        }

        for (int i = 0; i < barriers.Length; i++)
        {
            if (transform.position.z - barriers[i].transform.position.z > -3 && transform.position.z - barriers[i].transform.position.z < -0.5f)
            {
                if (transform.position.x - barriers[i].transform.position.x < 1 && transform.position.x - barriers[i].transform.position.x > -1)
                {
                    canCrouch = true;
                    barrier = i;
                }
            }
        }

        if (crouching)
        {
            animator.SetBool("Crouch", true);
            colliders[0].SetActive(false);
            colliders[1].SetActive(false);
            colliders[2].SetActive(true);
            colliders[3].SetActive(true);
            colliders[4].SetActive(false);
            colliders[5].SetActive(false);
        }
        else if (moving)
        {
            animator.SetBool("Crouch", false);
            colliders[0].SetActive(false);
            colliders[1].SetActive(false);
            colliders[2].SetActive(false);
            colliders[3].SetActive(false);
            colliders[4].SetActive(true);
            colliders[5].SetActive(true);
        }
        else
        {
            animator.SetBool("Crouch", false);
            colliders[0].SetActive(true);
            colliders[1].SetActive(true);
            colliders[2].SetActive(false);
            colliders[3].SetActive(false);
            colliders[4].SetActive(false);
            colliders[5].SetActive(false);
        }

        Vector3 soldierHeadPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);

        Ray ray = new Ray(soldierHeadPos, playerObject.transform.position - soldierHeadPos);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "Player")
            {
                canSeePlayer = true;
                playerLastPos = playerObject.transform.position;
            }
            else if (hit.transform.tag != "Wall")
            {
                if (!playerObject.GetComponent<PlayerScript>().crouching)
                {
                    canSeePlayer = true;
                    playerLastPos = playerObject.transform.position;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }

            if (playerObject.transform.position.x > 18 && playerObject.transform.position.z > 15)
            {
                canSeePlayer = false;
            }
            else if (playerObject.transform.position.x < -17 && playerObject.transform.position.z > 15)
            {
                canSeePlayer = false;
            }

        }

        if (moving)
        {
            animator.SetBool("Run", true);

            crouching = false;

            myNavMeshAgent.SetDestination(movedLocation);
            myNavMeshAgent.isStopped = false;

            if (Vector3.Distance(transform.position, movedLocation) < 1)
            {
                animator.SetBool("Run", false);
                moving = false;

                myNavMeshAgent.isStopped = true;

                stoppedLocation = transform.position;
            }
        }
        else
        {
            if (movedLocation != new Vector3(0, 0, 0))
            {
                transform.position = movedLocation;
            }
        }
    }

    private void Look()
    {
        if (canSeePlayer && !crouching && !moving)
        {
            transform.LookAt(new Vector3(playerObject.transform.position.x, transform.position.y, playerObject.transform.position.z));
        }
        else if (!canSeePlayer && !crouching && !moving)
        {
            transform.LookAt(playerLastPos);
        }

        if (crouching)
        {
            transform.LookAt(barriers[barrier].transform);
            transform.position = stoppedLocation;
        }
    }

    private void Move()
    {
        barrierChoice = Random.Range(0, barriers.Length);

        movedLocation = new Vector3(barriers[barrierChoice].transform.position.x, barriers[barrierChoice].transform.position.y, barriers[barrierChoice].transform.position.z - 2);

        moving = true;
    }

    public void takeDamage(int i)
    {
        health -= i;
    }

    public void MoveUp()
    {
        if (barrierLevel < 3)
        {
            barrierChoice = Random.Range(0, 2);

            switch (barrierLevel)
            {
                case 0:

                    Vector2 locationXY = new Vector2(barriers1[barrierChoice].transform.position.x, barriers1[barrierChoice].transform.position.y) + Random.insideUnitCircle * 2;

                    movedLocation = new Vector3(locationXY.x, 0, barriers1[barrierChoice].transform.position.z - 3);
                    break;

                case 1:

                    locationXY = new Vector2(barriers2[barrierChoice].transform.position.x, barriers2[barrierChoice].transform.position.y) + Random.insideUnitCircle * 2;

                    movedLocation = new Vector3(locationXY.x, 0, barriers2[barrierChoice].transform.position.z - 3);
                    break;

                case 2:

                    locationXY = new Vector2(barriers3[barrierChoice].transform.position.x, barriers3[barrierChoice].transform.position.y) + Random.insideUnitCircle * 2;

                    movedLocation = new Vector3(locationXY.x, 0, barriers3[barrierChoice].transform.position.z - 3);
                    break;
            }

            barrierLevel++;

            moving = true;
        }
    }

    public void MoveBack()
    {
        if (barrierLevel > 1)
        {
            barrierChoice = Random.Range(0, 2);

            switch (barrierLevel)
            {
                case 2:

                    Vector2 locationXY = new Vector2(barriers1[barrierChoice].transform.position.x, barriers1[barrierChoice].transform.position.y) + Random.insideUnitCircle * 2;

                    movedLocation = new Vector3(locationXY.x, 0, barriers1[barrierChoice].transform.position.z - 3);
                    break;

                case 3:

                    locationXY = new Vector2(barriers2[barrierChoice].transform.position.x, barriers2[barrierChoice].transform.position.y) + Random.insideUnitCircle * 2;

                    movedLocation = new Vector3(locationXY.x, 0, barriers2[barrierChoice].transform.position.z - 3);
                    break;
            }

            barrierLevel--;

            moving = true;
        }
    }

    public void RushForHealth()
    {
        Vector3 medLoc = medLocation + Random.insideUnitSphere * 2;

        movedLocation = new Vector3(medLoc.x, 0, medLoc.z);

        barrierLevel = 0;

        moving = true;
    }

    public void ShootPlayer()
    {
        float hitPlayer = Random.Range(1, 100);

        GetComponent<AudioSource>().Play();
        muzzleFlash.Play();

        if (hitPlayer < (50 - Vector3.Distance(transform.position, playerObject.transform.position)))
        {
            playerObject.GetComponent<PlayerScript>().HitByEnemy();
        }

        shot = true;
    }

    public void StandUp()
    {
        crouching = false;
        stand = true;
    }

    public void Crouch()
    {
        crouching = true;
        crouch = true;
    }

    public void ResetEnemy()
    {
        movedLocation = new Vector3(0, 0, 0);
        transform.position = startLocation;
        stoppedLocation = startLocation;
        barrierLevel = 0;
        hit = false;
        moved = false;
        moving = false;
        crouching = false;
        health = 100;
    }

}
