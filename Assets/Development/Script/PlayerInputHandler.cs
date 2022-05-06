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
        _playerManager.Move(context.ReadValue<Vector2>().x);
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
            _playerManager.Jump();
        
        if (context.canceled && GetComponent<Rigidbody2D>().velocity.y> 0 )
            _playerManager.shortJump();
    }
    
    
    
}
