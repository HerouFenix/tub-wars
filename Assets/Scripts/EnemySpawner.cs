using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public GameObject[] spawnPoints;

    private int numEnemies = 0;
    public int minNumEnemies = 9;

    public ParticleSystem ps;
    public bool active = false;
    public bool initial = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (active)
        {
            if (initial)
            {
                foreach (var point in spawnPoints)
                {
                    var obj = Instantiate(enemy, point.transform.position, point.transform.rotation);
                    var script = obj.GetComponent<ChasingEnemy>();
                    script.spawner = this;
                    script.player = player;

                    //Instantiate(ps, this.transform.position, Quaternion.Euler(-90.0f, 0.0f, 0.0f));

                    numEnemies++;
                }

                initial = false;
            }
            else
            {
                numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length; // Idky but this works but numenemies++ and -- doesn.t :/

                if (numEnemies < minNumEnemies)
                {
                    int randIndex = Random.Range(0, spawnPoints.Length - 1);
                    var obj = Instantiate(enemy, spawnPoints[randIndex].transform.position, spawnPoints[randIndex].transform.rotation);
                    var script = obj.GetComponent<ChasingEnemy>();
                    script.spawner = this;
                    script.player = player;

                    Instantiate(ps, this.transform.position, Quaternion.Euler(-90.0f, 0.0f, 0.0f));
                    numEnemies++;
                }
            }
        }
    }

    public void deleteAllEnemies()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);

            //Instantiate(ps, this.transform.position, Quaternion.Euler(-90.0f, 0.0f, 0.0f));

            numEnemies = 0;
        }
    }

    public void spawnDisplayEnemies()
    {
        foreach (var point in spawnPoints)
        {
            var obj = Instantiate(enemy, point.transform.position, point.transform.rotation);
            var script = obj.GetComponent<ChasingEnemy>();
            script.spawner = this;
            script.player = player;

        }
    }

    public void enemyDied()
    {
        //numEnemies -= 1;
    }

}
