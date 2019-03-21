using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float velocity;
    public float maxVelocity;
    public float minVelocity;
    AudioSource source;
    Rigidbody rb;
    bool airBorne;
    bool stuck;
    bool line = true;
    public Gradient lineGradient;
    LineRenderer lineRenderer;
    GameManager gameManager;
    // Start is called before the first frame update
    private void Start()
    {
        source = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        line = gameManager.line;
        rb = transform.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        airBorne = false;
        stuck = false;
        Physics.IgnoreLayerCollision(9, 9);
        createLine();
    }

    int counter = 0;
    void check()
    {
        counter++;
        if (counter > 60) { line = gameManager.line; counter = 0; }
    }

    public void fireArrow()
    {
        rb.isKinematic = false;
        transform.parent = null;
        rb.velocity = transform.forward * velocity;
        airBorne = true;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (airBorne == true)
            transform.forward = rb.velocity;
        if (stuck == false && airBorne == false)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                velocity += .5f;
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                velocity -= .5f;
            velocity = Mathf.Clamp(velocity, minVelocity, maxVelocity);
        }
    }

    void Update()
    {
        check();
        if (line == false && lineRenderer != null)
            Destroy(lineRenderer);
        if (line == true && lineRenderer == null)
            createLine();
        if (stuck == false && airBorne == true && Time.timeScale > 0 && line)
                updateLine(transform.position, rb.velocity, Physics.gravity);
        if (stuck == false && airBorne == false && Time.timeScale > 0 && line)
            updateLine(transform.position, transform.forward * velocity, Physics.gravity);
    }
    GameObject newParent;
    private void OnCollisionEnter(Collision collision)
    {
        Collider hitCollider = collision.contacts[0].thisCollider;
        if (collision.transform.tag != "Arrow")
        {
            airBorne = false;
            if (collision.transform.tag == "Target")
                newParent = collision.gameObject;
            else
                newParent = null;
            for (int i = 0; i < collision.contacts.Length; i++)
            {
                if (collision.contacts[i].thisCollider.name == "ArrowHead")
                {
                    Stick();
                    if (newParent != null)
                        newParent.GetComponent<Target>().hit();
                }
            }
            destroyLine();
        } 
    }

    private void Stick()
    {
        rb.isKinematic = true;
        Destroy(rb);
        stuck = true;
        if (newParent != null)
        {
            transform.parent = newParent.transform;
            rb.isKinematic = false;
        }
        Invoke("destroySelf", 5f);
    }

    private void destroySelf()
    {
        Destroy(gameObject);
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
        if (line == false)
        {
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, initialPosition - new Vector3(-100, -100, -100));
            return;
        }
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
