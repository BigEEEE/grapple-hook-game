using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;


public class GrappleShootScript : MonoBehaviour
{
    
    private Rigidbody rb;
    private Vector3 shootDirection;
    private Camera mainCamera;
    private GameObject player;

    private LineRenderer lineRenderer;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;


    [SerializeField] private float projectileSpeed;
    [SerializeField] private LayerMask rayCastTargetPlane;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();

        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>());
        //Render line between projectile and player
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = 2;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;

        

        
        //Get mouse position
        Ray mPosRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mPosRay, out RaycastHit raycastHit, float.MaxValue, rayCastTargetPlane);
        Vector3 lookTarget = new Vector3(raycastHit.point.x, raycastHit.point.y, 0);

        shootDirection = raycastHit.point - transform.position;
        rb.linearVelocity = new Vector3(shootDirection.x, shootDirection.y, 0).normalized * projectileSpeed;

    }

    void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        Vector3[] points = new Vector3[2];
        points[1] = transform.position;
        points[2] = player.transform.position;


        lineRenderer.SetPositions(points);
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(transform.position);
        player.GetComponent<PlayerController>().MoveTowardsTarget(transform.position);
        Destroy(gameObject);
    }
}
