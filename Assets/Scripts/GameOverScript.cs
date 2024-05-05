using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball")) {
            Vector3 TargetPos = collision.transform.position;
            TargetPos.y /= (collision.transform.rotation * Vector3.up).y;
            if (TargetPos.y > transform.position.y) {
                Debug.Log(TargetPos.y);
                Debug.Log(transform.position.y);
                GamaManager.instance.GameOverTriggerStay(collision);
            }
        }
    }
}
