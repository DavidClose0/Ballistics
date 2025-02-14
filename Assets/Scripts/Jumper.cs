using System;
using UnityEngine;

public class PlatformJumper : MonoBehaviour
{
    public GameObject[] platforms;
    public float muzzleVelocity = 10f;
    public Vector3 gravity = Physics.gravity;
    public float jumpDelay = 2f;

    private Rigidbody rb;
    private FiringSolution firingSolution;
    private int platformIndex = 0;
    private float timer = 0f;
    private bool isJumping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        firingSolution = new FiringSolution();
        timer = jumpDelay;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= jumpDelay && !isJumping)
        {
            if (platforms.Length > 0)
            {
                Vector3 startPosition = transform.position;
                Vector3 endPosition = platforms[platformIndex].transform.position;

                Nullable<Vector3> solution = firingSolution.Calculate(startPosition, endPosition, muzzleVelocity, gravity);

                if (solution.HasValue)
                {
                    Vector3 launchVelocity = solution.Value * muzzleVelocity;
                    rb.AddForce(launchVelocity, ForceMode.Impulse);
                    isJumping = true; // Set jumping flag
                }

                timer = 0f; // Reset timer even if no solution found to try again after delay
                platformIndex = (platformIndex + 1) % platforms.Length; // Move to next platform target
            }

            isJumping = false; // Reset jumping flag after attempt
        }
    }
}