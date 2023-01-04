using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleScore : MonoBehaviour
{
    public float effectDuration = 3.0f;
    private float currentTime = 0.0f;
    private Vector3 currScale = Vector3.zero;
    private Vector3 maxScale = new Vector3(1.5f, 1.5f, 1.5f);
    private bool increasing = true;

    private void Start()
    {
        currScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= effectDuration / 2)
        {
            increasing = !increasing;
            currentTime = 0.0f;
        }
        if (currentTime <= effectDuration / 2 && increasing)
        {
           currScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), maxScale, (currentTime / (effectDuration / 2)));
           transform.localScale = currScale;
        }
        else if(currentTime <= effectDuration / 2 && !increasing)
        {
            currScale = Vector3.Lerp(maxScale, new Vector3(1f, 1f, 1f), (currentTime / (effectDuration / 2)));
            transform.localScale = currScale;
        }
    }
}
