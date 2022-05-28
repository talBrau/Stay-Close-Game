using UnityEngine;

public class RevealPlatform : MonoBehaviour
{
    [SerializeField] private Collider2D feetCollider; //object has 2 colliders, this is the one thats not trigger
    [SerializeField] [Range(0, 1)] private float revealDelta;
    private SpriteRenderer _spriteRenderer;

    private Color _deltaColor;
    private bool _reveal;
    private bool _isDone;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        //hide at first
        _spriteRenderer.color = new Color(1,1,1,0);
        feetCollider.isTrigger = true;
        _deltaColor = new Color(0, 0, 0, revealDelta);
        _isDone = true;
        _reveal = false;
    }

    private void Update()
    {
        if (!_isDone)
        {
            if (_reveal)
            {

                _spriteRenderer.color += _deltaColor;
                _isDone = _spriteRenderer.color.a >= 1;
                feetCollider.isTrigger = !_isDone;
            }
            else
            {
                _spriteRenderer.color -= _deltaColor;
                _isDone = _spriteRenderer.color.a <= 0;
                feetCollider.isTrigger = _isDone;
            }
        }
    }

    public void Reveal()
    {
        _reveal = true;
        _isDone = false;
    }

    public void Hide()
    {
        _reveal = false;
        _isDone = false;

    }
}