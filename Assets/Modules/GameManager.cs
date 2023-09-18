using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    private bool _isPause; 
    public bool IsPause {
        get => _isPause;
        set
        {
            _isPause = value;
            Time.timeScale = _isPause ? 1 : 0;
        }
    }
    private void Awake()
    {
        if (I == null)
        {
            I = this;
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // public void OnPause(InputAction.CallbackContext context)
    // {
    //     if (context.performed)
    //     {
    //         IsPause = !IsPause;
    //     }
    // }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsPause = !IsPause;
        }
    }
}
