using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class birds : MonoBehaviour
{
    private Rigidbody _rb;
    public Transform centerRotatePoint;
    public Transform maxBackPoint;
    public Transform my_Trans;
    private Vector3 maxDistanse;
    // private Vector3 dlobal_radius = Vector3.zero;
    private float globalRotationRight = 0.0f;
    private float globalRotationUp = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        my_Trans = transform;
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        maxDistanse = centerRotatePoint.localPosition - maxBackPoint.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        //_rb.AddForce(Vector3.up * 1 * Input.GetAxis("stretching"), ForceMode.Impulse);
        if (Input.GetAxis("stretching") != 0){
            Vector3 radius = Vector3.right * (float)0.05f;
            my_Trans.RotateAround(centerRotatePoint.position, Vector3.forward, -globalRotationUp);
            my_Trans.RotateAround(centerRotatePoint.position, Vector3.up, -globalRotationRight);
            my_Trans.localPosition = my_Trans.localPosition + radius;
            my_Trans.RotateAround(centerRotatePoint.position, Vector3.up, globalRotationRight);
            my_Trans.RotateAround(centerRotatePoint.position, Vector3.forward, globalRotationUp);

        }
        float rotationRight = Input.GetAxis("Horizontal") * 0.2f;
        globalRotationRight += rotationRight;
        my_Trans.RotateAround(centerRotatePoint.position, Vector3.up, rotationRight);

        float rotationUp = Input.GetAxis("Vertical") * 0.2f;
        globalRotationUp += rotationUp;
        my_Trans.RotateAround(centerRotatePoint.position, Vector3.forward, rotationUp);
    }

}
