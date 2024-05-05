using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GamaManager : MonoBehaviour
{
    public static GamaManager instance;

    public float currentTime;
    public float cooltime;

    public GameObject GameOverLine;

    public BallScript Ball1;
    public BallScript Ball2;
    public BallScript Ball3;
    public BallScript Ball4;
    public BallScript Ball5;

    public BallScript currentBall;
    public bool ballOn;

    public float Ball1LimitLeft;
    public float Ball1LimitRight;

    public float Ball2LimitLeft;
    public float Ball2LimitRight;

    public float Ball3LimitLeft;
    public float Ball3LimitRight;

    public float currentLimitLeft;
    public float currentLimitRight;

    public Queue<BallScript> collisionQueue;

    public GameObject GameOverUI;
    public Button ToMainMenu;

    public TMP_Text ball1Result;
    public TMP_Text ball2Result;
    public TMP_Text ball3Result;
    public TMP_Text ball4Result;
    public TMP_Text ball5Result;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }



    public void makeBall() {
        int targetID;
        float prob = Random.Range(0f, 1f);
        if (prob < 0.6f) {
            targetID = 0;
            currentLimitLeft = Ball1LimitLeft;
            currentLimitRight = Ball1LimitRight;
        }
        else if (prob < 0.9f)
        {
            targetID = 1;
            currentLimitLeft = Ball2LimitLeft;
            currentLimitRight = Ball2LimitRight;
        }
        else
        {
            targetID = 2;
            currentLimitLeft = Ball3LimitLeft;
            currentLimitRight = Ball3LimitRight;
        }

        switch (targetID) {
            case 0:
                currentBall = Instantiate(Ball1, new Vector2(-5f, 6f), Quaternion.identity, instance.transform);
                currentBall.transform.GetComponent<Rigidbody2D>().simulated = false;
                currentBall.name = "ball";
                break;

            case 1:
                currentBall = Instantiate(Ball2, new Vector2(-5f, 6f), Quaternion.identity, instance.transform);
                currentBall.transform.GetComponent<Rigidbody2D>().simulated = false;
                currentBall.name = "ball";
                break;

            case 2:
                currentBall = Instantiate(Ball3, new Vector2(-5f, 6f), Quaternion.identity, instance.transform);
                currentBall.transform.GetComponent<Rigidbody2D>().simulated = false;
                currentBall.name = "ball";
                break;
        }

        ballOn = true;
         
    }
    

    // Start is called before the first frame update
    void Start()
    {
        GameOverUI.SetActive(false);
        collisionQueue = new Queue<BallScript>();
        ballOn = true;
        makeBall();
    }

    void setResult() {
        // List<GameObject> balls
        int ball1Counter = 0;
        int ball2Counter = 0;
        int ball3Counter = 0;
        int ball4Counter = 0;
        int ball5Counter = 0;

        BallScript[] balls = transform.GetComponentsInChildren<BallScript>();
        for (int inum = 0; inum < balls.Length; inum++) {

            int ballID = balls[inum].getID();
            switch (ballID) {
                case 0:
                    ball1Counter++;
                    break;               
                case 1:
                    ball2Counter++;
                    break;                
                case 2:
                    ball3Counter++;
                    break;               
                case 3:
                    ball4Counter++;
                    break;               
                case 4:
                    ball5Counter++;
                    break;
            }
        }

        ball1Result.text = ball1Counter.ToString();
        ball2Result.text = ball2Counter.ToString();
        ball3Result.text = ball3Counter.ToString();
        ball4Result.text = ball4Counter.ToString();
        ball5Result.text = ball5Counter.ToString();

    }

    public void OnButtonClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("MainScene");
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if (ballOn) {
            if (Input.GetKey(KeyCode.A))
            {
                Debug.Log("A");
                currentBall.transform.position -= new Vector3(Time.deltaTime * 5f, 0f, 0f);
            }
            else if (Input.GetKey(KeyCode.D)) {
                Debug.Log("D");
                currentBall.transform.position += new Vector3(Time.deltaTime * 5f, 0f, 0f);
            }

            if (currentBall.transform.position.x < currentLimitLeft) {
                currentBall.transform.position = new Vector3(currentLimitLeft, 6f, 0f);
            }
            if (currentBall.transform.position.x > currentLimitRight)
            {
                currentBall.transform.position = new Vector3(currentLimitRight, 6f, 0f);
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                ballOn = false;
                currentTime = 0f;
                currentBall.transform.GetComponent<Rigidbody2D>().simulated = true;
                currentBall.setDrop();
            }
        }

        if (!ballOn && currentTime > cooltime) {
            makeBall();
        }

        if (currentTime > 100f) {
            currentTime = 0f;
        }

    }

    public void GameOverCheck() {
        Time.timeScale = 0f;

        setResult();
        Debug.Log("GameOver");
        GameOverUI.SetActive(true);

    }

    public void GameOverTriggerStay(Collider2D coll) {
        if (!coll.transform.gameObject.GetComponent<BallScript>().isCurrent()) {
            GameOverCheck();
        }
    }

    public void BallQueue(BallScript _instance, Vector3 _createPos) {
        collisionQueue.Enqueue(_instance);
        if (collisionQueue.Count > 1) {
            BallScript ball1 = collisionQueue.Dequeue();
            BallScript ball2 = collisionQueue.Dequeue();

            //Vector3 ball1Pos = ball1.transform.position;
            //// ball1Pos.x /= (ball1.transform.rotation * Vector3.right).x;
            //// ball1Pos.y /= (ball1.transform.rotation * Vector3.up).y;

            //Vector3 ball2Pos = ball2.transform.position;
            //// ball2Pos.x /= (ball2.transform.rotation * Vector3.right).x;
            //// ball2Pos.y /= (ball2.transform.rotation * Vector3.up).y;

            //Vector2 instPos = new Vector2((ball1Pos.x + ball2Pos.y) / 2, (ball1Pos.y + ball2Pos.y) / 2);
            BallScript prefab = ball1.GetComponent<BallScript>().getNextBall();
            //// ball2.GetComponent<BallScript>().mergeLock();
            //Debug.Log(instPos);

            if (ball1.isLock()) {
                BallScript newBall = Instantiate(prefab, _createPos, Quaternion.identity, instance.transform);
                newBall.GetComponent<BallScript>();
                newBall.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                newBall.name = "ball";
                newBall.GetComponent<BallScript>().setVisible();
                newBall.GetComponent<BallScript>().setDrop();
                collisionQueue.Clear();
                Destroy(ball1.gameObject);
                Destroy(ball2.gameObject);
                return;
            }
            if (ball2.isLock())
            {
                BallScript newBall = Instantiate(prefab, _createPos, Quaternion.identity, instance.transform);
                newBall.GetComponent<BallScript>();
                newBall.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                newBall.name = "ball";
                newBall.GetComponent<BallScript>().setVisible();
                newBall.GetComponent<BallScript>().setDrop();
                collisionQueue.Clear();
            }
            Destroy(ball1.gameObject);
            Destroy(ball2.gameObject);
        }
    }
}
