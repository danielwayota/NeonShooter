using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float time = 1f;

    private void Awake()
    {
        Destroy(this.gameObject, this.time);
    }
}