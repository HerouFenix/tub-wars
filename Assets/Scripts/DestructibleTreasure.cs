using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleTreasure : MonoBehaviour
{
    public ParticleSystem ps;
    public TreasureSpawner spawner;
    public int balloonsPerChest = 3;
    public AudioClip[] sfx;

    /* For Debugging */
    /*
    private void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            kill();
        }
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        // Call player TAKE DAMAGE method
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerManager>().incrementBalloons(balloonsPerChest);
            kill();
        }
    }

    private void kill()
    {
        AudioManager.Instance.PlaySFX(sfx[0], 0.3f);
        Instantiate(ps, this.transform.position, Quaternion.Euler(-90.0f, 0.0f, 0.0f));

        if(spawner != null)
        {
            spawner.ChestDestroyed();
        }

        Destroy(this.gameObject);
    }
}
