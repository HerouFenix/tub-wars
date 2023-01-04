using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trajectoryController : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Line renderer variables")]
    public LineRenderer line;
    public int resolution;

    [Header("Formula variables")]
    //public Vector3 velocity;
    public float yLimit;
    public float g;
    public float value;


    private IEnumerator RenderArc()
    {
        line.positionCount = resolution + 1;
        line.SetPositions(CalculateLineArray());
        yield return null;
    }

    private Vector3[] CalculateLineArray()
    {
        Vector3[] lineArray = new Vector3[resolution + 1];

        for (int i = 0; i<lineArray.Length; i++)
        {
            var t = i/(float)lineArray.Length *2;
            lineArray[i] = CalculateLinePoint(t);
        }
        return lineArray;
    }

    private Vector3 CalculateLinePoint(float t)
    {
        float velocity = GetComponentInParent<WaterGun>().force/value;
        float x = velocity * t;
        float y = (velocity * t) - (g * Mathf.Pow(t, 2) / 2);

        return new Vector3(x, y, 0.0f);
    }
    void Start()
    {
        g = Mathf.Abs(Physics.gravity.y);
        //line = Instantiate(line, transform.position, transform.rotation);

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(RenderArc());
    }
}
