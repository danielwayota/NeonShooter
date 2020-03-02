using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DynamicEntity : Entity
{
    // Movement
    protected Rigidbody2D body;

    /// =============================================
    protected override void Awake()
    {
        base.Awake();

        this.body = this.GetComponent<Rigidbody2D>();
    }
}
