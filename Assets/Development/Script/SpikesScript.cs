using UnityEngine;

public class SpikesScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            GameObject.Find("SceneManager").GetComponent<SceneManager>().ChangeLevel(false);
    }
}
