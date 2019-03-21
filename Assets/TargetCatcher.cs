using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCatcher : MonoBehaviour
{
    public bool rerotate = false;
    public bool fixedRot = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Target")
        {
            if(fixedRot)
                transform.parent.GetComponent<TargetLauncher>().CatchVer3(col.gameObject.GetComponent<Target>());
            else if (rerotate)
                transform.parent.GetComponent<TargetLauncher>().CatchVer2(col.gameObject.GetComponent<Target>());
            else
                transform.parent.GetComponent<TargetLauncher>().CatchVer1(col.gameObject.GetComponent<Target>());
        }
    }
}
