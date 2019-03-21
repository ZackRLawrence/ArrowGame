using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    bool unHit;
    AudioSource source;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        unHit = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hit()
    {
        if (unHit == true)
        {
            GetComponentInChildren<Renderer>().material.color = Color.red;
            GameObject.Find("GameManager").GetComponent<GameManager>().targetHit();
            unHit = false;
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }
}
