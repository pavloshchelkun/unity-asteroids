using UnityEngine;
using System.Collections;

public class BoundaryController : MonoBehaviour
{
    private void Start()
    {
        Camera mainCamera = Camera.main;

        // Calculate screen space corner points
        Vector3 topLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, mainCamera.nearClipPlane));
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, mainCamera.nearClipPlane));

        // Scale the boundary to fit screen space
        transform.localScale = new Vector3(Vector3.Distance(topLeft, topRight), 5f, Vector3.Distance(topLeft, bottomLeft));
    }

    private void OnTriggerExit(Collider other)
    {
        // Destroy projectiles when they leave screen space
        if (other.tag == Tags.Projectile)
        {
            Destroy(other.gameObject);
        }
        // Reposition other objects to an opposite side of the screen when they leave it
        else
        {
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(other.transform.position);
            Vector3 newPosition = other.transform.position;

            if (viewportPosition.x > 1f || viewportPosition.x < 0)
            {
                newPosition.x = -newPosition.x;
            }

            if (viewportPosition.y > 1f || viewportPosition.y < 0)
            {
                newPosition.z = -newPosition.z;
            }

            other.transform.position = newPosition;
        }
    }
}
