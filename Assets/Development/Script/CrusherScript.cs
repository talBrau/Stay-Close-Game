using UnityEngine;

public class CrusherScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && col.gameObject.GetComponent<PlayerManager>().CanJump)
        {
            GameObject.Find("SceneManager").GetComponent<SceneManager>().ChangeLevel(false);
        }
    }
}
