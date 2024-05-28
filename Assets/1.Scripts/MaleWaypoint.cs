using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaleWaypoint : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float moveTime = 3f;
    private float currentMoveTime = 0f;
    private bool isMoving = true;

    void Start()
    {
        StartCoroutine(MoveAndTurn());
    }

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    IEnumerator MoveAndTurn()
    {
        while (true)
        {
            // Move forward for a specified time
            currentMoveTime = 0f;
            while (currentMoveTime < moveTime)
            {
                currentMoveTime += Time.deltaTime;
                yield return null;
            }

            // Stop moving and start turning around
            isMoving = false;
            Quaternion initialRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0, 180, 0) * initialRotation;
            while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                yield return null;
            }

            // Ensure the rotation is exactly what we want in case of any precision errors
            transform.rotation = targetRotation;

            // Resume moving in the new direction
            isMoving = true;
            yield return null;  // Optional: Add a small delay before starting to move again
        }
    }
}
