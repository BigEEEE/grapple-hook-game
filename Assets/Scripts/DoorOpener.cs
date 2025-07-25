using UnityEngine;

public class DoorOpener : MonoBehaviour, Item
{
    public GameObject door;

    public void Collect()
    {
        door.SetActive(false);
        Destroy(gameObject);
    }
}
