using System.Collections;

using UnityEngine;

public class Enemy : DynamicEntity
{
    [Header("Enemy")]
    public float speed = 1f;

    public Weapon weapon;

    private Vector3 targetPosition;

    protected bool arrived
    {
        get => Vector3.Distance(this.transform.position, this.targetPosition) < this.minDistanceToPoint;
    }

    private float time = 0;
    private float timeOut = 0.5f;

    private float minDistanceToPoint = 0.1f;

    /// ==========================================
    protected override void Awake()
    {
        base.Awake();

        this.targetPosition = this.transform.position;

        StartCoroutine(this.ShootLoop());
    }

    /// ==========================================
    private void Update()
    {
        this.time += Time.deltaTime;
        if (this.time >= this.timeOut)
        {
            this.time -= this.timeOut;

            Vector3 path = this.targetPosition - this.transform.position;
            Vector3 direction = path.normalized;

            if (this.arrived == false)
            {
                // Avoid getting too small movement velocity
                float distance = Mathf.Max(path.magnitude, this.minDistanceToPoint);
                direction *= Mathf.Min(this.speed, distance);

                this.body.velocity = direction;
            }
            else
            {
                bool found = false;
                Vector3 newTarget = this.transform.position;

                float minDistance = 2 * this.minDistanceToPoint;

                while (found == false)
                {
                    newTarget = EnemyGrid.current.GetRandomPoint();

                    float distance = (newTarget - this.targetPosition).sqrMagnitude;
                    if (distance > minDistance)
                    {
                        found = true;
                    }
                }

                this.targetPosition = newTarget;
            }
        }
    }

    /// ==========================================
    IEnumerator ShootLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            this.weapon.Shoot();
        }
    }
}
