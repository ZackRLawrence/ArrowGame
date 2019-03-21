using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrossBow : MonoBehaviour
{
    Arrow Arrow;
    Arrow originalArrow;
    Vector3 ArrowSlot;
    Vector3 ArrowScale;
    Quaternion ArrowRotation;
    AudioSource source;
    public AudioClip clip;
    bool loaded;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        originalArrow = GetComponentInChildren<Arrow>();
        Arrow = originalArrow;
        ArrowSlot = Arrow.transform.localPosition;
        ArrowRotation = Arrow.transform.localRotation;
        ArrowScale = Arrow.transform.localScale;
        Arrow.transform.position = new Vector3(0, -50, 0);
        Arrow.transform.parent = null;
        loaded = false;
        Reload();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Reload()
    {
        Arrow = GameObject.Instantiate<Arrow>(originalArrow, transform);
        Arrow.transform.localPosition = ArrowSlot;
        Arrow.transform.localRotation = ArrowRotation;
        Arrow.transform.localScale = ArrowScale;
        loaded = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && loaded == true && Time.deltaTime > 0)
        {
            Arrow.fireArrow();
            Arrow = null;
            loaded = false;
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }

        if (Input.GetMouseButtonDown(1) && loaded == false && Time.deltaTime > 0)
            Reload();
    }

}
