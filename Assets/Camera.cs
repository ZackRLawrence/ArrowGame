using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject cameraDolly;

    Vector2 mouseLook;
    Vector2 smoothV;
    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked && Time.timeScale > 0)
        {
            var md = new Vector2(Input.GetAxisRaw("Mouse X")/* + Input.GetAxis("HorizontalLook")*/, Input.GetAxisRaw("Mouse Y")/* + Input.GetAxis("VerticalLook")*/);

            md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
            smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
            smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
            mouseLook += smoothV;

            if (transform.rotation.eulerAngles.x >= 270 || transform.rotation.eulerAngles.x <= 90)
                transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            /*
            else if (transform.localRotation.eulerAngles.x > 180)
                transform.localRotation = Quaternion.Euler(270, 0, 0);
            else
                transform.localRotation = Quaternion.Euler(90, 0, 0);
            */
            cameraDolly.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, Vector3.up);
        }
    }
}
