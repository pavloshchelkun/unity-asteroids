using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour
{
    public float speed;
    public float tumble;

    void Start()
    {
        rigidbody.velocity = transform.forward * speed;
        rigidbody.angularVelocity = Random.insideUnitSphere * tumble;
	}
}
