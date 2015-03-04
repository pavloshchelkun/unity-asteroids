using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

	void Start ()
    {
        // Set initial velocity by speed
        rigidbody.velocity = transform.forward * speed;
	}
}
