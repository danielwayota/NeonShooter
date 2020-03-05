using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon")]
    public float recoilTime = 1f;
    public GameObject projectilePrfb;

    [Header("Shoot Points")]
    public Transform[] shootPoints;

    public bool ready
    {
        get => this.time >= this.recoilTime;
    }

    protected float time;

    /// =============================================
    private void Awake()
    {
        this.time = this.recoilTime;
    }

    /// =============================================
    private void Update()
    {
        if (this.ready == false)
        {
            this.time += Time.deltaTime;
        }
    }

    /// =============================================
    public void Shoot()
    {
        if (this.ready == false)
            return;

        this.time = 0;

        if (this.shootPoints.Length == 0)
            throw new System.Exception($"The {this.name} weapon has no shootpoints!");

        foreach (var shootPoint in this.shootPoints)
        {
            var projectile = Pool.Instantiate(
                this.projectilePrfb.name,
                shootPoint.position,
                shootPoint.rotation
            );
        }
    }
}
