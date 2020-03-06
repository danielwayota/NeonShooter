using System.Collections;

using UnityEngine;

public class Enemy : DynamicEntity
{
    [Header("Enemy")]
    public float speed = 1f;

    public Weapon weapon;

    [HideInInspector]
    public EnemyHorde horde;

    protected Vector3 targetPosition;

    private Vector2Int _targetGridCoordinates;
    public Vector2Int targetGridCoordinates {
        protected get => this._targetGridCoordinates;
        set {
            EnemyGrid.current.LeaveCoordinate(this._targetGridCoordinates);

            EnemyGrid.current.HoldCoordinate(value);
            this._targetGridCoordinates = value;

            this.targetPosition = EnemyGrid.current.GetPositionFromCoordinate(value);
        }
    }

    public bool isMoving
    {
        get; protected set;
    }

    private float time = 0;
    public float timeOut = 0.5f;

    protected float minDistanceToPoint = 0.1f;

    /// ==========================================
    protected override void Awake()
    {
        base.Awake();

        this.targetPosition = this.transform.position;
        this.isMoving = true;
        this._targetGridCoordinates = Vector2Int.zero;

        StartCoroutine(this.ShootLoop());
    }

    /// ==========================================
    private void Update()
    {
        this.time += Time.deltaTime;
        if (this.time >= this.timeOut)
        {
            this.time -= this.timeOut;

            this.OnTick();
        }
    }

    /// ==========================================
    protected virtual void OnTick()
    {
        Vector3 path = this.targetPosition - this.transform.position;
        Vector3 direction = path.normalized;

        bool needToMove = Vector3.Distance(this.transform.position, this.targetPosition) < this.minDistanceToPoint;

        if (needToMove == false)
        {
            this.isMoving = true;

            // Avoid getting too small movement velocity
            float distance = Mathf.Max(path.magnitude, this.minDistanceToPoint);
            direction *= Mathf.Min(this.speed, distance);

            this.body.velocity = direction;
        }
        else
        {
            if (this.isMoving)
            {
                this.isMoving = false;

                this.body.velocity = Vector3.zero;

                // Reached destination event
                this.OnDestinationReached();
            }
        }
    }

    /// ==========================================
    IEnumerator ShootLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(1f, 3f);

            yield return new WaitForSeconds(waitTime);

            this.weapon.Shoot();
        }
    }

    /// ==========================================
    protected override void Die()
    {
        this.horde?.OnEnemyDestroyed(this);

        base.Die();
    }

    /// ==========================================
    protected virtual void OnDestinationReached() {}
}
