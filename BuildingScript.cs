using UnityEngine;
using UnityEngine.UI;

public class BuildingScript : MonoBehaviour
{
    public Sprite Foundation;
    public float DeadZone, TargetZone;
    public GameObject ScoreText, ExplosionPrefab, ExplosionAudio, Appearance;
    public bool NeedToPlay = true;
    GameObject Connection = null, Touch;
    bool IsNeed = true;
    
    
    private void Start()
    {
        Instantiate(Appearance, transform.position, Quaternion.identity);
        Touch = GameObject.Find("TouchSound");
        ExplosionAudio = GameObject.Find("ExplosionSound");
        ScoreText = GameObject.Find("MaxScoreText");
    }
    void Update()
    {
        if(Connection != null)
            Camera.main.GetComponent<Main>().SetCurHeight(Mathf.Round(transform.position.y * 10.0f) * 1f, gameObject);
        if(Camera.main.GetComponent<Main>().Lose && Mathf.RoundToInt(Camera.main.transform.position.y) - 2 <= Mathf.RoundToInt(transform.position.y))
        {
            ExplosionAudio.GetComponent<AudioSource>().Play();
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (transform.position.y < TargetZone && IsNeed)
        {
            if (NeedToPlay)
            {
                Camera.main.GetComponent<Main>().NeedToCorrect = true;
                Camera.main.GetComponent<Main>().ResOfCorrect = 0;
                IsNeed = false;
            }
            else
            {
                if (Camera.main.GetComponent<Main>().CountOn != true)
                {
                    Camera.main.GetComponent<Main>().CountOfFall++;
                    Camera.main.GetComponent<Main>().CountOn = true;
                    Camera.main.GetComponent<Main>().Starter();
                }
            }
        }
        if (transform.position.y <= DeadZone)
            Destroy(gameObject);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GetComponent<Rigidbody2D>().isKinematic)
        {
            if (collision.tag == "Building")
            {
                if(collision.gameObject.name == "Platform")
                {
                    GetComponent<SpriteRenderer>().sprite = Foundation;
                    Instantiate(Appearance, transform.position, Quaternion.identity);
                }
                if (NeedToPlay)
                {
                    Touch.GetComponent<AudioSource>().Play();
                    Camera.main.GetComponent<Main>().NeedToCorrect = true;
                    Camera.main.GetComponent<Main>().ResOfCorrect = Camera.main.transform.position.y + Camera.main.GetComponent<Main>().deltaCorrect;
                    NeedToPlay = false;
                }
                Connection = collision.gameObject;
                transform.tag = "Building";
            }
            else
            {
                Camera.main.GetComponent<Main>().NeedToCorrect = true;
                Camera.main.GetComponent<Main>().ResOfCorrect = Camera.main.transform.position.y + Camera.main.GetComponent<Main>().deltaCorrect;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Connection = null;
    }
}
