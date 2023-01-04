using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float maxhealth = 100f;
    [SerializeField] private float health = 100f;
    public float score = 0000;
    public bool dead = false;
    public Vector3 startPos;

    public int balloons = 0;

    private int heartCount = 3;
    public string _tag = "";

    public GameObject manager;
    private GameManager gm;

    private void Start()
    {
        gm = manager.GetComponent<GameManager>();
        startPos = transform.position;
        health = maxhealth;
        balloons = 0;
    }

    public void TakeDamage(float damage)
    {
        if (!dead)
        {
            health -= damage;
            if (health >= (maxhealth / 3) && health <= (2 * maxhealth / 3) && heartCount == 3)
            {
                gm.SubHeart();
                heartCount--;
            }
            else if (health > 0 && health <= (maxhealth / 3) && heartCount == 2)
            {
                gm.SubHeart();
                heartCount--;
            }
            else if (health <= 0)
            {
                heartCount--;
                GetComponent<SaveScore>().RecordScore(score, _tag);
                gm.EndGame();
                health = maxhealth;
                heartCount = 3;
                dead = true;
            }
        }
    }

    public void increaseScore(float score)
    {
        this.score += score;
        if(gm != null)
            gm.SetScore();
    }

    public void incrementBalloons(int number)
    {
        balloons += number;
        gm.SetBaloons();
    }

    public void decrementBaloons()
    {
        balloons -= 1;
        gm.SetBaloons();
    }

}
