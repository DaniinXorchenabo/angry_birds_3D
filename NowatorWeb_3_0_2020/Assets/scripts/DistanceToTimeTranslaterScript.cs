using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceToTimeTranslaterScript : MonoBehaviour
{
    public GameObject mapsBox;
    private Dictionary<float, float> distanceToTimeMap;
    private MuvementMapLib mapsBoxSct;
    private Vector3 CenterPointPos;
    public float t1, t2;


    // Start is called before the first frame update
    void Start()
    {
        var my_mass = GetComponent<Rigidbody>().mass;
        var fly = GetComponent<FlyingScript>();
        var k = fly.stiffness_coefficient;
        CenterPointPos = (fly.A_point_transform.position + fly.B_point_transform.position)/2;
        mapsBoxSct = mapsBox.GetComponent<MuvementMapLib>();
        distanceToTimeMap = mapsBoxSct.muveMap[my_mass][k];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var a = (CenterPointPos - transform.position).magnitude * 100;
        var k = (a - Mathf.Floor(a)) / (Mathf.Ceil(a) - Mathf.Floor(a)); // умножить на меньшее и прибавить меньшее
        var big_n = distanceToTimeMap[Mathf.Ceil(a)/100];
        var small_n = distanceToTimeMap[Mathf.Floor(a)/100];
        t1 = small_n + k*small_n;
        var A = Mathf.Floor(a);
        var B = Mathf.Ceil(a);
        var C = a;
        var AC = C - A;
        var CB = B - C;
        var lam = AC/CB;
        var Xa = small_n;
        var Xb = big_n;
        var res = (Xa + lam*Xb)/(1 + lam);
        t2 = res;  //distanceToTimeMap[Mathf.Round(a)/100];
        // print("cool t = " + t1.ToString() + "   " + t2);        
    }
}
