using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureSpawner : MonoBehaviour
{
    public float timeBetweenSpawns = 20.0f;
    public float timeTillSpawn = 0.0f;

    private bool respawning = false;
    public GameObject treasure;

    public ParticleSystem ps;


    // Start is called before the first frame update
    void Start()
    {
        timeTillSpawn = 0;
        respawning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (respawning)
        {
            if(timeTillSpawn > 0)
            {
                timeTillSpawn -= Time.deltaTime;
            }
            else
            {
                // Spawn chest
                var obj = Instantiate(treasure, transform.position, transform.rotation);
                obj.GetComponent<DestructibleTreasure>().spawner = this;

                Instantiate(ps, this.transform.position, Quaternion.Euler(-90.0f, 0.0f, 0.0f));

                respawning = false;
                timeTillSpawn = timeBetweenSpawns;
            }
        }
    }

    public void ChestDestroyed()
    {
        respawning = true;
    }
}
