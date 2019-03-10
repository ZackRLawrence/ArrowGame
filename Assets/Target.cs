using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    bool unHit;
    // Start is called before the first frame update
    void Start()
    {
        unHit = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Arrow" && unHit == true)
        {
            GetComponentInChildren<Renderer>().material.color = Color.red;
            GameObject.Find("GameManager").GetComponent<GameManager>().targetHit();
            unHit = false;
        }
    }
}
