using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeScript : MonoBehaviour
{
    public float timer = 0;
    public float timerDuration = 3f;

    public SoldierScript soldier;

    // Start is called before the first frame update
    void Start()
    {
        soldier = GetComponent<SoldierScript>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (soldier.hit && !soldier.moving && !soldier.crouching)
        {
            soldier.crouching = true;
            soldier.hit = false;
            ResetTimer(0);
        }
        else if (soldier.hit)
        {
            soldier.hit = false;
            ResetTimer(0);
        }

        if (soldier.health <= 20)
        {
            soldier.RushForHealth();
            ResetTimer(-3);
        }

        if (timer > timerDuration)
        {
            if (soldier.shot)
            {
                soldier.shot = false;
            }

            if (soldier.hit && !soldier.moving && !soldier.crouching)
            {
                soldier.crouching = true;
                soldier.hit = false;
                ResetTimer(0);
            }
            else if(soldier.hit)
            {
                soldier.hit = false;
                ResetTimer(0);
            }

            if (soldier.health < 50)
            {
                if (!soldier.canSeePlayer && soldier.barrierLevel > 1 && !soldier.crouching)
                {
                    soldier.MoveBack();
                    ResetTimer(-1);
                }
            }
            else
            {
                if (!soldier.moving)
                {
                    if (!soldier.crouching)
                    {
                        if (soldier.canSeePlayer)
                        {
                            soldier.ShootPlayer();
                            ResetTimer(0);
                        }
                        else if (soldier.barrierLevel < 3)
                        {
                            soldier.MoveUp();
                            ResetTimer(-1);
                        }
                    }
                    else
                    {
                        soldier.StandUp();
                        ResetTimer(1);
                    }
                }
            }
        }
    }

    public void ResetTimer(int i)
    {
        timer = i;
    }
}
