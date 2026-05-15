using UnityEngine;

public class CheckpointChecker : MonoBehaviour
{
    static int currentCheckpoint = 0;

    public int checkpointCount;

    public GameObject bultina;

    // current target checkpoint
    private Transform targetCheckpoint;

    // rotation smoothness
    public float rotationSpeed = 5f;

    void Start()
    {
        // Find all active GameObjects
        Object[] allObjects = FindObjectsByType(typeof(GameObject), FindObjectsSortMode.None);

        checkpointCount = 0;

        foreach (Object obj in allObjects)
        {
            if (obj.name.ToLower().Contains("checkpoint"))
            {
                checkpointCount++;
            }
        }

        Debug.Log("Total checkpoints found: " + checkpointCount);

        // Set first target checkpoint
        SetNextCheckpointTarget();
    }

    void Update()
    {
        // Rotate bultina toward target checkpoint
        if (bultina != null && targetCheckpoint != null)
        {
            Vector3 direction =
                targetCheckpoint.position - bultina.transform.position;

            // Ignore vertical rotation if wanted
            direction.y = 0;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation =
                    Quaternion.LookRotation(direction);

                bultina.transform.rotation = Quaternion.Slerp(
                    bultina.transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        string obj_name = other.gameObject.name.ToLower();

        if (obj_name.Contains("checkpoint"))
        {
            if (obj_name.EndsWith(currentCheckpoint + ""))
            {
                Destroy(other.gameObject);

                currentCheckpoint += 1;

                Debug.Log("Checkpoint OK: " + currentCheckpoint);

                if (currentCheckpoint == checkpointCount)
                {
                    Debug.Log("FINISHED!");

                    Destroy(this.gameObject);
                }
                else
                {
                    // Find next checkpoint
                    SetNextCheckpointTarget();
                }
            }
            else
            {
                Debug.Log("WRONG CHECKPOINT");
            }
        }
    }

    void SetNextCheckpointTarget()
    {
        string nextCheckpointName = "checkpoint_" + currentCheckpoint;

        GameObject nextCheckpoint =
            GameObject.Find(nextCheckpointName);

        if (nextCheckpoint != null)
        {
            targetCheckpoint = nextCheckpoint.transform;

            Debug.Log("Next checkpoint target: " + nextCheckpointName);
        }
        else
        {
            Debug.LogWarning(
                "Could not find next checkpoint: " +
                nextCheckpointName
            );
        }
    }
}