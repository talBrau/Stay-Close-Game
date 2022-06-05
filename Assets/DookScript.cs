using UnityEngine;

public class DookScript : MonoBehaviour
{
    public void PushDook()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = 5;
        rb.simulated = true;
        rb.AddForceAtPosition(Vector2.right * 5,Vector2.up * 3);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
