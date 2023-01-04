using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticles : MonoBehaviour
{
    private Camera camera;
    private bool wasUnderwater = false;
    private bool particlesActive = false;
    public bool gameStarted = false;
    private ParticleSystem particles;
    private bool first = true;

    // Start is called before the first frame update
    void Start()
    {
        camera = this.GetComponent<Camera>();
        particles = gameObject.transform.Find("WaterDrops").GetComponent<ParticleSystem>();
        particles.Clear();
        particles.Stop();
        particles.gameObject.SetActive(false);
    }

    

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            bool underwater = false;

            if (first)
            {
                particles.gameObject.SetActive(true);
                particles.Stop();
                particles.Clear();

                first = false;
                return;
            }

            if(camera.transform.position.y < -2.1f && camera.transform.position.x > -100.0f)
            {
                underwater = true;

                particlesActive = false;
                particles.Clear();
                particles.Stop();
            }
            else
            {
                underwater = false;
            }

            if (wasUnderwater && !underwater && !particlesActive)
            {
                // Activate particles
                particles.Clear();
                particles.Play();
                particlesActive = true;
            }

            wasUnderwater = underwater;
        }
        else
        {
            particles.Stop();
            particles.Clear();
        }
    }
}
