using System.Collections;
using UnityEngine;

public class MovingPlatformWithPlayer : MonoBehaviour
{
    [Header("Movement Positions")]
    [Tooltip("Start position of the platform (world coordinates)")]
    public Vector3 startPosition;

    [Tooltip("End position of the platform (world coordinates)")]
    public Vector3 endPosition;

    [Header("Movement Settings")]
    [Tooltip("Units per second the platform moves")]
    public float speed = 2f;

    [Tooltip("Move back and forth between points if true")]
    public bool pingPong = true;

    [Tooltip("Optional pause at each point in seconds")]
    public float waitTime = 0f;

    private Vector3 targetPosition;
    private bool movingToEnd = true;
    private bool isWaiting = false;

    void Start()
    {
        transform.position = startPosition;
        targetPosition = endPosition;
    }

    void Update()
    {
        if (isWaiting) return;

        // Move toward target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if reached target
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            if (waitTime > 0f)
                StartCoroutine(WaitAtPoint());
            else
                SwitchTarget();
        }
    }

    private void SwitchTarget()
    {
        if (pingPong)
            movingToEnd = !movingToEnd;

        targetPosition = movingToEnd ? endPosition : startPosition;
    }

    private IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        SwitchTarget();
    }

    // Handle player riding the platform
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Check if the player is standing on top
            if (collision.contacts[0].normal.y > 0.5f)
            {
                collision.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    // Visualize positions in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startPosition, 0.2f);
        Gizmos.DrawSphere(endPosition, 0.2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startPosition, endPosition);
    }
}

