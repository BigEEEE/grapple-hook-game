using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundCheckMask;
    private float grapplePower;

    private bool jumpKeyWasPressed = false;
    private float horizontalInput;
    
    public float jumpHeight;
    public float moveSpeed;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
       
        if (Input.GetKeyDown(KeyCode.Space) && (Physics.OverlapSphere(groundCheckTransform.position, 0.5f, groundCheckMask).Length > 0))
        {
            jumpKeyWasPressed=true;
        }

        horizontalInput = Input.GetAxis("Horizontal") * moveSpeed;
        


    }

    private void FixedUpdate()
    {
        if (jumpKeyWasPressed == true) 
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
            jumpKeyWasPressed = false;
        }

        if (horizontalInput != 0)
        {
            rb.linearVelocity = new Vector3(horizontalInput, rb.linearVelocity.y, rb.linearVelocity.z);
        }
    }
    public void MoveTowardsTarget(Vector3 grappleImpactPosition)
    {
       
        Vector3 moveDirection = grappleImpactPosition - transform.position;
        grapplePower = Mathf.Clamp(2 * moveDirection.magnitude, 0, 15);
        Debug.Log(grapplePower);
        rb.linearVelocity = new Vector3(moveDirection.x, moveDirection.y, 0).normalized * grapplePower;
        Debug.Log(rb.linearVelocity);
    }
}
