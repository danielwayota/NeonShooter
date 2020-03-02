using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [Tooltip("If this field is blank, use the current GameObject")]
    public Transform target;

    public float speed;

    private Transform receiver;

    private void Awake()
    {
        this.receiver = this.target != null ? this.target : this.transform;
    }

    private void Update()
    {
        this.transform.Rotate(Vector3.forward * this.speed);
    }
}
