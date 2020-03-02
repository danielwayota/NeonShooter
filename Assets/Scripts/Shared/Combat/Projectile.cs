using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("Projectile")]
    public float speed;

    protected virtual void Awake()
    {
        var body = GetComponent<Rigidbody2D>();
        body.velocity = this.transform.up * this.speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(this.gameObject);

        var entity = other.gameObject.GetComponent<Entity>();

        if (entity == null)
            return;

        // TODO: Get this value from some where
        entity.DamageHealth(10);
    }
}
