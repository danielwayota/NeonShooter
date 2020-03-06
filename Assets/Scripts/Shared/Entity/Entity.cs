using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity")]
    public float maxHealth = 10;

    protected float healthPercent
    {
        get => this.health / this.maxHealth;
    }

    public bool isAlive
    {
        get => this.health > 0;
    }

    protected float health;

    /// =============================================
    protected virtual void Awake()
    {
        this.health = this.maxHealth;
    }

    /// =============================================
    public void ModifyHealth(float amount)
    {
        this.health = Mathf.Clamp(this.health + amount, 0, this.maxHealth);

        if (this.health == 0)
        {
            this.Die();
        }
    }

    /// =============================================
    public void RestoreHealth(float amount)
    {
        this.ModifyHealth(amount);
    }

    /// =============================================
    public virtual void DamageHealth(float amount)
    {
        this.ModifyHealth(-amount);
    }

    /// =============================================
    protected virtual void Die()
    {
        Destroy(this.gameObject);
    }
}