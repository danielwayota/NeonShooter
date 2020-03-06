using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : DynamicEntity
{
    [Header("Player")]
    public float speed = 4f;

    [Header("Player Combat")]
    public Weapon activeWeapon;

    /// =============================================
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            this.activeWeapon.Shoot();
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 movement = (new Vector3(h, v, 0)).normalized;

        this.body.velocity = movement * this.speed;
    }

    protected override void Die()
    {
        GameManager.current.OnPlayerDeath();

        base.Die();
    }
}
