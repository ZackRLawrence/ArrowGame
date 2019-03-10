using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float velocity;
    public float maxVelocity;
    public float minVelocity;
    Rigidbody rb;
    bool airBorne;
    bool stuck;
    public Gradient lineGradient;
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    private void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        airBorne = false;
        stuck = false;
        Physics.IgnoreLayerCollision(9, 9);
        createLine();
    }

    public void fireArrow()
    {
        rb.isKinematic = false;
        transform.parent = null;
        rb.velocity = transform.forward * velocity;
        airBorne = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (airBorne == true)
            transform.forward = rb.velocity;
        if (stuck == false)
        {
            if (airBorne == false)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    velocity += .5f;
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    velocity -= .5f;
                velocity = Mathf.Clamp(velocity, minVelocity, maxVelocity);
                updateLine(transform.position, transform.forward * velocity, Physics.gravity);
            }
            else
                updateLine(transform.position, rb.velocity, Physics.gravity);
        }
    }
    GameObject newParent;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "Arrow")
        {
            airBorne = false;
            //rb.isKinematic = true;
            Invoke("Stick", 0.0f);
            stuck = true;
            newParent = collision.gameObject;
        }
    }

    private void Stick()
    {
        //rb.isKinematic = true;
        Destroy(rb);
        destroyLine();
        transform.parent = newParent.transform;
    }

    void createLine()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineRenderer.endWidth = 0.1f;
        lineRenderer.receiveShadows = false;
        lineRenderer.material = (Material)Resources.Load("Line", typeof(Material));
        lineRenderer.colorGradient = lineGradient;
        Debug.Log("Got em");

    }

    private void destroyLine()
    {
        Destroy(gameObject.GetComponent<LineRenderer>());
    }

    //gfoot's methodology used as a basis https://forum.unity.com/threads/projectile-prediction-line.143636/
    public LayerMask mask;
    void updateLine(Vector3 initialPosition, Vector3 initialVelocity, Vector3 gravity)
    {
        float timeDelta = 0.25f / initialVelocity.magnitude; // for example

        Vector3 lastPositon;
        Vector3 position = initialPosition;
        Vector3 velocity = initialVelocity;
        RaycastHit hit;
        float distance;
        float drag = rb.drag;
        Vector3 direction;
        for (int i = 0; i < 1000; ++i)
        {
            lineRenderer.positionCount = i + 1;
            lineRenderer.SetPosition(i, position);

            lastPositon = position;
            position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
            distance = Vector3.Distance(lastPositon, position);
            direction = (lastPositon - position).normalized;
            if (Physics.Raycast(lastPositon, direction, out hit, distance, layerMask: ~mask))
                break;
            velocity += gravity * timeDelta;
            velocity *= (1 - (drag * timeDelta));
        }
    }

}
