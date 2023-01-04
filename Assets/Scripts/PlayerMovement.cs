using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //public CharacterController controller;
    public Rigidbody rb;
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public bool isMoving;

    public ParticleSystem movingPS;
    bool psOn = false;
    public bool canMove = false;

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0.0f, vertical).normalized;
            //Debug.Log(direction.magnitude);

            //rb.velocity = new Vector3(horizontal, rb.velocity.y, vertical) * speed;
            if (direction.magnitude >= 0.1f)
            {
                isMoving = true;
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                //rb.velocity = moveDir.normalized * speed * Time.deltaTime;
                rb.velocity = moveDir.normalized * speed;
            }
            else isMoving = false;

            if (!isMoving)
            {
                movingPS.Stop();
            }else if (movingPS.isStopped)
            {
                movingPS.Play();
            }
        }
    }
}
