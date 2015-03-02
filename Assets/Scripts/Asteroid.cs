using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour
{
    public float speed;
    public float tumble;

    public GameObject explosion;
    public int scoreValue;
    public int piecesOnDestroy;

    private int size;

    public void SetSize(int asteroidSize)
    {
        size = asteroidSize;
        transform.localScale = Vector3.one * asteroidSize;
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        rigidbody.velocity = transform.forward * speed;
        rigidbody.angularVelocity = Random.insideUnitSphere * tumble;
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != Tags.Projectile && other.tag != Tags.Player)
        {
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (other.tag == Tags.Player)
        {
            var player = other.GetComponent<PlayerController>();
            player.Kill();
        }
        else
        {
            GameController.Instance.AddScore(scoreValue * size);
            Destroy(other.gameObject);
        }

        if (size > 1)
        {
            for (int i = 0; i < piecesOnDestroy; i++)
            {
                GameController.Instance.CreateAsteroid(transform.position, size - 1);
            }
        }

        GameController.Instance.OnAsteroidDestroyed();
        Destroy(gameObject);
    }
}
