using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string targetScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);
        }
    }
}
