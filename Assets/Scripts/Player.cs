using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] CharacterMovement charMovement;
    [SerializeField] Rotator rotator;

    bool startPositionSetted;
    Vector3 respawnPosition;

    public static Player instance;

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    

    void Start()
    {
        respawnPosition = transform.position;
        startPositionSetted = true;
        rotator.enabled = false;
    }

    private void OnDisable()
    {
        InputManager.instance.RemoveHorizontalInput(SetHorizontalDirection);
        InputManager.instance.RemoveVerticalInput(SetVerticalDirection);
    }

    private void SetVerticalDirection(float axis)
    {
        charMovement.SetVerticalDirection(axis);
    }

    private void SetHorizontalDirection(float axis)
    {
        charMovement.SetHorizontalDirection(axis);
    }

    internal void Initialize()
    {
        if(startPositionSetted)
            transform.position = respawnPosition;
        rotator.enabled = false;
        transform.rotation = Quaternion.identity;
        spriteRenderer.enabled = true;
        charMovement.Stop();
    }

    internal void StartLevel()
    {
        InputManager.instance.SetHorizontalInput(SetHorizontalDirection);
        InputManager.instance.SetVerticalInput(SetVerticalDirection);
    }

    internal void Vanish()
    {
        spriteRenderer.enabled = false;
    }

    internal void Stop()
    {
        charMovement.Stop();
        InputManager.instance.RemoveHorizontalInput(SetHorizontalDirection);
        InputManager.instance.RemoveVerticalInput(SetVerticalDirection);
    }

    internal void Kill()
    {
        Stop();
        rotator.enabled = true;
    }
}
