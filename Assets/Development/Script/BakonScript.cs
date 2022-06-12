using UnityEngine;

public class BakonScript : MonoBehaviour
{
    [SerializeField] private CircleCollider2D friendCollider;
    [SerializeField] private float newRadius;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            friendCollider.radius = newRadius;
    }
}
