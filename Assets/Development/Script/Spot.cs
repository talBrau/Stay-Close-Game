using Script;
using UnityEngine;
using UnityEngine.Events;

public class Spot : MonoBehaviour
{
    #region Inspector
    
    public UnityEvent spotEvent;

    #endregion

    #region Fields

    [SerializeField] private Sprite spotOnSprite;
    [SerializeField] private Sprite spotOffSprite;

    #endregion
    
    #region MonoBehaviour

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("FriendRaduis"))
        {
            col.gameObject.GetComponentInParent<FriendController>().EnteredSpot(this);
        }

        if (col.gameObject.CompareTag("friend"))
        {
            col.gameObject.GetComponent<FriendController>().ActivateSpotEvent(this);
            col.gameObject.GetComponent<FriendController>().OnSpot = this;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("FriendRaduis"))
        {
            other.gameObject.GetComponentInParent<FriendController>().ExitSpot(this);
        }
    }

    #endregion MonoBehaviour
    
    
    #region Methods

    public void HighlightSpot()
    {
        GetComponent<SpriteRenderer>().sprite = spotOnSprite;
    }
    public void UnHighlightSpot()
    {
        GetComponent<SpriteRenderer>().sprite = spotOffSprite;
    }
    
    
    #endregion
    
    


}
