using System.Runtime.CompilerServices;

using Unity.Mathematics;
using Unity.VisualScripting;

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject menuController;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private Transform wallCheckTransform;
    [SerializeField] private LayerMask groundCheckMask;
    [SerializeField] private Vector3 groundCheckSize;
    [SerializeField] private Vector3 wallCheckSize;



    [SerializeField] private float jumpHeight;
    [SerializeField] private float wallJumpVertical;
    [SerializeField] private float wallJumpHorizontal;
    [SerializeField] private float wallSlideSpeedModifier;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float grappleMidAirWait;
    
    public float timer;
   
    private float grapplePower;
    private float horizontalInput;
    private bool canMoveMidair = true;
    private bool jumpKeyWasPressed = false;
    private bool grappleUsed = false;
    private bool isGrounded = false;
    private bool isWallSliding = false;

    private Vector3 wallJumpDirection;
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
        horizontalInput = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if ((ObjectProximityCheck(groundCheckTransform.transform.position,groundCheckSize,groundCheckTransform.transform.rotation,groundCheckMask)))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded == true | isWallSliding == true)
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
        WallSlide();

        if (jumpKeyWasPressed && isGrounded)
        {
            Jump(Vector3.up, jumpHeight);
        }
        else if (jumpKeyWasPressed && isWallSliding)
        {
            Debug.Log("Wall jumped");
            canMoveMidair = false;
            Jump(wallJumpDirection, jumpHeight);
        }

        if (horizontalInput != 0 && canMoveMidair == true)
        {
            rb.linearVelocity = new Vector3(horizontalInput, rb.linearVelocity.y, rb.linearVelocity.z);
        }
        else if (isGrounded == true && grappleUsed == false)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, rb.linearVelocity.z);
        }

    }

    private bool ObjectProximityCheck (Vector3 pos, Vector3 size, quaternion rot, LayerMask mask)
    {
        if ((Physics.OverlapBox(pos, size, rot, mask).Length > 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Jump(Vector3 direction, float height)
    {
     
        rb.AddForce(direction * height, ForceMode.VelocityChange);
        jumpKeyWasPressed = false;

    }

    private void WallSlide()
    {
        
        //Moves wall collider to either side.
        if (horizontalInput < 0)
        {
            wallCheckTransform.transform.position = new Vector3(gameObject.transform.position.x - 0.25f, gameObject.transform.position.y, gameObject.transform.position.z);
            wallJumpDirection = new Vector3(Input.GetAxisRaw("Horizontal") / 2 * -1, 1.5f, 0);
           
        }
        else if (horizontalInput > 0)
        {
            wallCheckTransform.transform.position = new Vector3(gameObject.transform.position.x + 0.25f, gameObject.transform.position.y, gameObject.transform.position.z);
            wallJumpDirection = new Vector3(Input.GetAxisRaw("Horizontal") / 2 * -1, 1.5f, 0);
        }
        else if (horizontalInput == 0)
        {
            wallCheckTransform.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        if ((ObjectProximityCheck(wallCheckTransform.transform.position, wallCheckSize, wallCheckTransform.transform.rotation, groundCheckMask)))
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, - wallSlideSpeedModifier), rb.linearVelocity.z);
        }
        else 
        { 
            isWallSliding = false; 
        }
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

    void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            item.Collect();
        }

        if (other.CompareTag("Hazard"))
        {
            menuController.GetComponent<MenuController>().gameOverScreen.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(groundCheckTransform.position, groundCheckSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(wallCheckTransform.position, wallCheckSize);
    }

}
