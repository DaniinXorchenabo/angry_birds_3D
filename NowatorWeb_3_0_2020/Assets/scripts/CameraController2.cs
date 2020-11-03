using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera camera;
    public Transform target;
    public float speed_x = 250f;
    public float speed_y = 240f;
    public float limit_y = 40f;
    public float minDistance = 1.5f;
    public LayerMask obstacles;
    public LayerMask noPlayer;
    [HideInInspector]
    public float maxDistance_now;
    private float maxDistance;
    private Vector3 localPosition;
    private float current_y_rotation;
    private Vector3 position{
        get {return transform.position;}
        set {transform.position = value;}
    }
    private float counter = 0f;
    private Transform myStartParent;

    void Awake()
    {
        myStartParent = transform.parent;
        maxDistance = Vector3.Distance(position, target.position);
        transform.position = target.position;
        localPosition = target.InverseTransformPoint(position);
        transform.parent = target;
    }

    
    void OnEnable(){
        // maxDistance = Vector3.Distance(position, target.position);
        counter = 0f;
        transform.position = target.position;
        localPosition = target.InverseTransformPoint(position);
        transform.parent = myStartParent;
    }

    void FixedUpdate(){
        counter += 0.015f;
        ChangeMawDistance(counter);
    }
    public void ChangeMawDistance(float newDetermineDisiant){
        if (newDetermineDisiant <= 1f){
            // position = target.TransformPoint(localPosition);
            // position += maxDistance * newDetermineDisiant - maxDistance_now;
            // localPosition = target.InverseTransformPoint(position);

            maxDistance_now = maxDistance * newDetermineDisiant;
            
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        position = target.TransformPoint(localPosition);
        CameraRotation();
        ObsraclesReact();
        PlayerReact();
        localPosition = target.InverseTransformPoint(position);

    }
    void CameraRotation(){
        var mx = Input.GetAxis("Mouse X");
        var my = Input.GetAxis("Mouse Y");

        if (my != 0){
            var tmp = Mathf.Clamp(current_y_rotation + my*speed_y * Time.deltaTime, -limit_y, limit_y);
            if (tmp != current_y_rotation){
                var rot = tmp - current_y_rotation;
                transform.RotateAround(target.position, transform.right, rot);
                current_y_rotation = tmp;
            }
        }
        if (mx != 0){
            transform.RotateAround(target.position, Vector3.up, mx*speed_x * Time.deltaTime);
        }

        transform.LookAt(target);

    }
    void ObsraclesReact(){
        var distance = Vector3.Distance(position, target.position);
        RaycastHit hit;
        if (Physics.Raycast(target.position, transform.position - target.position, out hit, maxDistance_now, obstacles)){
            position = hit.point;
            print("--ffff");
        } else if (distance < maxDistance_now && ! Physics.Raycast(position, -transform.forward, .1f, obstacles)) {
            position = position - transform.forward * .05f;
            print("--dddddd");
        } else {
            print("maxDistance_now " + maxDistance_now);
            print("distance " + distance);
        }
    }
    void PlayerReact(){}

}
