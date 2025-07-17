using UnityEngine;
using UnityEngine.Rendering;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask rayCastTargetPlane;
    [SerializeField] private GameObject grappleProjectile;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Ray mPosRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(mPosRay, out RaycastHit raycastHit, float.MaxValue, rayCastTargetPlane))
        {
            Vector3 lookTarget = new Vector3(raycastHit.point.x, raycastHit.point.y, 0);
            transform.LookAt(lookTarget);
          
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) == true)
        {
            GrappleShot();
        }

    }

    void GrappleShot()
    {
        Instantiate(grappleProjectile, transform.position, transform.localRotation);
    }

}
