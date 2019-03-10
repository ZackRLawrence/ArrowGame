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
    bool loaded;
    // Start is called before the first frame update
    void Start()
    {
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
        if (Input.GetMouseButtonDown(0) && loaded == true)
        {
            Arrow.fireArrow();
            Arrow = null;
            loaded = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.R) && loaded == false)
            Reload();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}
