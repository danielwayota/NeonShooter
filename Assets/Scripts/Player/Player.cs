using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : DynamicEntity
{
    [Header("Player")]
    public float speed = 4f;
    public HUD hud;

    [Header("Player Combat")]
    public float stepsToPowerUp = 8;
    public Weapon[] weapons;

    [Header("Player sound")]
    public AudioSource shootSound;
    public AudioSource damageSound;
    public GameObject deathEffect;

    protected Weapon activeWeapon;

    protected Shake cameraShake;

    private int _killStreak;
    protected int killStreak
    {
        set {
            int weaponIndex = Mathf.FloorToInt(value / this.stepsToPowerUp);
            weaponIndex = Mathf.Clamp(weaponIndex, 0, this.weapons.Length - 1);

            this.SwitchWeapon(this.weapons[weaponIndex]);

            this._killStreak = value;
        }
        get => this._killStreak;
    }

    /// =============================================
    protected override void Awake()
    {
        base.Awake();

        this.killStreak = 0;

        this.hud.UpdateHealth(this.healthPercent);
        this.hud.UpdateKillStreak(this.killStreak);

        foreach (var weapon in this.weapons)
        {
            weapon.OnKill += this.OnKill;
        }

        this.cameraShake = GameObject.FindObjectOfType<Shake>();
    }

    /// =============================================
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (this.activeWeapon.Shoot())
                this.shootSound.Play();
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 movement = (new Vector3(h, v, 0)).normalized;

        this.body.velocity = movement * this.speed;
    }

    /// =============================================
    public override void DamageHealth(float amount)
    {
        this.cameraShake.DoTheShake();
        this.damageSound.Play();
        base.DamageHealth(amount);

        this.killStreak = 0;

        this.hud.UpdateKillStreak(this.killStreak);
        this.hud.UpdateHealth(this.healthPercent);
    }

    /// =============================================
    protected void SwitchWeapon(Weapon weapon)
    {
        if (this.activeWeapon != null)
            this.activeWeapon.gameObject.SetActive(false);

        this.activeWeapon = weapon;

        if (this.activeWeapon != null)
            this.activeWeapon.gameObject.SetActive(true);
    }

    /// =============================================
    protected void OnKill()
    {
        this.killStreak++;

        this.hud.UpdateKillStreak(this.killStreak);
    }

    /// =============================================
    protected override void Die()
    {
        GameManager.current.OnPlayerDeath();

        Instantiate(this.deathEffect, this.transform.position, Quaternion.identity);
        this.hud.gameObject.SetActive(false);

        base.Die();
    }
}
