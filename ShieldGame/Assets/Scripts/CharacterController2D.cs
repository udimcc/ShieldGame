using UnityEngine;
using UnityEngine.Events;

public enum DirectionMode
{
    None,
    LookAtMouse,
    ByMovement
}

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField] float _jumpForce = 400f;
    [SerializeField] int _jumpAmount = 2;
    [Range(0, .3f)] [SerializeField] float _movementSmoothing = .05f;
    [SerializeField] bool _airControl = false;
    [SerializeField] DirectionMode _directionMode = DirectionMode.None;
    [SerializeField] LayerMask _whatIsGround = new LayerMask();
    [SerializeField] GameObject _flipObject = null;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    Rigidbody2D _rb;
    const float _groundedRadius = .2f;
    bool _grounded;
    const float _ceilingRadius = .2f;
    Vector3 _velocity = Vector3.zero;
    int _lastFlipDirection = 0;
    int _currentJumpAmount = 0;

    private void Awake()
    {
        this._rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (this._directionMode == DirectionMode.LookAtMouse)
        {
            this.PlayerLookAtMouse();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Utils.IsInLayerMask(this._whatIsGround, collision.gameObject.layer))
        {
            this._grounded = true;
            this._currentJumpAmount = 0;
            this.OnLandEvent.Invoke();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (Utils.IsInLayerMask(this._whatIsGround, collision.gameObject.layer))
        {
            this._grounded = false;
        }
    }


    public void Move(float move, bool jump)
    {
        if ((this._grounded) || (this._airControl))
        {
            Vector3 targetVelocity = new Vector2(move * 10f, this._rb.velocity.y);
            this._rb.velocity = Vector3.SmoothDamp(this._rb.velocity, targetVelocity, ref this._velocity, this._movementSmoothing);

            if ((this._directionMode == DirectionMode.ByMovement) && (move != 0))
            {
                int flipDirection = move > 0 ? 1 : -1;

                if (this._lastFlipDirection != flipDirection)
                {
                    this.Flip(flipDirection);
                }

                this._lastFlipDirection = flipDirection;
            }
        }

        if (jump)
        {
            if (((this._currentJumpAmount < this._jumpAmount) && (this._currentJumpAmount > 0)) ||
                ((this._currentJumpAmount == 0) && (this._grounded)))
            {
                this._grounded = false;
                this._rb.velocity = new Vector2(this._rb.velocity.x, 0);
                this._rb.AddForce(new Vector2(0f, this._jumpForce));

                this._currentJumpAmount += 1;
            }
        }
    }


    private void PlayerLookAtMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos -  (Vector2)this._flipObject.transform.position;
        this.Flip(lookDir.x > 0 ? 1 : -1);
    }


    private void Flip(int lookDirInt)
    {
        this._flipObject.transform.localScale = new Vector2(Mathf.Abs(this._flipObject.transform.localScale.x) * lookDirInt, 
                                                            this._flipObject.transform.localScale.y);
    }
}

