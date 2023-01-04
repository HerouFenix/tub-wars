using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGun : MonoBehaviour
{

    public GameObject projectiles;
    public GameObject madeProjectile;
    public GameObject waterBaloon;
    public GameObject previewLine;
    public PlayerMovement movement;
    public bool isShooting;
    public bool isPreparing;
    public float force;

    public float water_fill = 100.0f;
    public float decrease_rate = 0.1f;
    public float increase_rate = 0.05f;
    private bool overflown = false;

    private bool mouse_0_pressed = false;
    private bool mouse_1_pressed = false;

    public AudioClip[] sfx;

    public BarController waterBar;

    // Start is called before the first frame update
    void Start()
    {
        madeProjectile = Instantiate(projectiles, transform.position, transform.rotation);
        //madeProjectile.SetActive(false);
        madeProjectile.GetComponent<ParticleSystem>().Stop(true);
        isShooting = false;
        previewLine.GetComponent<LineRenderer>().enabled = false;
        movement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        waterBar.SetMaxValue(water_fill);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            mouse_0_pressed = false;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            mouse_0_pressed = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            mouse_1_pressed = false;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().balloons > 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>().decrementBaloons();
                mouse_1_pressed = true;
            }
        }

        // If not preparing water balloon and pressed to shoot enable particles if enough water fill
        if (!isPreparing && mouse_0_pressed && !overflown)
        {
            if (water_fill > 9 && !overflown)
            {               
                //madeProjectile.SetActive(true);
                madeProjectile.GetComponent<ParticleSystem>().Play();
                madeProjectile.GetComponent<ParticleSystem>().startSpeed = 7;
                isShooting = true;
                GetComponent<AudioSource>().volume = 0.5f;
                if (movement.isMoving)
                    madeProjectile.GetComponent<ParticleSystem>().startSpeed = 7 + movement.speed;
            }
        }
        
        // Stop Shooting
        if(!mouse_0_pressed || overflown)
        {
            //madeProjectile.SetActive(false);
            madeProjectile.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            isShooting = false;
            GetComponent<AudioSource>().volume = 0.0f;
        }

        // If shooting enabled, decrease fill and make projectile
        if (isShooting && water_fill>9 && !overflown)
        {
            decreaseFill(decrease_rate * Time.deltaTime);
            if(water_fill <= 9)
            {
                overflown = true;
                isShooting = false;
                GetComponent<AudioSource>().volume = 0.0f;
            }
            else
            {
                madeProjectile.transform.position = transform.position;
                madeProjectile.transform.rotation = transform.rotation;
            }
        }
        else if (water_fill < 100)
        {
            increaseFill(increase_rate * Time.deltaTime);
            if (water_fill >= 31) {
                overflown = false;
            }
        }


        // If not shooting and pressed aim button start aiming
        if (!isShooting && mouse_1_pressed)
        {
            isPreparing = true;
            previewLine.GetComponent<LineRenderer>().enabled = true;
        }
        if (isPreparing && !mouse_1_pressed)
        {
            AudioManager.Instance.PlaySFX(sfx[(int)Random.Range(0.0f,3.0f)],0.5f);
            GameObject temp = Instantiate(waterBaloon, transform.position, transform.rotation);
            temp.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
            //if (movement.isMoving)
            temp.GetComponent<Rigidbody>().AddForce(transform.forward * movement.speed, ForceMode.Impulse);
            temp.GetComponent<Rigidbody>().AddForce(transform.up * force, ForceMode.Impulse);
            isPreparing = false;
            previewLine.GetComponent<LineRenderer>().enabled = false;
        }
    }


    public void decreaseFill(float dmg)
    {
        water_fill -= dmg;
        if (waterBar != null)
        {
            waterBar.SetSlider(water_fill);
        }
    }

    public void increaseFill(float inc)
    {
        water_fill += inc;
        if (waterBar != null)
        {
            waterBar.SetSlider(water_fill);
        }
    }
}
