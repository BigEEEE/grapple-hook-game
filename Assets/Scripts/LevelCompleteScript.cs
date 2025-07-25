using UnityEngine;

public class LevelCompleteScript : MonoBehaviour, Item
{
    public GameObject winScreen;

    public void Collect()
    {
        winScreen.SetActive(true);
        Destroy(gameObject);
        Time.timeScale = 0f;
    }
}
