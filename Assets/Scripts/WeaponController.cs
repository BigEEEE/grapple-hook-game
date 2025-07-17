using UnityEngine;
using UnityEngine.Rendering;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask rayCastTargetPlane;
    [SerializeField] private GameObject grappleProjectile;
    [SerializeField] private Transform grappleGun;
    

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Get mouse position
        Ray mPosRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mPosRay, out RaycastHit raycastHit, float.MaxValue, rayCastTargetPlane);

        //Rotates gun to point at mouse position
        Vector3 lookTarget = new Vector3(raycastHit.point.x, raycastHit.point.y, 0);
        transform.LookAt(lookTarget);

       


        if (Input.GetKeyDown(KeyCode.Mouse1) == true)
        {
            Debug.Log("Shoot");
            Instantiate(grappleProjectile, grappleGun.position, grappleGun.rotation);
        }

    }

    

}
