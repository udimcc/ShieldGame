using System.Threading.Tasks;
using System.Linq;
using UnityEngine;

enum EnemyState
{
    Patrol,
    AfterPlayer,
    Attack
}


[RequireComponent(typeof(Patrol))]
[RequireComponent(typeof(CharacterController2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("General")]
    public float speed = 300;

    // Attack
    [Header("Attack Settings")]
    public float discoverPlayerDist = 7f;
    public float attackPlayerDist = 5f;
    public float giveUpDist = 10f;
    public float fireRate = 1f;
    public float bulletSpeed = 5;

    // Public Refs
    [Header("Refs")]
    public LayerMask enemyLayer = new LayerMask();
    public Transform firePos;
    public GameObject projectile;

    GameObject _player;
    [SerializeField] EnemyState _state = EnemyState.Patrol;

    // Attack
    float _fireFrequency;
    float _nextFiringTime = 0;

    // Private Refs
    Patrol _patrol;
    CharacterController2D _controller;

    void Awake()
    {
        this._patrol = this.GetComponent<Patrol>();
        this._controller = this.gameObject.GetComponent<CharacterController2D>();
        this._player = GameObject.FindGameObjectWithTag("Player");
        this._fireFrequency = 1 / this.fireRate;
    }

    private void Start()
    {
        this._patrol.speed = this.speed;
    }

    void Update()
    {
        switch (this._state)
        {
            case EnemyState.Patrol:
                if (this.CanSeePlayer())
                {
                    this._state = EnemyState.AfterPlayer;
                    this._patrol.enabled = false;
                }
                break;

            case EnemyState.AfterPlayer:
                if (Vector2.Distance(this.transform.position, this._player.transform.position) >= this.giveUpDist)
                {
                    this._state = EnemyState.Patrol;
                    this._patrol.enabled = true;
                    break;
                }

                if (this.CanAttackPlayer())
                {
                    this._state = EnemyState.Attack;
                    break;
                }

                this._controller.MoveToTarget(this._player.transform.position, this.speed);
                break;

            case EnemyState.Attack:
                if (!this.CanAttackPlayer())
                {
                    this._state = EnemyState.AfterPlayer;
                }

                this.AttackState();
                break;
        }
    }

    private bool CanSeePlayer()
    {
        Vector2 direction = (this._player.transform.position - this.transform.position).normalized;
        LayerMask mask = Utils.OmitLayer(Layers.Blocking, this.gameObject.layer);
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, this.discoverPlayerDist, mask);

        return hit.transform?.gameObject == this._player;
    } 

    private bool CanAttackPlayer()
    {
        Vector2 direction = (this._player.transform.position - this.transform.position).normalized;
        LayerMask mask = Utils.OmitLayer(Layers.Blocking, this.gameObject.layer);
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, this.attackPlayerDist, mask);

        return hit.transform?.gameObject == this._player;
    }

    void AttackState()
    {
        this._nextFiringTime -= Time.deltaTime;

        if (this._nextFiringTime <= 0)
        {
            this._nextFiringTime = this._fireFrequency;

            if (this._player == null)
            {
                return;
            }

            GameObject projectileObj = Instantiate(this.projectile, this.firePos.transform.position, Quaternion.identity);
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            Rigidbody2D projRb = projectileObj.GetComponent<Rigidbody2D>();

            if (projectile == null)
            {
                Debug.LogError("Projectile prefab doesn't have Projectile component");
                return;
            }

            if (projRb == null)
            {
                Debug.LogError("Projectile prefab doesn't have Rigidbody2D component");
                return;
            }

            Vector2 fireDir = (this._player.transform.position - this.firePos.transform.position).normalized;
            projectile.Initialize(Layers.EnemyProjectile, this.enemyLayer, this.bulletSpeed);
            projRb.AddForce(fireDir, ForceMode2D.Impulse);
        }
    }
}
