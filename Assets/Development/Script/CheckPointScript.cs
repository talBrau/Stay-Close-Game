using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameManager.LastCheckPoint = gameObject;
            var cameraScript = GetComponent<CinemachineChange>();
            if (cameraScript)
                cameraScript.activatedFlag = true;
        }
    }
}
