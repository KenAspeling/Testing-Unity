using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public float minSize = 0.5f;
    public float maxSize = 2.0f;
    Rigidbody2D rb;
    public float minSpeed = 50f;
    public float maxSpeed = 250f;
    public float maxSpin = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(size, size, 1);

        rb = GetComponent<Rigidbody2D>();

        float speed = Random.Range(minSpeed, maxSpeed);
        Vector2 randomDir = Random.insideUnitCircle;
        rb.AddForce(randomDir * speed / size);

        float spin = Random.Range(-maxSpin, maxSpin);
        rb.AddTorque(spin);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
