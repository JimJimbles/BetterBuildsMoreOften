using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1;
    public float responsiveness = 1;
    
    private InputManager inputManager;
    private Vector3 currentVelocity = Vector3.zero;

    public Health health;
    void Start()
    {
        inputManager = GetComponent<InputManager>();
        health = GetComponent<Health>();

        health.OnDie += OnDie;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DoMovement(inputManager.GetMoveDir());
    }

    public void DoMovement(Vector3 dir)
    {
        // converts move input to a worldspace vector based on our character's transform orientation
        Vector3 worldspaceMoveInput = transform.TransformVector(dir);

        Vector3 targetVelocity = worldspaceMoveInput * moveSpeed;

        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity,
            responsiveness * Time.deltaTime);
        transform.position += currentVelocity;
    }

    public void OnDie()
    {
        Debug.Log("You died");
        Application.Quit();
        
    }

}
