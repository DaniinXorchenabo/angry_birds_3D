using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class DictTimeDistanceCreater : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    public Transform MaxBackPoint;
    private string result = "private Dictionary<float, float> MuvementCart = new Dictionary<float, float>{";
    private Vector3 startPlayerPosition;
    private float setup = 0.01f;
    private float now_setup = 0f;
    private float startTime = 0f;
    private bool startTestFlag = false;
    private float nowStartDistance;
    private Vector3 basisVectorMuvement;
    private bool WorkingFlag = true;
    private Vector3 muvementVector;
    //----------------
    private FlyingScript flyScr;
    private BeforeFlyPreparation beforeFlyScr;
    private Rigidbody _rb;
    private Dictionary<float, float> MuvementCart = new Dictionary<float, float>{{0f, 0.0006313324f}, {0.01f, 0.548255f}, {0.02f, 0.003969193f}, {0.03f, 0.002761841f}, {0.04f, 0.03271008f}, {0.05f, 0.03132343f}, {0.06f, 0.03108788f}, {0.07f, 0.02927589f}, {0.08f, 0.04809856f}, {0.09f, 0.02345371f}, {0.1f, 0.02960682f}, {0.11f, 0.04337311f}, {0.12f, 0.03579617f}, {0.13f, 0.02726555f}, {0.14f, 0.02892017f}, {0.15f, 0.03196716f}, {0.16f, 0.02751637f}, {0.17f, 0.03354168f}, {0.18f, 0.0411005f}, {0.19f, 0.04507828f}, {0.2f, 0.039464f}, {0.21f, 0.03822327f}, {0.22f, 0.04596138f}, {0.23f, 0.02368355f}, {0.24f, 0.02541637f}, {0.25f, 0.0271244f}, {0.26f, 0.0243187f}, {0.27f, 0.04055405f}, {0.28f, 0.04174137f}, {0.29f, 0.04149437f}, {0.3f, 0.06704807f}, {0.31f, 0.04054737f}, {0.32f, 0.02949238f}, {0.33f, 0.02448082f}, {0.34f, 0.03648949f}, {0.35f, 0.04293156f}, {0.36f, 0.02907944f}, {0.37f, 0.03415489f}, {0.38f, 0.03670597f}, {0.39f, 0.04968548f}, {0.4f, 0.03984261f}, {0.41f, 0.03859711f}, {0.42f, 0.02993298f}, {0.43f, 0.02849102f}, {0.44f, 0.03381252f}, {0.45f, 0.0281105f}, {0.46f, 0.03619003f}, {0.47f, 0.02709961f}, {0.48f, 0.03270817f}, {0.49f, 0.04920292f}, {0.5f, 0.04189873f}, {0.51f, 0.03088284f}, {0.52f, 0.03380966f}, {0.53f, 0.03394413f}, {0.54f, 0.0216732f}, {0.55f, 0.03090572f}, {0.56f, 0.03040695f}, {0.57f, 0.03542328f}, {0.58f, 0.03402519f}, {0.59f, 0.0221405f}, {0.6f, 0.03705025f}, {0.61f, 0.02949047f}, {0.62f, 0.02884579f}, {0.63f, 0.03332424f}, {0.64f, 0.03878117f}, {0.65f, 0.03940105f}, {0.66f, 0.04446125f}, {0.67f, 0.02860165f}, {0.68f, 0.04233456f}, {0.69f, 0.03876686f}, {0.7f, 0.0414629f}, {0.71f, 0.02879238f}, {0.72f, 0.03168201f}, {0.73f, 0.03902912f}, {0.74f, 0.04219151f}, {0.75f, 0.03882217f}, {0.76f, 0.03756332f}, {0.77f, 0.03851795f}, {0.78f, 0.03597069f}, {0.79f, 0.04265594f}, {0.8f, 0.02663517f}, {0.81f, 0.03164482f}, {0.82f, 0.02933311f}, {0.83f, 0.03673458f}, {0.84f, 0.03882504f}, {0.85f, 0.03256512f}, {0.86f, 0.02963448f}, {0.87f, 0.02906132f}, {0.88f, 0.05764866f}, {0.89f, 0.04000282f}, {0.9f, 0.04708767f}, {0.91f, 0.04415131f}, {0.92f, 0.02555656f}, {0.93f, 0.03518486f}, {0.94f, 0.05565834f}, {0.95f, 0.03739929f}, {0.96f, 0.04231167f}, {0.97f, 0.0260334f}, {0.98f, 0.04065609f}, {0.99f, 0.04219437f}, {1f, 0.03468704f}, {1.01f, 0.0362711f}, {1.02f, 0.05177975f}, {1.03f, 0.030797f}, {1.04f, 0.03226566f}, {1.05f, 0.04071426f}, {1.06f, 0.03572464f}, {1.07f, 0.03471375f}, {1.08f, 0.0435667f}, {1.09f, 0.02991772f}, {1.1f, 0.02719212f}, {1.11f, 0.0345993f}, {1.12f, 0.02734184f}, {1.13f, 0.02821255f}, {1.14f, 0.02871609f}, {1.15f, 0.03480721f}, {1.16f, 0.03095245f}, {1.17f, 0.03770161f}, {1.18f, 0.03798389f}, {1.19f, 0.02176094f}, {1.2f, 0.0307188f}, {1.21f, 0.03255367f}, {1.22f, 0.03868008f}, {1.23f, 0.02655506f}, {1.24f, 0.0315876f}, {1.25f, 0.0297823f}, {1.26f, 0.02633953f}, {1.27f, 0.04302025f}, {1.28f, 0.04320717f}, {1.29f, 0.02941513f}, {1.3f, 0.04183578f}, {1.31f, 0.0415535f}, {1.32f, 0.02872086f}, {1.33f, 0.02947426f}, {1.34f, 0.0346508f}, {1.35f, 0.04012108f}, {1.36f, 0.03411674f}, {1.37f, 0.0403595f}, {1.38f, 0.04812241f}, {1.39f, 0.04501534f}, {1.4f, 0.03834915f}, {1.41f, 0.03701973f}, {1.42f, 0.03765297f}, {1.43f, 0.03087807f}, {1.44f, 0.04181862f}, {1.45f, 0.03343201f}, {1.46f, 0.03415108f}, {1.47f, 0.02366638f}, {1.48f, 0.03485107f}, {1.49f, 0.03494835f}, {1.5f, 0.04380608f}, {1.51f, 0.03569412f}, {1.52f, 0.05117798f}, {1.53f, 0.03733826f}, {1.54f, 0.029459f}, {1.55f, 0.05677986f}, {1.56f, 0.03757095f}, {1.57f, 0.02672577f}, };


    


    void Start()
    {
        startPlayerPosition = player.position;
        flyScr = player.gameObject.GetComponent<FlyingScript>();
        beforeFlyScr = player.gameObject.GetComponent<BeforeFlyPreparation>();
        _rb = player.gameObject.GetComponent<Rigidbody>();
        muvementVector = MaxBackPoint.position - startPlayerPosition;
        var maxNumber = System.Math.Max(System.Math.Max(muvementVector.x, muvementVector.y), muvementVector.z);
        basisVectorMuvement = (setup / maxNumber) * muvementVector /setup;
        print(maxNumber + "  " + (basisVectorMuvement).ToString());
    }

    // Update is called once per frame
    void Update(){
        Working();
    }

    void FixedUpdate(){
        Working();
    }

    void Working(){
        if (WorkingFlag){
            if(!startTestFlag){
                SetOnStartPosition();
                startTest();
            }
            if(startTestFlag){
                if ((startPlayerPosition - player.position).x >= 0.0f){
                    float nowTime = (float)Time.realtimeSinceStartup;
                    _rb.velocity = Vector3.zero;
                    _rb.isKinematic = true;
                    player.gameObject.SetActive(false);
                    startTestFlag = false;
                    result += "{" + (Mathf.Round(nowStartDistance*100)/100).ToString().Replace(",", ".") + "f, " + (nowTime-startTime).ToString().Replace(",", ".") + "f}, ";
                } else {
                    print("Working");
                }

            }
        }
    }

    void SetOnStartPosition(){
        beforeFlyScr.enabled = false;
        flyScr.enabled = false;
        player.position = startPlayerPosition + basisVectorMuvement*now_setup;
        nowStartDistance = Mathf.Round((startPlayerPosition - player.position).magnitude*100)/100;
        now_setup += setup;
        startTime = 0f;
        _rb.isKinematic = true;
        _rb.velocity = Vector3.zero;
        player.gameObject.SetActive(false);
    }

    void startTest(){
        if ( (basisVectorMuvement*now_setup).magnitude <= muvementVector.magnitude){
            startTestFlag = true;
            startTime = (float)Time.realtimeSinceStartup;
            flyScr.enabled = true;
            player.gameObject.SetActive(true);
        } else {
            WorkingFlag = false;
            result += "};";
            print("ended work");
            print(result);
        }
    }

}
