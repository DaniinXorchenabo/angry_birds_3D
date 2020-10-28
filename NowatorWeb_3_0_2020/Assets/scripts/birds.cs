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
    private Vector3 spase_progress;

    // private float radians_setup_right = 0.2f;
    private static float max_radians_right = 10.0f;
    private float right_radians_progress;


    // private float radians_setup_up = 0.2f;
    private static float max_radians_up = 50.0f;
    private float up_radians_progress;


    // Start is called before the first frame update
    void Start()
    {
        my_Trans = transform;
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;

        maxDistanse = maxBackPoint.localPosition - centerRotatePoint.localPosition;
        spase_progress = maxDistanse;

        right_radians_progress = max_radians_right;
        // radians_setup_right = right_radians_progress * 0.05f;

        up_radians_progress = max_radians_up;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        //_rb.AddForce(Vector3.up * 1 * Input.GetAxis("stretching"), ForceMode.Impulse);
        if (Input.GetAxis("stretching") != 0){
            Vector3 radius = spase_progress * (float)0.01f;
            spase_progress = spase_progress - radius;
            my_Trans.RotateAround(centerRotatePoint.position, Vector3.forward, -globalRotationUp);
            my_Trans.RotateAround(centerRotatePoint.position, Vector3.up, -globalRotationRight);
            my_Trans.localPosition = my_Trans.localPosition + radius;
            my_Trans.RotateAround(centerRotatePoint.position, Vector3.up, globalRotationRight);
            my_Trans.RotateAround(centerRotatePoint.position, Vector3.forward, globalRotationUp);

        }
        if (Input.GetAxis("Horizontal") != 0){
            float rotationRight = Input.GetAxis("Horizontal") * (Mathf.Abs(right_radians_progress) * 0.05f);
            if (Mathf.Abs(rotationRight) > 0.01f || rotationRight * right_radians_progress < 0.0f){
                globalRotationRight += rotationRight;
                right_radians_progress = right_radians_progress - rotationRight;
                if (Mathf.Abs(right_radians_progress) > max_radians_right){
                    right_radians_progress = -right_radians_progress;
                }
                // radians_setup_right = Mathf.Abs(right_radians_progress) * 0.05f;
                my_Trans.RotateAround(centerRotatePoint.position, Vector3.up, rotationRight);
            }
        }

        // print(globalRotationRight);

        if (Input.GetAxis("Vertical") != 0){
            float rotationUp = Input.GetAxis("Vertical") * (Mathf.Abs(up_radians_progress) * 0.01f);
            if (Mathf.Abs(rotationUp) > 0.01f || rotationUp * up_radians_progress < 0.0f){

                globalRotationUp += rotationUp;
                up_radians_progress = up_radians_progress - rotationUp;
                if (Mathf.Abs(up_radians_progress) > max_radians_up){
                    up_radians_progress = -up_radians_progress;
                }
                // radians_setup_right = radians_setup_right - 
                my_Trans.RotateAround(centerRotatePoint.position, Vector3.forward, rotationUp);
                // print(globalRotationUp);
            }
        }
    }

}
