//using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;


public class GrappleShootScript : MonoBehaviour
{

    private float timer;
    private bool readyToDestroy = false;
    private bool collisionHapppened = false;
    private Rigidbody rb;
    private Vector3 shootDirection;
    private Vector3[] points = new Vector3[2];
    private Camera mainCamera;
    private GameObject projectileOrigin;
    private GameObject player;
    private LineRenderer lineRenderer;

    public Color c1 = new Color(130f, 50f,0f);
    public Color c2 = new Color(130f, 50f, 0f);

    [SerializeField] private float grappleLength;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private LayerMask rayCastTargetPlane;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        projectileOrigin = GameObject.FindGameObjectWithTag("ProjectileOrigin");
        player = GameObject.FindGameObjectWithTag("Player");

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = 0.1f;

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
        
        points[0] = transform.position;
        points[1] = projectileOrigin.transform.position;


        lineRenderer.SetPositions(points);

        Vector3 distance = player.transform.position - transform.position;

        Debug.Log(distance.magnitude);

        if (timer < 0.5 && collisionHapppened == true)
        {
            timer += Time.deltaTime;
        }
        else if (collisionHapppened == true)
        {
            readyToDestroy = true;
            timer = 0;
        }

        if (readyToDestroy == true || distance.magnitude > grappleLength)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Impact");
        AudioManager.instance.PlaySFX("ImpactGrapple");
        Debug.Log(transform.position);
        player.GetComponent<PlayerController>().MoveTowardsTarget(transform.position);
        player.GetComponent<PlayerController>().timer = 0;
        rb.linearVelocity = Vector3.zero;
        collisionHapppened = true;
    }
}
