using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    [Header("Ball Info")]
    [SerializeField] int ColliderID = 0;
    [SerializeField] BallScript NextBallPrefab;
    [SerializeField] bool merging;
    [SerializeField] bool isFinal;
    [SerializeField] bool isTriggerOn;

    [SerializeField] float currentTime;
    [SerializeField] float sizeupTime;
    [SerializeField] bool currentBall;
    [SerializeField] bool drop;
    [SerializeField] bool visible;

    // Start is called before the first frame update
    void Start()
    {
        currentBall = true;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = transform.GetChild(0).position;

        if (visible && transform.localScale.x < 1f) {
            sizeupTime += Time.deltaTime;
            transform.localScale += new Vector3(sizeupTime * 1f, sizeupTime * 1f, 1f);

            if (transform.localScale.x > 1f) {
                transform.localScale = new Vector3(1f, 1f, 1f);
                visible = false;
            }

        }

        if (drop) {
            currentTime += Time.deltaTime;

            if (currentTime > 3f) {
                currentBall = false;
            }
        }

        if (currentTime > 100f) {
            currentTime = 0f;
        }

    }

    public bool isCurrent() {
        return currentBall;
    }

    public void mergeLock() {
        merging = true;
    }

    public bool isLock() {
        return merging;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void setVisible() {
        visible = true;
    }

    public void setDrop() {
        drop = true;
    }

    public int getID() {
        return ColliderID;
    }

    public BallScript getNextBall() {
        return NextBallPrefab;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            MergedCollistion(collision);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            MergedCollistion(collision);
        }

    }

    public void MergedCollistion(Collision2D coll) {
        if (coll.transform.CompareTag("Ball") && !isFinal)
        {
            if (coll.gameObject.GetComponent<BallScript>().ColliderID == ColliderID && !isTriggerOn)
            {
                isTriggerOn = true;
                Vector2 collPos = coll.transform.position;
                if (collPos.y < transform.position.y || (collPos.y == transform.position.y && collPos.x < transform.position.x)) {
                    Debug.Log($"Origin {transform.position}");
                    Debug.Log($"Collision {collPos}");
                    coll.gameObject.GetComponent<BallScript>().mergeLock();
                }


                if (NextBallPrefab != null)
                {
                    GamaManager.instance.BallQueue(gameObject.GetComponent<BallScript>(), coll.contacts[0].point);
                }

                //if (merging)
                //{
                //    //Destroy(gameObject);
                //}
                //else {
                //    Destroy(gameObject);
                //    if (!isFinal && NextBallPrefab != null) {
                //        //BallScript ballObject = Instantiate(NextBallPrefab, coll.contacts[0].point, Quaternion.identity);
                //        //ballObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                //        //ballObject.name = "ball";
                //        //ballObject.setVisible();
                //        //ballObject.setDrop();
                //    }
            }
        }
    }
}
