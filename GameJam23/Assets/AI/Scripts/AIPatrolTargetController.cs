using UnityEngine;

public class AIPatrolTargetController : MonoBehaviour
{
    [SerializeField]
    private Transform patrolTargetTransform;
    [SerializeField]
    private float timeBetweenRotations;

    private void Start()
    {
        InvokeRepeating("RotatePatrolTarget", 0f, timeBetweenRotations);
    }

    private void RotatePatrolTarget()
    {
        patrolTargetTransform.SetLocalPositionAndRotation(
            new Vector3(-patrolTargetTransform.localPosition.x, patrolTargetTransform.localPosition.y, patrolTargetTransform.localPosition.z),
            Quaternion.identity
        );
    }
}
