using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ChasingEnemy : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;

    public GameObject quack;

    public float HP = 100;

    public float timeBetweenQuacks = 5;
    public float timeQuackOnScreen = 2;

    public float chaseRadius = 20.0f;

    private float quackTime = 0;
    private bool quacking = false;

    public ParticleSystem ps;

    public BarController healthBar;

    public EnemySpawner spawner;

    public AudioClip[] sfx;

    public ParticleSystem movingPS;

    // Start is called before the first frame update
    void Start()
    {
        quackTime = timeBetweenQuacks;
        quack = this.transform.GetChild(1).gameObject;

        healthBar.SetMaxValue(HP);
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.speed < 3.5f)
        {
            agent.speed += Time.deltaTime * 0.25f;
            if(agent.speed > 3.5f)
            {
                agent.speed = 3.5f;
            }
        }

        // Display QUACK! message on top of agent
        if (quacking)
        {
            if (quackTime > 0)
            {
                quackTime -= Time.deltaTime;
            }
            else
            {
                quack.SetActive(false);
                quackTime = timeBetweenQuacks;
                quacking = false;
            }
        }
        else
        {
            if (quackTime > 0)
            {
                quackTime -= Time.deltaTime;
            }
            else
            {
                quack.SetActive(true);
                quackTime = timeQuackOnScreen;
                quacking = true;
                AudioManager.Instance.PlaySFX(sfx[(int)Random.Range(0.0f, 3.0f)], 0.15f);
            }
        }

        // Check if player is within radius, if so move to it, else move randomly

        // Move Agent
        if (checkPlayerInRadius())
        {
            agent.SetDestination(player.transform.position);
        }
        else
        { // Wander randomly
            if ((agent.remainingDistance != Mathf.Infinity && agent.remainingDistance <= 0.5) || agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                agent.SetDestination(transform.position + new Vector3(Random.Range(-chaseRadius, chaseRadius), 0, Random.Range(-chaseRadius, chaseRadius))); // Random point in radius
            }
        }

        if (agent.isStopped)
        {
            movingPS.Stop();
        }else if (movingPS.isStopped)
        {
            movingPS.Play();
        }
    }

    bool checkPlayerInRadius()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        //Debug.Log(dir.magnitude);

        if (dir.magnitude <= chaseRadius)
        {
            //Debug.Log("Player in sight");
            return true;
        }

        //Debug.Log("Player out of sight");
        return false;
    }

    /* Use this to make it so the agent stops after hitting the player */
    
    public void OnCollisionEnter(Collision collision)
    {
        // Call player TAKE DAMAGE method
        if(collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerManager>().TakeDamage(30.0f);
            healthBar.SetSlider(0);
            kill();
        }
       
    }

    public void takeDamage(float dmg)
    {
        HP -= dmg;
        healthBar.SetSlider(HP);

        if (agent.speed > 0.25f)
        {
            agent.speed -= 0.25f;
            if (agent.speed < 0.25f)
            {
                agent.speed = 0.25f;
            }
        }

        if (HP <= 0)
        {
            kill(true);
        }
    }

    public void kill(bool award = false)
    {
        if (award)
        {
            player.GetComponent<PlayerManager>().increaseScore(100);
        }

        spawner.enemyDied();
        

        AudioManager.Instance.PlaySFX(sfx[3], 0.4f);

        Instantiate(ps, this.transform.position + new Vector3(0, 0.25f, 0), Quaternion.Euler(-90.0f, 0.0f, 0.0f));
        Destroy(this.gameObject);
    }
    
}
