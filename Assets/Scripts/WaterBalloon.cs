using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBalloon : MonoBehaviour
{
    Rigidbody rb = null;
    public Light associatedLight;
    public ParticleSystem ps;
    public bool enemyBalloon = false;

    public AudioClip[] sfx;

    // Start is called before the first frame update
    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        if (sfx.Length > 0)
            AudioManager.Instance.PlaySFX(sfx[0], 0.6f);
            
    }

    // Update is called once per frame
    void Update()
    {
        // Face the velocity direction
        var dir = rb.velocity;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        /*
        if(transform.position.y < -1.0f)
        {
            DestroyImmediate(associatedLight.gameObject);
            DestroyImmediate(this.gameObject);
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PostProcessing")
        {
            return;
        }

        if (enemyBalloon)
        {
            // Call player TAKE DAMAGE method
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerManager>().TakeDamage(30.0f);
            }else if(other.gameObject.tag == "BoatEnemy") // Dont collide with boat
            {
                return;
            }
        }

        else
        {
            if(other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<ChasingEnemy>().kill(true);
                Destroy(this.gameObject, 0f);
            }
            else if(other.gameObject.tag == "Player")
            {
                return;
            }
        }

        Instantiate(ps, this.transform.position, Quaternion.Euler(-90.0f, 0.0f, 0.0f));
        if(enemyBalloon)
            Destroy(associatedLight.gameObject, 0f);
        if(sfx.Length > 1)
            AudioManager.Instance.PlaySFX(sfx[1], 0.4f);
        Destroy(this.gameObject, 0f);
    }
}
