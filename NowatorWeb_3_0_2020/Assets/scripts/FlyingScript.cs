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
    public GameObject Cam;

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
    private bool flag1 = true;
    private bool flag = true;
    private CameraController2 camScr;
    private float lastTime = 0f;
    private Vector3 all_impuls = Vector3.zero;
    private bool testingFlag = false;
    private DistanceToTimeTranslaterScript distanceToTimeTranslaterScript;
    

    void Awake(){
        Cam.transform.parent = transform;
        camScr = Cam.GetComponent<CameraController2>();
        camScr.enabled = false;
        distanceToTimeTranslaterScript = GetComponent<DistanceToTimeTranslaterScript>();
    }

    public void init_param(Transform my_trans,
                    Rigidbody my_rb,
                    Vector3 center_p,
                    Vector3 distanse_v,
                    Transform A_point,
                    Transform B_point){
        my_tr = my_trans;
        _rb = my_rb;
        start_center_point = center_p;
        flying_direction = distanse_v;
        my_mass = _rb.mass;
        A_point_transform = A_point;
        B_point_transform = B_point;
        my_tr.localPosition = (B_point_transform.localPosition + A_point_transform.localPosition)/2;
        AB_vector = B_point_transform.localPosition - A_point_transform.localPosition;
        AB_len = AB_vector.magnitude;
    }

    void OnEnable(){
        _rb.isKinematic = false;
        _rb.useGravity = false;
        CA_vector = A_point_transform.localPosition - my_tr.localPosition;
        CA_len = CA_vector.magnitude;
        a = new Vector3(0.0f, 0.0f, 0.0f);
        powerfull = 0.0f;
        CM_vector =  (B_point_transform.localPosition + A_point_transform.localPosition)/2 - my_tr.localPosition;
        start_local_position = my_tr.localPosition;
        flag1 = true;
        flag = true;
        camScr.enabled = true;
        all_impuls = Vector3.zero;
        lastTime = 0f;
        distanceToTimeTranslaterScript.enabled = false;
        // print("C point " + transform.localPosition.ToString());
        // print("CM vector " + CM_vector.ToString());
        // print("M point " + start_center_point.ToString());
    }

    void FixedUpdate(){
        if (testingFlag){
            TestPowerControl();
        } else {
             
            PowerControl();
        }

    }
    void Update(){
        if (!testingFlag && flag1){
            if ((start_center_point - my_tr.localPosition).x > 0.0f){
                EndedForse();
            }
        }
    }

    public void SetWorkingFlag(bool newFlag){
        testingFlag = newFlag;
    }

    void TestPowerControl(){
        var impuls =  ImpulsGettingControl();
        all_impuls += impuls;
        _rb.AddForce(impuls, ForceMode.Impulse);
    }

    void PowerControl(){
        if (flag){
            script_start_time = (float)Time.realtimeSinceStartup;
            flag = false;
            // var l_CM = start_center_point- transform.localPosition;
            // var l_CA = A_point_transform.localPosition - transform.localPosition;
            // var l_CB = B_point_transform.localPosition - transform.localPosition;
            // print(l_CM.ToString() + l_CA.ToString() + l_CB.ToString());
            // var pr1_CA = Vector3.Scale(l_CM * 1000,  l_CA)/(l_CM * 1000).magnitude;
            // var pr1_CB = Vector3.Scale(l_CM * 1000,  l_CB)/(l_CM * 1000).magnitude;
            // var pr2_CA = Vector3.Project(l_CA, l_CM);
            // var pr2_CB = Vector3.Project(l_CB, l_CM);
            // print(pr1_CA.ToString() + pr2_CA.ToString() + pr1_CB.ToString()+pr2_CB.ToString());

        }
        if ((start_center_point - my_tr.localPosition).x < 0.0f){
            // Vector3 forse = Vector3.Scale(generateForse2(), new Vector3(1.0f, 1.0f, 1.0f));
            // print("forse" + forse.ToString());
            // _rb.AddRelativeForce(forse);
            var impuls =  ImpulsGettingControl();
            all_impuls += impuls;
            _rb.AddForce(impuls, ForceMode.Impulse);
        } else {
            if (flag1){
                EndedForse();
            }
        }
    }

    void EndedForse(){
        // print("коэфициент" + (_rb.velocity.magnitude / (CM_vector * Mathf.Sqrt(2*stiffness_coefficient/my_mass)).magnitude).ToString());
                
        var timee = (float)Time.realtimeSinceStartup - script_start_time;
        transform.localPosition = start_center_point;
        _rb.velocity = -GetSpeed(distanceToTimeTranslaterScript.t2);
        // print("time " + timee);
        print(_rb.velocity.ToString() + "  " + GetSpeed(timee).ToString() + " " + GetSpeed(distanceToTimeTranslaterScript.t1).ToString() + " " + GetSpeed(distanceToTimeTranslaterScript.t2).ToString());                                     
        // print("скорости " + _rb.velocity.ToString() + "  " + all_impuls/my_mass + "  " + GetInpulsOnInterval(0f, (float)Time.realtimeSinceStartup - script_start_time)/my_mass);
        // print("Расчетная скорость" + (CM_vector * Mathf.Sqrt(2*stiffness_coefficient/my_mass)).ToString());
        // print("импульс " + all_impuls);
        // print((_rb.velocity.magnitude/(CM_vector * Mathf.Sqrt(2*stiffness_coefficient/my_mass)).magnitude));
        // print((2/stiffness_coefficient) + " " +  Mathf.Sqrt(1/stiffness_coefficient));
        // pint("ускорения" + a.ToString() + ForseOnly2(((float)Time.realtimeSinceStartup - script_start_time), 0f).ToString());
        // print((start_center_point - my_tr.localPosition).ToString());               
        _rb.useGravity = true;
        flag1 = false;
    }

    // Vector3 SimpleA(float time){
    //     return  2*CM_vector * (stiffness_coefficient/my_mass);
    // }

    // float ReturnSimpleT(){
    //     return  Mathf.Sqrt(my_mass/stiffness_coefficient);
    // }
    // Vector3 GetSimpleImpuls(float deltaTime){
    //     return SimpleA(deltaTime) * deltaTime *my_mass;
    // }

    // Vector3 GetA(float time){
    //     return -2*stiffness_coefficient*CM_vector/(my_mass + stiffness_coefficient * time*time);
    // }

    // Vector3 GetDeltaA(float lastTime, float time){
    //     return GetA(time) - GetA(lastTime);
    // }
    Vector3 GetSpeed(float time){
        return -2 * CM_vector * Mathf.Sqrt(stiffness_coefficient) * Mathf.Atan(time*Mathf.Sqrt(stiffness_coefficient)/Mathf.Sqrt(my_mass))/Mathf.Sqrt(my_mass);
    }
    Vector3 GetDeltaSpeed(float lastTime, float time){
        return GetSpeed(lastTime) -  GetSpeed(time);
    }

    Vector3 GetInpulsOnInterval(float lastTime, float time){
        return GetDeltaSpeed(lastTime, time) * my_mass;
    }

    Vector3 ImpulsGettingControl(){
        float time = (float)Time.realtimeSinceStartup - script_start_time;
        var data = GetInpulsOnInterval(lastTime, time);
        // var data = GetSimpleImpuls( time - lastTime);
        lastTime = time;
        return data;
    }
/*
    Vector3 generateForse(){
        float time = (float)Time.realtimeSinceStartup - script_start_time;
        Vector3 s = a * time*time /2;
        Vector3 local_CA =  A_point_transform.localPosition - (start_local_position + s);
        float local_CA_len = local_CA.magnitude;
        Vector3 new_c_point = my_tr.localPosition;
        Vector3 new_MC = start_center_point - new_c_point;
        Vector3 a1 = Vector3.Project(local_CA, new_MC);
        Vector3 local_CB = local_CA - AB_vector;
        Vector3 a2 = Vector3.Project(local_CB, new_MC);
        a = (a1 + a2) * stiffness_coefficient / my_mass;
        return a*my_mass;
    }

    Vector3 generateForse2(){
        float time = (float)Time.realtimeSinceStartup - script_start_time;
        a += ForseOnly2(time, lastTime);
        lastTime = time;
        return a;
    }
    
    public Vector3 ForseOnly(float time, float last_time){
        var new_CM = CM_vector * (time - last_time) - a * (time*time*time - last_time*last_time*last_time)/6;
        var new_CA = new_CM - AB_vector * (time - last_time) / 2;
        var new_CB = new_CM + AB_vector * (time - last_time) / 2;
        var a_setup = (Vector3.Project(new_CA, new_CM) + Vector3.Project(new_CB, new_CM)) * stiffness_coefficient / my_mass;
        return a_setup;
    }
    public Vector3 ForseOnly2(float time, float last_time){
        if (false && my_mass-stiffness_coefficient * last_time * last_time != 0f && time * last_time != 0){
            // a += -start_center_point * Mathf.Log(Mathf.Pow(Mathf.Abs(my_mass-stiffness_coefficient * time * time), last_time)/Mathf.Pow(Mathf.Abs(my_mass-stiffness_coefficient * last_time * last_time), time))/(time * last_time);
        }
        var delta_a = Vector3.zero;
        
        delta_a = getSpeed(time) - getSpeed(last_time);

        return delta_a * my_mass;
    }

    public Vector3 getSpeed(float time){
        // var l_speed = -start_center_point * Mathf.Log(Mathf.Abs(my_mass-stiffness_coefficient * time * time))/time;
        // var l_speed = -CM_vector * Mathf.Log(Mathf.Abs(my_mass + stiffness_coefficient * time * time))/time;
        var l_speed = -2 * CM_vector * Mathf.Sqrt(stiffness_coefficient) * Mathf.Atan(time*Mathf.Sqrt(stiffness_coefficient)/Mathf.Sqrt(my_mass))/Mathf.Sqrt(my_mass);
        return l_speed;
    }

    Vector3 generateForse3(){
        float time = (float)Time.realtimeSinceStartup - script_start_time;
        return a;
    } */
    // void OnTriggerEnter(Collider collider) {
    //     if (collider.gameObject.tag == "Center_point") {
    //         print("-0-0-0-0-0-0-[0 " + (start_center_point - my_tr.localPosition).ToString());
    //     }
    // }


}
