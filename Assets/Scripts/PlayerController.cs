using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundCheckMask;

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
        Debug.Log(Physics.OverlapSphere(groundCheckTransform.position, 0.1f).Length);
       
        if (Input.GetKeyDown(KeyCode.Space) && (Physics.OverlapSphere(groundCheckTransform.position, 0.5f, groundCheckMask).Length > 0))
        {
            jumpKeyWasPressed=true;
        }

        horizontalInput = Input.GetAxis("Horizontal") * moveSpeed;
        Debug.Log(horizontalInput);


    }

    private void FixedUpdate()
    {
        if (jumpKeyWasPressed == true) 
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
            jumpKeyWasPressed = false;
        }

        rb.linearVelocity = new Vector3(horizontalInput, rb.linearVelocity.y, rb.linearVelocity.z);

    }

}
