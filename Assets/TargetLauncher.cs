using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLauncher : MonoBehaviour
{

    public bool loaded;
    GameObject YEET;
    Transform directionObj;
    public float launchSpeed = 15;
    Vector3 OriginalAngle;
    // Start is called before the first frame update
    void Awake()
    {
        directionObj = transform.Find("Direction");
        Debug.Log(directionObj);
        loaded = false;
        OriginalAngle = transform.eulerAngles;
    }



    public void CatchVer1(Target target)
    {
        target.transform.parent = transform;
        target.GetComponent<Rigidbody>().isKinematic = true;
        target.transform.localPosition = new Vector3(0, 0, 0);
        YEET = target.gameObject;
        loaded = true;
        Invoke("Launch", 1);
    }

    public void CatchVer3(Target target)
    {
        target.transform.parent = transform;
        target.GetComponent<Rigidbody>().isKinematic = true;
        target.transform.localPosition = new Vector3(0, 0, 0);
        target.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        YEET = target.gameObject;
        loaded = true;
        Invoke("Launch", 1);
    }

    public void CatchVer2(Target target)
    {
        transform.eulerAngles = new Vector3(OriginalAngle.x, OriginalAngle.y + Random.Range(-6f, 6f), OriginalAngle.z);
        Vector3 temp = target.transform.lossyScale;
        target.transform.parent = transform;
        target.GetComponent<Rigidbody>().isKinematic = true;
        target.transform.localPosition = new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        target.transform.localEulerAngles = new Vector3(Random.Range(-4,4), Random.Range(-4, 4), Random.Range(-4, 4));
        target.transform.parent = null;
        target.transform.localScale = temp;
        target.transform.parent = transform;
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
