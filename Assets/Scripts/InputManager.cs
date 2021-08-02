using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public delegate void OnPressAxis(float axis);
    OnPressAxis getHorizontalAxisRaw;
    OnPressAxis getVerticalAxisRaw;


    public static InputManager instance;


    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        if (getHorizontalAxisRaw != null)
            getHorizontalAxisRaw.Invoke(Input.GetAxisRaw("Horizontal"));
        if (getVerticalAxisRaw != null)
            getVerticalAxisRaw.Invoke(Input.GetAxisRaw("Vertical"));
    }

    public void SetHorizontalInput(OnPressAxis action)
    {
        getHorizontalAxisRaw = action;
    }

    public void SetVerticalInput(OnPressAxis action)
    {
        getVerticalAxisRaw = action;
    }

    public void RemoveHorizontalInput(OnPressAxis action)
    {
        getHorizontalAxisRaw -= action;
    }

    public void RemoveVerticalInput(OnPressAxis action)
    {
        getVerticalAxisRaw -= action;
    }

}

