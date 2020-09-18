using System.Threading.Tasks;
using System.Linq;
using UnityEngine;


enum PatrolState
{
    Idle,
    Patrol,
}

[RequireComponent(typeof(CharacterController2D))]
public class Patrol : MonoBehaviour
{
    public float speed = 300f;
    public float patrolDelayMin = 0.4f;
    public float patrolDelayMax = 1.5f;
    public float reachDistance = 1f;

    // Public Refs
    public GameObject patrolArea = null;

    PatrolState _state = PatrolState.Idle;
    float _timeToPatrol;
    Transform[] _patrolPoints;
    Vector2 _nextPatrolPoint;

    // Private Refs
    CharacterController2D _controller;


    private void Awake()
    {
        this._controller = this.gameObject.GetComponent<CharacterController2D>();
    }

    private void Start()
    {
        if (this.patrolArea == null)
        {
            GameObject[] patrolAreas = GameObject.FindGameObjectsWithTag(Tags.PatrolArea.Value);

            if (patrolAreas.Length == 0)
            {
                Debug.LogError("No patrol areas found");
            }

            this.patrolArea = patrolAreas.OrderBy(x => Vector2.Distance(x.transform.position, this.transform.position)).First();
        }

        this._patrolPoints = Utils.GetChildren(this.patrolArea.transform);
    }

    private void FixedUpdate()
    {
        switch (this._state)
        {
            case PatrolState.Idle:
                this._timeToPatrol -= Time.fixedDeltaTime;

                if (this._timeToPatrol <= 0)
                {
                    this._nextPatrolPoint = this.GetNewPatrolPoint();
                    this._state = PatrolState.Patrol;
                }

                break;

            case PatrolState.Patrol:
                this._controller.MoveToTarget(this._nextPatrolPoint, this.speed);

                float targetDistance = Vector2.Distance((Vector2)this.transform.position, this._nextPatrolPoint);

                if (targetDistance < reachDistance)
                {
                    this._state = PatrolState.Idle;
                    this._timeToPatrol = Random.Range(this.patrolDelayMin, this.patrolDelayMax);
                }
                break;
        }
    }

    private Vector3 GetNewPatrolPoint()
    {
        int ppIndex = Random.Range(0, this._patrolPoints.Length - 2);
        Transform p1 = this._patrolPoints[ppIndex];
        Transform p2 = this._patrolPoints[ppIndex + 1];

        Vector3 patrolVector = p2.position - p1.position;
        return p1.position + patrolVector * Random.value;
    }
}
