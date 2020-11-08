/*clone*/using System.Collections;
/*clone*/using System.Collections.Generic;
/*clone*/using UnityEngine;
/*clone*/
/*clone*/public class MuvementMapLib1 : MonoBehaviour
/*clone*/{
/*clone*/    public Dictionary<float, Dictionary<float, Dictionary<float, float>>> muveMap =  new Dictionary<float, Dictionary<float, Dictionary<float, float>>>{
/*new_mass*/
/*mass 1f*/        {2f, new Dictionary<float, Dictionary<float, float>>{
/*new_k*/
/*k 120f*/                {120f, new  Dictionary<float, float>{
/*data*/                    {0f, 4f}, {1f, 3f}
/*-k 1f*/                }},
/*blank*/  
/*-mass 120f*/        }},
/*blank*/ 
/*clone*/    };
/*clone*/}
