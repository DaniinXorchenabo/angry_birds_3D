using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public float camera_muvement_speed = 8f;
    private Vector3 _position;
    public LayerMask maskObjectals;
    // Start is called before the first frame update
    void Start()
    {
        _position = target.InverseTransformPoint(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        var oldRotation = target.rotation;
        target.rotation = Quaternion.Euler(0, oldRotation.eulerAngles.y, 0);
        var currentPosition = target.TransformPoint(_position);
        target.rotation = oldRotation;

        transform.position = Vector3.Lerp(transform.position, currentPosition, camera_muvement_speed*Time.deltaTime);
        var currentRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, currentRotation, camera_muvement_speed*Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(target.position, transform.position - target.position, out hit, Vector3.Distance(transform.position, target.position), maskObjectals)){
            transform.position = hit.point;
            transform.LookAt(target);
        }
        */
    }

    void FixedUpdate(){
        
    }
}
