using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Rendering;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask rayCastTargetPlane;
    [SerializeField] private GameObject grappleProjectile;
    [SerializeField] private Transform grappleGun;
    [SerializeField] private float grappleGunRotSpeed;

    private GameObject currentGrappleProjectile;
    private bool canShootGrapple = true;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentGrappleProjectile = GameObject.FindGameObjectWithTag("GrappleProjectile");
      
        //Get mouse position
        Ray mPosRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mPosRay, out RaycastHit raycastHit, float.MaxValue, rayCastTargetPlane);

        //Rotates gun to point at mouse position
        if (canShootGrapple == true)
        {
            Vector3 lookTarget = new Vector3(raycastHit.point.x, raycastHit.point.y, 0);
            Vector3 lookDirection = lookTarget - gameObject.transform.position;
            lookDirection.Normalize();

            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.LookRotation(lookDirection), grappleGunRotSpeed * Time.deltaTime);
            
        }
        //Rotates gun to point at fired projectile.
        else if (currentGrappleProjectile != null)
        {
            transform.LookAt(currentGrappleProjectile.transform.position);
        }

       


        if (Input.GetKeyDown(KeyCode.Mouse1) == true && canShootGrapple == true)
        {
            Debug.Log("Shoot");
            Instantiate(grappleProjectile, grappleGun.position, grappleGun.rotation);
            canShootGrapple = false;
        }
        if (GameObject.FindGameObjectsWithTag("GrappleProjectile").Length == 0)
        {
            canShootGrapple = true;
        }
        
    }

    
    

}
