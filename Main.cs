using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;

public class Main : MonoBehaviour
{
    [SerializeField]
    public static bool MuteSound = false;
    public Sprite MuteSprite, UnMuteSprite;
    public PostProcessProfile ProfileInGame;
    public GameObject[] Buildings = new GameObject[3];
    public int Currentbuilding, CountOfFall;
    public bool Placed = false, NeedToCorrect = false, Lose = true, CountOn = false;
    public float Speed, MaxLeft, MaxRight, deltaCorrect, ResOfCorrect, MaxScore, CurHeight, MaxHeight;
    public GameObject MaxScoreText, StartButton, MuteButton, GoDownButton, CanvasWithTutorial, BiteText, DropSound, Every10PointSound;
    public Animator EveryNpoints;
    float MoveK = 1, SpeedToStart, BiteScore, ToMakeHappy = 50;
    GameObject Building, Wallet, CurLeader;

    void Start()
    {
        Wallet = new GameObject("Buildings");
        BiteText.transform.position = new Vector2(Screen.width / 2, (Screen.height / 10) * 8);
        BiteText.transform.Rotate(0, 0, Random.Range(-25, 25));
        MaxScore = PlayerPrefs.GetFloat("MaxScore");
        BiteScore = MaxScore;
        BiteText.transform.Rotate(0, 0, Random.Range(-25, 25));
        BiteText.GetComponent<Text>().text += BiteScore + "m!";
        MaxScoreText.transform.position = new Vector2(Screen.width / 2, (Screen.height / 10) * 9);
        MuteButton.transform.position = new Vector2(Screen.width / 2, (Screen.height / 100) * 35);
        GoDownButton.transform.position = new Vector2(Screen.width / 2, (Screen.height / 100) * 15);
        StartButton.transform.localScale = new Vector2((Screen.width / 100) * 20, (Screen.width / 100) * 20);
        MuteButton.transform.localScale = new Vector2((Screen.width / 100) * 15, (Screen.width / 100) * 15);
        GoDownButton.transform.localScale = new Vector2((Screen.width / 100) * 15, (Screen.width / 100) * 15);
    }

    void Update()
    {
        if(CurHeight > ToMakeHappy)
        {
            Every10PointSound.GetComponent<AudioSource>().Play();
            EveryNpoints.SetTrigger("Start");
            ToMakeHappy += 50;
        }
        if (Lose != true)
        {
            MaxScoreText.GetComponent<Text>().text = "Max Height: \n" + MaxHeight + "m";
            if (CurHeight > MaxHeight)
            {
                MaxHeight = CurHeight;
            }
            if (CurLeader != null)
                CurHeight = Mathf.Round(CurLeader.transform.position.y * 10.0f) * 1.0f;
            if (CurHeight / 10 < Camera.main.transform.position.y - 6 || CountOfFall >= 3)
            {
                if (MaxHeight > MaxScore)
                {
                    PlayerPrefs.SetFloat("MaxScore", MaxHeight);
                    MaxScore = MaxHeight;
                }
                Lose = true;
                GoDownButton.SetActive(true);
                NeedToCorrect = true;
                ResOfCorrect = 2;
                if (transform.position.y > 3)
                    SpeedToStart = transform.position.y / 3;
                GetComponent<SkyController>().GoToStart = true;
            }
        }

        if (NeedToCorrect)
        {
            if(Camera.main.transform.position.y <= ResOfCorrect && Lose != true)
            {
                Camera.main.transform.Translate(Vector2.up * Time.deltaTime);
            }
            else if(Camera.main.transform.position.y >= ResOfCorrect && Lose)
            {
                Camera.main.transform.Translate(Vector2.down * Time.deltaTime * SpeedToStart);
                if (Camera.main.transform.position.y <= ResOfCorrect)
                {
                    ResetLevel();
                }
            }
            else
            {
                Placed = false;
                NeedToCorrect = false;
            }
        }

        if (Placed != true && NeedToCorrect != true && Lose != true)
        {
            Building = Instantiate(Buildings[Currentbuilding]);
            var RandomX = Random.Range(MaxLeft, MaxRight);
            Building.transform.position = new Vector3(RandomX, Camera.main.transform.position.y + 1.5f, 0);
            Building.transform.parent = Wallet.transform;
            Speed = Random.Range(1f, 3f);

            Placed = true;
        }

        if (Input.touchCount > 0 && Lose != true)
        {
            Touch touch = Input.GetTouch(0);
            Currentbuilding = Random.Range(0, 3);
            if (touch.phase == TouchPhase.Began && Building != null)
            {
                Building.GetComponent<Rigidbody2D>().isKinematic = false;
                DropSound.GetComponent<AudioSource>().Play();
                Building = null;
            }
        }

        if (Building != null && Lose != true)
        {
            Building.transform.Translate(Vector3.right * MoveK * Speed * Time.deltaTime);
            if(Building.transform.position.x <= MaxLeft)
            {
                MoveK = 1;
            }
            else if(Building.transform.position.x >= MaxRight)
            {
                MoveK = -1;
            }
        }

        if(MuteSound)
        {
            AudioListener.volume = 0f;
            MuteButton.GetComponent<Image>().sprite = UnMuteSprite;
        }
        else
        {
            MuteButton.GetComponent<Image>().sprite = MuteSprite;
            AudioListener.volume = 1f;
        }
    }

    public void Restart()
    {
        StartButton.SetActive(false);
        MuteButton.SetActive(false);
        CanvasWithTutorial.SetActive(true);
        MaxScoreText.SetActive(true);
        BiteText.SetActive(false);
        GetComponent<PostProcessVolume>().profile = ProfileInGame;
        Lose = false;
        NeedToCorrect = true;
    }

    public void Mute()
    {
        MuteSound = !MuteSound;
    }
    
    public void Starter()
    {
        StartCoroutine(WaitUntilLose());
    }

    public void SetCurHeight(float posY, GameObject CurBuilding)
    {
        if(CurHeight < posY && !CurBuilding.GetComponent<Rigidbody2D>().isKinematic)
        {
            CurLeader = CurBuilding;
        }
    }

    void ResetLevel()
    {
        ToMakeHappy = 50;
        CurHeight = 0;
        MaxScoreText.GetComponent<Text>().text = "" + MaxScore;
        StartButton.SetActive(true);
        MuteButton.SetActive(true);
        GoDownButton.SetActive(false);
        BiteText.transform.Rotate(0, 0, Random.Range(-25, 25));
        BiteText.GetComponent<Text>().text = "Try To Beat Your Record: " + MaxScore + "!";
        BiteText.SetActive(true);
        MaxScoreText.SetActive(false);
        GetComponent<SkyController>().GoToStart = false;
        CountOfFall = 0;
        CountOn = false;
    }

    public void GoDown()
    {
        ResetLevel();
        transform.position = new Vector3(transform.position.x, 2, transform.position.z);
    }

    IEnumerator WaitUntilLose()
    {
        yield return new WaitForSeconds(1f);
        CountOfFall = 0;
        CountOn = false;
    }
}