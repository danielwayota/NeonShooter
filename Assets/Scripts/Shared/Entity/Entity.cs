using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity")]
    public float maxHealth = 100;

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
    public void DamageHealth(float amount)
    {
        this.ModifyHealth(-amount);
    }

    /// =============================================
    protected virtual void Die()
    {
        Destroy(this.gameObject);
    }
}