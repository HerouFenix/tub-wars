using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public GameObject player;

    public GameObject bullet;

    public Light targetArea;

    public ParticleSystem particles;

    public float timeBetweenShots = 5;

    private float timeSinceLastShot = 0;
    private Vector3 cannonForward;


    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastShot = 0;
        cannonForward = this.transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        /* For Debugging */
        /*
        if (Input.GetKeyDown("b"))
        {
            ShootAtPlayer();
        }
        */

        if (timeSinceLastShot > 0)
        {
            timeSinceLastShot -= Time.deltaTime;
        }

        // Check if player is in cone of vision
        if (checkPlayerInVision() && timeSinceLastShot <= 0)
        {
            // Shoot
            ShootAtPlayer();
            timeSinceLastShot = timeBetweenShots;

        }
    }

    bool checkPlayerInVision()
    {
        Vector3 dir = player.transform.position - this.transform.position;
        //Debug.Log(dir.magnitude);

        float angle_to_player = Vector3.Angle(this.cannonForward, dir);
        //Debug.Log(angle_to_player);
        if (dir.magnitude <= 10.0f && Mathf.Abs(angle_to_player) < 60.0f)
        {
            //Debug.Log("Player in sight");
            return true;
        }

        //Debug.Log("Player out of sight");
        return false;
    }

    void ShootAtPlayer()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 startPosition = transform.position + new Vector3(0.38f, 0.557f, 0.187f); // Make it so balloon comes out of cannon

        playerPosition += player.transform.forward * 0.1f; // Add small offset


        GameObject shot = Instantiate(bullet, startPosition, Quaternion.Euler(0f, 0f, -45.0f));
        shot.GetComponent<WaterBalloon>().enemyBalloon = true;
        shot.GetComponent<Rigidbody>().velocity = ParabolicVelocity(playerPosition);

        Light light = Instantiate(targetArea, targetArea.transform.position + playerPosition, Quaternion.Euler(90.0f, 0.0f, 0.0f));
        shot.GetComponent<WaterBalloon>().associatedLight = light;

        // Emmit particles
        particles.Play();
    }

    Vector3 ParabolicVelocity(Vector3 target)
    {
        var dir = target - transform.position; // Target Direction
        var h = dir.y; // Height Difference

        dir.y = 0; // We only need horizontal direction

        var dist = dir.magnitude; // Horizontal distance
        dir.y = dist; // Set elevation to 45 degrees
        dist += h; // Correct for difference in heights

        var velocity = Mathf.Sqrt(dist * Physics.gravity.magnitude);
        return velocity * dir.normalized;
    }
}
