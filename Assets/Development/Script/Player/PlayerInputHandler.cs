using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    #region Insperctor

    #endregion

    #region Fields

    private PlayerManager _playerManager;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _playerManager = GetComponent<PlayerManager>();
    }

    #endregion
    
    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
            _playerManager.Move(context.ReadValue<Vector2>().x);
        if(context.canceled)
            _playerManager.Move(0f);
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
            _playerManager.Jump();
        
        if (context.canceled && GetComponent<Rigidbody2D>().velocity.y> 0 )
            _playerManager.ShortJump();
    }
    
    
    public void MagnetToFriend(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _playerManager.MagnetToFriend();
        }
    }
    
}
