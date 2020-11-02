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
    public Transform A_point_transform;
    public Transform B_point_transform;

    private Vector3 A_point;
    private Vector3 a = new Vector3(0.0f, 0.0f, 0.0f); // ускарение
    private Vector3 CA_vector;
    private float CA_len;
    private Vector3 AB_vector;
    private float AB_len;
    private float my_mass;
    public float stiffness_coefficient;  // коэффициент жесткости
    private Vector3 CM_vector;
    private Vector3 start_local_position;


    void Awake(){
        AB_vector = B_point_transform.localPosition - A_point_transform.localPosition;
        AB_len = AB_vector.magnitude;

    }



    public void init_param(Transform my_trans,
                    Rigidbody my_rb,
                    Vector3 center_p,
                    Vector3 distanse_v){
        my_tr = my_trans;
        _rb = my_rb;
        start_center_point = center_p;
        flying_direction = distanse_v;
        my_mass = _rb.mass;
        my_tr.localPosition = (B_point_transform.localPosition + A_point_transform.localPosition)/2;


    }
    // Start is called before the first frame update
    void OnEnable(){
        _rb.isKinematic = false;

        CA_vector = A_point_transform.localPosition - my_tr.localPosition;
        CA_len = CA_vector.magnitude;
        a = new Vector3(0.0f, 0.0f, 0.0f);
        powerfull = 0.0f;
        CM_vector =  (B_point_transform.localPosition + A_point_transform.localPosition)/2 - my_tr.localPosition;
        start_local_position = my_tr.localPosition;
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
        if ((start_center_point - my_tr.localPosition).x <= 0.0f){
            Vector3 forse = Vector3.Scale(generateForse(), new Vector3(1.0f, -1.0f, 1.0f));
            // print("forse" + forse.ToString());
            // _rb.AddRelativeForce(forse);
             _rb.AddForce(-forse);
        }
        counter++;

    }
     Vector3 generateForse(){
        float time = (float)Time.realtimeSinceStartup - script_start_time;
        Vector3 s = a * time*time /2;
        // Vector3 local_CA =  A_point_transform.localPosition - ((B_point_transform.localPosition + A_point_transform.localPosition)/2 - CM_vector * ((CM_vector.magnitude - s.magnitude)/CM_vector.magnitude)) ;  // CA_vector * ((CA_len - s)/CA_len);
        Vector3 local_CA =  A_point_transform.localPosition - (start_local_position + s);
        float local_CA_len = local_CA.magnitude;
        // print("local_CA" + local_CA.ToString());
        // print("направление" + ((B_point_transform.localPosition + A_point_transform.localPosition)/2 - my_tr.localPosition).ToString());
        // Vector3 a1 = stiffness_coefficient*(local_CA + AB_vector * 0.5f);
        // Vector3 b1 = (local_CA + AB_vector).magnitude * local_CA + local_CA_len * (local_CA + AB_vector);
        Vector3 new_c_point = my_tr.localPosition;
        Vector3 new_MC = start_center_point - new_c_point;
        // Vector3 a1 = Vector3.Scale(new_MC, local_CA)/local_CA_len;
        Vector3 a1 = Vector3.Project(local_CA, new_MC);

        Vector3 local_CB = local_CA - AB_vector;
        // Vector3 a2 = Vector3.Scale(new_MC, local_CB)/local_CB.magnitude;
        Vector3 a2 = Vector3.Project(local_CB, new_MC);
        a = (a1 + a2) * stiffness_coefficient / my_mass;
        print(new_MC.ToString()  + local_CA.ToString() + a1.ToString() + local_CB.ToString() + a2.ToString()+(a1 + a2).ToString());
        // print(a.ToString()  + a1.ToString() + a2.ToString());






        // a = Vector3.Cross(a1, b1)/(my_mass * local_CA_len * (local_CA + AB_vector).magnitude);
        return a*my_mass;
     }


}
