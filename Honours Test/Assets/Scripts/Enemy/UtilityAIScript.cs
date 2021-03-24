using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityAIScript : MonoBehaviour
{
    public float shotTimer = 0;
    public float standTimer = 0;
    public float crouchTimer = 0;

    public float[] weights = new float[6];

    public int choice = 0; //0 - stand, 1 - crouch, 2 - move forward, 3 - move back, 4 - shoot, 5 - rush for health

    public SoldierScript soldier;

    public GameObject playerObject;

    private bool canPerform = true;

    void Start()
    {
        soldier = GetComponent<SoldierScript>();
        playerObject = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < weights.Length; i++)
        {
            weights[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (soldier.shot)
        {
            shotTimer += Time.deltaTime;
            canPerform = false;
        }

        if (soldier.stand)
        {
            standTimer += Time.deltaTime;
            canPerform = false;
        }

        if (soldier.crouch)
        {
            crouchTimer += Time.deltaTime;
            canPerform = false;
        }

        weights[0] = StandWeight();
        weights[1] = CrouchWeight();
        weights[2] = MoveFWeight();
        weights[3] = MoveBWeight();
        weights[4] = ShootWeight();
        weights[5] = HealthWeight();

        SortChoice();

        if (soldier.shot || soldier.stand || soldier.crouch)
        {
            if (shotTimer > 3)
            {
                soldier.shot = false;
                shotTimer = 0;
                canPerform = true;
            }
            else if (standTimer > 2)
            {
                soldier.stand = false;
                standTimer = 0;
                canPerform = true;
            }
            else if (crouchTimer > 3)
            {
                soldier.crouch = false;
                crouchTimer = 0;
                canPerform = true;
            }
        }

        if (!soldier.moving && canPerform)
        {
            PerformAction();
        }
    }

    public float StandWeight()
    {
        float weight = 0;

        if (!soldier.crouching)
        {
            return 0;
        }
        else
        {
            weight += soldier.health;

            weight += Vector3.Distance(playerObject.transform.position, transform.position);

            weight += 100 - playerObject.GetComponent<PlayerScript>().health;

            return weight;
        }
    }

    public float CrouchWeight()
    {
        float weight = 0;

        if (!soldier.canCrouch || soldier.crouching)
        {
            return 0;
        }
        else
        {
            if (soldier.canSeePlayer)
            {
                weight += 50 - Vector3.Distance(playerObject.transform.position, transform.position);
            }

            if (soldier.hit)
            {
                weight += 75;
                soldier.hit = false;
            }

            weight += 75 - soldier.health;

            weight += playerObject.GetComponent<PlayerScript>().health;

            return weight;
        }
    }

    public float MoveFWeight()
    {
        float weight = 0;

        if (soldier.barrierLevel == 3 || soldier.crouching || soldier.moved)
        {
            return 0;
        }
        else
        {
            weight += soldier.health;

            weight += Vector3.Distance(playerObject.transform.position, transform.position);

            if (!soldier.canSeePlayer)
            {
                weight += 30;
            }

            weight += 100 - playerObject.GetComponent<PlayerScript>().health;

            return weight;
        }
    }

    public float MoveBWeight()
    {
        float weight = 0;

        if (soldier.barrierLevel < 2 || soldier.crouching || soldier.moved)
        {
            return 0;
        }
        else
        {
            weight += 100 - soldier.health;

            weight += 50 - Vector3.Distance(playerObject.transform.position, transform.position);

            if (!soldier.canSeePlayer)
            {
                weight += 50;
            }

            weight += playerObject.GetComponent<PlayerScript>().health;

            return weight;
        }

    }

    public float ShootWeight()
    {
        float weight = 0;

        if (!soldier.canSeePlayer || soldier.crouching)
        {
            return 0;
        }
        else
        {
            weight += soldier.health;

            weight += 100 - Vector3.Distance(playerObject.transform.position, transform.position);

            weight += 100 - playerObject.GetComponent<PlayerScript>().health;

            return weight;
        }
    }

    public float HealthWeight()
    {
        float weight = 0;

        if (soldier.health == 100 || soldier.barrierLevel == 0)
        {
            return 0;
        }
        else
        {
            weight += 100 - soldier.health;

            if (!soldier.canSeePlayer)
            {
                weight += 50;
            }

            weight += Vector3.Distance(playerObject.transform.position, transform.position);

            return weight;
        }
    }

    public void SortChoice()
    {
        choice = 0;

        for (int i = 1; i < weights.Length; i++)
        {
            if (weights[i] > weights[choice])
            {
                choice = i;
            }
        }
    }

    public void PerformAction()
    {
        switch (choice)
        {
            case 0:

                soldier.StandUp();
                break;

            case 1:

                soldier.Crouch();
                break;

            case 2:

                soldier.MoveUp();
                soldier.moved = true;
                break;

            case 3:

                soldier.MoveBack();
                soldier.moved = true;
                break;

            case 4:

                soldier.ShootPlayer();
                break;

            case 5:

                soldier.RushForHealth();
                break;
        }
    }

    /*public void ResetTimer(int i)
    {
        timer = i;
    }*/
}
