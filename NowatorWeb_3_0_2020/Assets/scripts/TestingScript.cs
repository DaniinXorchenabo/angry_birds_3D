using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    public Transform centerRotatePoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(centerRotatePoint.position, Vector3.up, 0.1f);
        transform.RotateAround(centerRotatePoint.position, Vector3.right, 0.1f);

    }
}
