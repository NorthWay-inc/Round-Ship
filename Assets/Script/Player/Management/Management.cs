using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UniRx;

public class Management : MonoBehaviour
{
    [SerializeField] public IntReactiveProperty Money = new IntReactiveProperty(10);

    [Header("Player Setting Cell")]
    [CanBuy("player", "Speed", "Increase your speed", 10,10)]
    public int SpeedLevel = 1;
    [SerializeField] public float speed = 0f;

    [SerializeField, Range(0.01f, 1f)] private float movemetnSpeed = 0.7f;


    [CanBuy("Player", "Ahaha", "Increase your speed", 10,5)]
    [SerializeField] public int CurrentHealthLevel = 1;


    [SerializeField] public int MaxHealthLevel = 6;
    private int _minHealth = 10;



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
