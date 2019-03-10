using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLauncher : MonoBehaviour
{

    public bool loaded;
    GameObject YEET;
    Transform directionObj;
    public float launchSpeed = 15;
    // Start is called before the first frame update
    void Awake()
    {
        directionObj = transform.Find("Direction");
        Debug.Log(directionObj);
        loaded = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(YEET.transform.parent);
    }

    
    public void Catch(Target target)
    {
        target.transform.parent = transform;
        target.GetComponent<Rigidbody>().isKinematic = true;
        target.transform.localPosition = new Vector3(0, 0, 0);
        YEET = target.gameObject;
        loaded = true;
        Invoke("Launch", 1);
    }

    void Launch()
    {
        Vector3 direction;
        YEET.transform.parent = null;
        Debug.Log(YEET.transform.parent);
        direction = (directionObj.position - transform.position).normalized;
        YEET.GetComponent<Rigidbody>().isKinematic = false;
        YEET.GetComponent<Rigidbody>().velocity = direction * launchSpeed;
        loaded = false;
    }
}
