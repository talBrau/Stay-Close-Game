using UnityEngine;
using UnityEngine.Events;

public class CheckPointScript : MonoBehaviour
{
    public UnityEvent checkPointEvent;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameManager.LastCheckPoint = gameObject;
            checkPointEvent.Invoke();
        }
    }
}
