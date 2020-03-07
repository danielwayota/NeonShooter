using System.Collections;

using UnityEngine;

public class Enemy : DynamicEntity
{
    [Header("Enemy")]
    public float speed = 1f;

    public Weapon weapon;

    public GameObject deathEffect;

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

    // Personal space
    public float personalSpaceRadius = 1f;
    public LayerMask personalSpaceMask;

    private Collider2D[] personalSpaceBuffer;

    protected float minDistanceToPoint = 0.1f;

    /// ==========================================
    protected override void Awake()
    {
        base.Awake();

        this.targetPosition = this.transform.position;
        this.isMoving = true;
        this._targetGridCoordinates = Vector2Int.zero;

        StartCoroutine(this.ShootLoop());

        this.personalSpaceBuffer = new Collider2D[4];
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
        bool needToMove = Vector3.Distance(this.transform.position, this.targetPosition) < this.minDistanceToPoint;

        if (needToMove == false)
        {
            this.isMoving = true;

            Vector3 path = this.targetPosition - this.transform.position;
            Vector3 direction = path.normalized;

            // Avoid other enemies
            int count = Physics2D.OverlapCircleNonAlloc(
                this.transform.position,
                this.personalSpaceRadius,
                this.personalSpaceBuffer,
                this.personalSpaceMask
            );

            Vector3 enemiesPush = Vector3.zero;
            for (int i = 0; i < count; i++)
            {
                if (this.personalSpaceBuffer[i].gameObject == this.gameObject)
                    continue;

                var otherEnemy = this.personalSpaceBuffer[i].transform;
                Vector3 goAwayDirection = this.transform.position - otherEnemy.position;
                float distanceToEnemy = goAwayDirection.sqrMagnitude;

                // Avoid zero division error
                distanceToEnemy = Mathf.Max(distanceToEnemy, .5f);

                goAwayDirection /= distanceToEnemy;

                enemiesPush += goAwayDirection;
            }

            direction += enemiesPush;


            // Avoid getting too small movement velocity
            float distanceToGridPoint = Mathf.Max(path.magnitude, this.minDistanceToPoint);
            direction *= Mathf.Min(this.speed, distanceToGridPoint);

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
        yield return new WaitForSeconds(3f);

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

        Instantiate(this.deathEffect, this.transform.position, Quaternion.identity);

        base.Die();
    }

    /// ==========================================
    protected virtual void OnDestinationReached() {}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, this.personalSpaceRadius);
    }
}
