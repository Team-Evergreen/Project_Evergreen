using UnityEngine;

public class TruckController : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float arriveDistance = 0.05f;

    private TruckStopZone currentTargetZone;
    private bool isMoving;

    public bool IsMoving => isMoving;

    public void MoveTo(TruckStopZone targetZone)
    {
        if (targetZone == null)
        {
            Debug.LogWarning("Truck target zone is null");
            return;
        }

        currentTargetZone = targetZone;
        isMoving = true;
    }

    public void StopAtZone()
    {
        isMoving = false;
        currentTargetZone = null;
    }

    private void Update()
    {
        if (!isMoving || currentTargetZone == null)
            return;

        Vector3 target = currentTargetZone.TargetPosition;
        target.z = transform.position.z;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) <= arriveDistance)
        {
            currentTargetZone.SetReached();
            StopAtZone();
        }
    }
}