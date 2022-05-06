
using UnityEngine;

public class Spot : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("friend"))
        {
            col.gameObject.GetComponent<FriendController>().EnteredSpot(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("friend"))
        {
            other.gameObject.GetComponent<FriendController>().ExitSpot(this);
        }
    }

    public void HighlightSpot()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void UnHighlightSpot()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
