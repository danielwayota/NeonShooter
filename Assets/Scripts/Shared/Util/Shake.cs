using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float speed = 0.1f;
    public float amplitude = 1;
    private float angle;

    private const float TAU = Mathf.PI * 2;

    private Vector3 pivot;

    // ==========================================
    void Start()
    {
        this.angle = TAU;

        this.pivot = this.transform.position;
    }

    public void DoTheShake()
    {
        this.angle = 0;
    }

    // ==========================================
    void Update()
    {
        if (this.angle < TAU)
        {
            this.angle += this.speed * Time.deltaTime;

            float x = Mathf.Sin(this.angle) * this.amplitude;

            this.transform.position = this.pivot + Vector3.right * x;
        }
    }
}
