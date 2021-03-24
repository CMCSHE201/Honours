using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTGameManager : MonoBehaviour
{
    public GameObject soldierPrefab;

    private Vector3 soldierSpawnLoc = new Vector3(18.8f, 0.1f, -21.5f);

    private GameObject[] soldiers;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        soldiers = GameObject.FindGameObjectsWithTag("BT");

        if (soldiers.Length < 5)
        {
            Instantiate(soldierPrefab, soldierSpawnLoc, Quaternion.identity);
        }
    }
}
