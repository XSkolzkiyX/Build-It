using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    public GameObject SkyPrefab;
    public bool GoToStart = false;
    GameObject SkyLeftD, SkyRightD, SkyLeftU, SkyRightU;
    void Start()
    {
        SkyLeftD = Instantiate(SkyPrefab);
        SkyLeftD.transform.position = new Vector3(-15, 0, 1);
        SkyRightD = Instantiate(SkyPrefab);
        SkyRightD.transform.position = new Vector3(15, 0, 1);
        SkyLeftU = Instantiate(SkyPrefab);
        SkyLeftU.transform.position = new Vector3(-15, 17.6f, 1);
        SkyRightU = Instantiate(SkyPrefab);
        SkyRightU.transform.position = new Vector3(15, 17.6f, 1);
    }

    void Update()
    {
        SkyCheck(SkyLeftD);
        SkyCheck(SkyRightD);
        SkyCheck(SkyLeftU);
        SkyCheck(SkyRightU);
    }
    void SkyCheck(GameObject Sky)
    {
        if(Sky.transform.position.x > 30)
        {
            Sky.transform.position = new Vector3(-30, Sky.transform.position.y, Sky.transform.position.z);
        }
        else
        {
            Sky.transform.Translate(Vector2.right * Time.deltaTime);
        }
        if (!GoToStart)
        {
            if (Sky.transform.position.y < Camera.main.transform.position.y - 17.6f)
            {
                Sky.transform.position = new Vector3(Sky.transform.position.x, Sky.transform.position.y + 35.2f, Sky.transform.position.z);
            }
        }
        else
        {
            if (Sky.transform.position.y > Camera.main.transform.position.y + 17.6f)
            {
                Sky.transform.position = new Vector3(Sky.transform.position.x, Sky.transform.position.y - 35.2f, Sky.transform.position.z);
            }
        }
    }
}
