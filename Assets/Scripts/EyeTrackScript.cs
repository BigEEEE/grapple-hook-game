using UnityEngine;

public class EyeTrackScript : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask rayCastTargetPlane;

    // Update is called once per frame
    void Update()
    {
        //Get mouse position
        Ray mPosRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mPosRay, out RaycastHit raycastHit, float.MaxValue, rayCastTargetPlane);

        //Rotates eye to point at mouse position
        Vector3 lookTarget = raycastHit.point;
        transform.LookAt(lookTarget);
    }
}
