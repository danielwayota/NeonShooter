using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public string poolName;

    [Header("Projectile")]
    public float speed;
    public Weapon weapon;

    private Rigidbody2D body;

    /// ==================================================
    private void OnEnable()
    {
        if (this.body == null)
        {
            this.body = GetComponent<Rigidbody2D>();
        }
        this.body.velocity = this.transform.up * this.speed;
    }

    /// ==================================================
    float time = 0;
    private void Update()
    {
        this.time += Time.deltaTime;

        if (this.time > 0.25f)
        {
            this.time = 0;
            if (Pool.IsOutOfBounds(this.transform.position))
                Pool.Destroy(this.poolName, this.gameObject);
        }
    }

    /// ==================================================
    private void OnDisable()
    {
        this.body.velocity = Vector3.zero;
    }

    /// ==================================================
    private void OnCollisionEnter2D(Collision2D other)
    {
        Pool.Destroy(this.poolName, this.gameObject);

        var entity = other.gameObject.GetComponent<Entity>();

        if (entity == null)
            return;

        this.weapon.OnProjectileHit(entity);
    }
}
