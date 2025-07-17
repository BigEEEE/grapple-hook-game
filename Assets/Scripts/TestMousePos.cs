using UnityEngine;

public class TestMousePos : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray mPosRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(mPosRay, out RaycastHit raycastHit))
        {
            transform.position = raycastHit.point;
        }
        
    }
}
