using System;
using UnityEngine;

public class TruckStopZone : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public event Action OnTruckReached;

    public Vector3 TargetPosition => transform.position;

    private bool isReached;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        SetWaiting();
    }

    public void SetWaiting()
    {
        isReached = false;

        if (spriteRenderer != null)
            spriteRenderer.color = new Color(1f, 0f, 0f, 0.35f);
    }

    public void SetReached()
    {
        if (isReached)
            return;

        isReached = true;

        if (spriteRenderer != null)
            spriteRenderer.color = new Color(0f, 0.45f, 1f, 0.35f);

        OnTruckReached?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out TruckController truck))
            return;

        truck.StopAtZone();
        SetReached();

        Debug.Log("Truck Stop Zone 도착");
    }
}