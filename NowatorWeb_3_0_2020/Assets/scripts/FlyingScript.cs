using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingScript : MonoBehaviour
{
    private Rigidbody _rb;
    private Transform my_tr;
    private Vector3 start_center_point;
    private Vector3 flying_direction;
    private float powerfull = 0.0f;
    private int counter = 0;
    private float script_start_time;


    public void init_param(Transform my_trans,
                    Rigidbody my_rb,
                    Vector3 center_p,
                    Vector3 distanse_v){
        my_tr = my_trans;
        _rb = my_rb;
        start_center_point = center_p;
        flying_direction = distanse_v;
    }
    // Start is called before the first frame update
    void OnEnable(){
        _rb.isKinematic = false;
    }
    void Start()
    {
        
    }
    void FixedUpdate(){
        PowerControl();
    }

    void PowerControl(){
        if (counter == 0){
            script_start_time = (float)Time.realtimeSinceStartup;
        }
        if ((start_center_point - my_tr.localPosition).z >= 0.0f){
            _rb.AddRelativeForce(new Vector3(0.0f, 0.0f, generateForse()));
        }
        counter++;

    }
     float generateForse(){
        float time = (float)Time.realtimeSinceStartup - script_start_time;
        return 30.0f;
     }


}
