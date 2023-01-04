using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MoveCamera : MonoBehaviour
{

    public bool camEnabled = false;
    public float flySpeed = 10.0f;
    Rigidbody m_Rigidbody;
    public float sensitivity = 5f;

    public float yaw = 0.0f;
    public float pitch = 0.0f;

    
    private float hitDistance;
    private Ray raycast;
    private RaycastHit hit;
    [SerializeField] GameObject volume;
    DepthOfField dof;

    public float focusSpeed = 1.0f;
    public float maxFocusDistance = 20.0f;

    private bool isHit;
    public AudioClip sfx;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        volume.GetComponent<Volume>().profile.TryGet<DepthOfField>(out dof);
    }

    // Update is called once per frame
    void Update()
    {
        if (camEnabled)
        {
            if (Input.GetKey(KeyCode.W))
            {
                //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
                m_Rigidbody.velocity = transform.forward * flySpeed;
            }
            

            if (Input.GetKey(KeyCode.S))
            {
                //Move the Rigidbody backwards constantly at the speed you define (the blue arrow axis in Scene view)
                m_Rigidbody.velocity = -transform.forward * flySpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                //Rotate the sprite about the Y axis in the positive direction
                m_Rigidbody.velocity = transform.right * flySpeed;
            }

            if (Input.GetKey(KeyCode.A))
            {
                //Rotate the sprite about the Y axis in the negative direction
                m_Rigidbody.velocity = -transform.right * flySpeed;
            }

            if (Input.GetKey(KeyCode.C))
            {
                DateTime theTime = DateTime.Now;
                string dateTime = theTime.ToString("yyyy-MM-dd-HH-mm-ss");

                ScreenCapture.CaptureScreenshot("pictures/" + dateTime + ".png");
                AudioManager.Instance.PlaySFX(sfx, 0.4f);
            }

            yaw += 2.0f * Input.GetAxis("Mouse X");
            pitch -= 2.0f * Input.GetAxis("Mouse Y");

            

            transform.rotation = Quaternion.Euler(pitch,yaw, 0.0f);
            //transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivity, 0);
            //transform.Rotate(-Input.GetAxis("Mouse Y") * sensitivity, 0, 0);
            //Debug.Log(transform.forward);

            // Dynamic DoF
            raycast = new Ray(transform.position, transform.forward * maxFocusDistance);
            isHit = false;

            if(Physics.Raycast(raycast, out hit, maxFocusDistance))
            {
                isHit = true;
                hitDistance = Vector3.Distance(transform.position, hit.point);
            }
            else
            {
                if(hitDistance < maxFocusDistance)
                {
                    hitDistance++;
                }
            }

            SetFocus();
        }
    }

    private void OnDrawGizmos()
    {
        if (isHit)
        {
            Gizmos.DrawSphere(hit.point, 0.1f);

            Debug.DrawRay(transform.position, transform.forward * Vector3.Distance(transform.position, hit.point));
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * 100f);
        }
    }

    void SetFocus()
    {
        dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, hitDistance, Time.deltaTime * focusSpeed);
    }

    public void ResetFocus()
    {
        dof.focusDistance.value = 1.74f;
    }

}
