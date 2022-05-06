using UnityEngine;

public class Spot : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("friend"))
        {
            print("hi");
            col.gameObject.GetComponent<friendController>().EnteredSpot(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("friend"))
        {
            other.gameObject.GetComponent<friendController>().ExitSpot(this);
        }
    }
}
