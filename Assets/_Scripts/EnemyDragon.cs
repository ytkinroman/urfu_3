using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnemyDragon : MonoBehaviour
{
    public GameObject dragonEggPrefab;
    public float speed = 4;
    public float timeBetweenEggDrops = 2f;
    public float leftRightDistance = 10f;
    public float chanceDirection = 0.01f;

    public int sceneIndex;
    private bool dataApplied = false;


    private void Start ()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Invoke("DropEgg", 4f);
    }

    private void DropEgg ()
    {
        Vector3 myVector = new Vector3(0.0f, 5.0f, 0.0f);
        GameObject egg = Instantiate<GameObject>(dragonEggPrefab);
        egg.transform.position = transform.position + myVector;
        Invoke("DropEgg", timeBetweenEggDrops);
    }


    private void Update ()
    {
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        if (pos.x < -leftRightDistance) {
            speed = Mathf.Abs(speed);
        }
        else if (pos.x > leftRightDistance) {
            speed = -Mathf.Abs(speed);
        }

        if (!dataApplied && DataLoader.dataSet.ContainsKey(sceneIndex)) {
            DataLoader.DataEntry dataEntry = DataLoader.dataSet[sceneIndex];
            ApplyData(dataEntry);
            dataApplied = true;
        }
    }

    private void ApplyData (DataLoader.DataEntry dataEntry)
    {
        speed = dataEntry.Speed;
        timeBetweenEggDrops = dataEntry.TimeBetweenEggDrops;
        leftRightDistance = dataEntry.LeftRightDistance;
        chanceDirection = dataEntry.ChanceDirection;
    }

    private void FixedUpdate ()
    {
        if (Random.value < chanceDirection) {
            speed *= -1;
        }
    }
}
