using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Management : MonoBehaviour
{
    [Header("Player Setting Cell")]
    [SerializeField] private float speed = 0f;
    [SerializeField, Range(0.01f, 1f)] private float movemetnSpeed = 0.7f;

    [SerializeField] private Transform playerRotation;

    [HideInInspector] private bool direction;
    [SerializeField] public bool isUseButton;
    public void ManagementButton(bool isRight) { direction = isRight; isUseButton = true; }
    public void DontMove() { isUseButton = false; }

    private void Update()
    {
        Move();

        if (!isUseButton) { return; }

        MovementPlayer();
    }

    public void MovementPlayer()
    {
        if (direction == true) { speed += movemetnSpeed * Time.deltaTime; }
        else { speed -= movemetnSpeed * Time.deltaTime; }
    }

    private void Move()
    {
        playerRotation.Rotate(new Vector3(0, speed, 0));

        if (speed > 0) { speed -= 0.45f * Time.deltaTime; }
        if (speed < 0) { speed += 0.45f * Time.deltaTime; }
    }
}
