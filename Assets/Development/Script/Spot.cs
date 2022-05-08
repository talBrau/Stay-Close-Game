using Script;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Spot : MonoBehaviour
{
    #region Inspector
    
    [SerializeField] private UnityEvent spotEvent;

    #endregion

    #region Fields

    #endregion
    
    
    #region MonoBehaviour

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("FriendRaduis"))
        {
            col.gameObject.GetComponentInParent<FriendController>().EnteredSpot(this);
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
        GetComponent<SpriteRenderer>().color = Color.red;
    }
    public void UnHighlightSpot()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void InvokeEvent()
    {
        spotEvent.Invoke();
    }
    
    #endregion
    
    


}
