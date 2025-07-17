using UnityEngine;

public class GrappleShootScript : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = rb.linearVelocity;
        v.x = projectileSpeed;
        rb.linearVelocity = v;
    }

  
}
