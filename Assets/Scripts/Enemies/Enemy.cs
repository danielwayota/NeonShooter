using System.Collections;

using UnityEngine;

public class Enemy : DynamicEntity
{
    [Header("Enemy")]
    public float speed = 1f;

    public Weapon weapon;

    protected override void Awake()
    {
        base.Awake();

        this.body.velocity = Vector3.down * this.speed;

        StartCoroutine(this.ShootLoop());
    }

    private void Update()
    {

    }

    IEnumerator ShootLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            this.weapon.Shoot();
        }
    }
}
