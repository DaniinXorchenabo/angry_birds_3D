﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class BeforeFlyPreparation : MonoBehaviour
{
    private Rigidbody _rb;
    public Transform centerRotatePoint;
    public Transform maxBackPoint;
    public Transform A_point_transform;
    public Transform B_point_transform;
    [HideInInspector]
    public Transform my_Trans;
    private Vector3 maxDistanse;
    // private Vector3 dlobal_radius = Vector3.zero;
    private float globalRotationRight = 0.0f;
    private float globalRotationUp = 0.0f;
    private Vector3 spase_progress;

    // private float radians_setup_right = 0.2f;
    private static float max_radians_right = 30.0f;
    private float right_radians_progress;


    // private float radians_setup_up = 0.2f;
    private static float max_radians_up = 50.0f;
    private float up_radians_progress;

    private bool spase_on_click = false;

    private FlyingScript flyingScript;

    // Start is called before the first frame update

    void Awake(){

        my_Trans = transform;
        centerRotatePoint.position = (A_point_transform.position + B_point_transform.position) / 2;
        maxBackPoint.position = new Vector3(maxBackPoint.position.x, centerRotatePoint.position.y, centerRotatePoint.position.z);

        my_Trans.position = centerRotatePoint.position;
        _rb = GetComponent<Rigidbody>();
        _rb.isKinematic = true;
        if (_rb.mass == 0){
            _rb.mass = 1f;
            Debug.LogError("Масса не может быть равна нулю!");
        }

        maxDistanse = maxBackPoint.localPosition - centerRotatePoint.localPosition;

        flyingScript = gameObject.GetComponent<FlyingScript>();
        flyingScript.enabled = false;
        flyingScript.init_param(my_Trans, _rb, centerRotatePoint.localPosition, maxDistanse, A_point_transform, B_point_transform);
    }

    void Start()
    {
        spase_progress = maxDistanse;
        right_radians_progress = max_radians_right;
        up_radians_progress = max_radians_up;
    }

    void FixedUpdate(){
        //_rb.AddForce(Vector3.up * 1 * Input.GetAxis("stretching"), ForceMode.Impulse);
        if (Input.GetAxis("stretching") != 0){
            if (!spase_on_click){
                spase_on_click = true;
            }
            Vector3 radius = spase_progress * (float)0.01f;
            spase_progress = spase_progress - radius;
            my_Trans.RotateAround(centerRotatePoint.position, Vector3.forward, -globalRotationUp);
            my_Trans.RotateAround(centerRotatePoint.position, Vector3.up, -globalRotationRight);
            my_Trans.localPosition = my_Trans.localPosition + radius;
            my_Trans.RotateAround(centerRotatePoint.position, Vector3.up, globalRotationRight);
            my_Trans.RotateAround(centerRotatePoint.position, Vector3.forward, globalRotationUp);

        } else {
            if (spase_on_click){  // кнопка была нажата, а теперь отпущена
                flyingScript.enabled = true;
                this.enabled = false;
            }
        }
        if (Input.GetAxis("Horizontal") != 0){
            float rotationRight = Input.GetAxis("Horizontal") * (Mathf.Abs(right_radians_progress) * 0.05f);
            if (centerRotatePoint.position == my_Trans.position){
                rotationRight *= -1;
            }
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
            if (centerRotatePoint.position == my_Trans.position){
                rotationUp *= -1;
            }
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
        // var enging_speed = Mathf.Sqrt((Vector3.Scale(-2*(centerRotatePoint.localPosition - transform.localPosition), (A_point_transform.localPosition + B_point_transform.localPosition - 2*transform.localPosition))/_rb.mass).magnitude);
        // var end_time = 
        // print(enging_speed);
    }

}
