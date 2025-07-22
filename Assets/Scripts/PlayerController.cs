using Unity.VisualScripting;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundCheckMask;
    [SerializeField] private float groundCheckRadius;

    [SerializeField] private GameObject wallCheckCollider;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float grappleMidAirWait;
    [SerializeField] private float timer;
   

    private float grapplePower;
    private float horizontalInput;
    private bool canMoveMidair = true;
    private bool jumpKeyWasPressed = false;
    private bool grappleUsed = false;
    private bool isGrounded = false;
   
    
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {

        horizontalInput = Input.GetAxis("Horizontal") * moveSpeed;

        if ((Physics.OverlapSphere(groundCheckTransform.position, groundCheckRadius, groundCheckMask).Length > 0))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true)
        {
            jumpKeyWasPressed = true;
            AudioManager.instance.PlaySFX("Jump");
        }

        //Timer for movement delay when grapple is used.
        if (timer < grappleMidAirWait && canMoveMidair == false)
        {
            timer += Time.deltaTime;
        }
        else
        {
            canMoveMidair = true;
            timer = 0;
        }




    }

    private void FixedUpdate()
    {
        if (jumpKeyWasPressed == true) 
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
            jumpKeyWasPressed = false;
        }

        if (horizontalInput != 0 && canMoveMidair == true)
        {
            rb.linearVelocity = new Vector3(horizontalInput, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        else if (isGrounded == true && grappleUsed == false)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
        }


        //Wall jump.
        //Moves wall collider to either side.
        if (horizontalInput < 0)
        {
            wallCheckCollider.transform.position = new Vector3(gameObject.transform.position.x - 0.25f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else if (horizontalInput > 0)
        {
            wallCheckCollider.transform.position = new Vector3(gameObject.transform.position.x + 0.25f, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else if (horizontalInput == 0)
        {
            wallCheckCollider.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        /*if(horizontalInput != 0)
        {
            if (jumpKeyWasPressed == true)
            {
                rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
                jumpKeyWasPressed = false;
            }
        }*/

    }
    public void MoveTowardsTarget(Vector3 grappleImpactPosition)
    {
        canMoveMidair = false;
        grappleUsed = true;

        Vector3 moveDirection = grappleImpactPosition - transform.position;
        grapplePower = Mathf.Clamp(2 * moveDirection.magnitude, 0, 15);
        Debug.Log(grapplePower);
        rb.linearVelocity = new Vector3(moveDirection.x, moveDirection.y, 0).normalized * grapplePower;
        Debug.Log(rb.linearVelocity);


    }

    void OnCollisionEnter(Collision collision)
    {
        if (grappleUsed == true)
        {
            canMoveMidair = true;
            grappleUsed = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        
    }
}
