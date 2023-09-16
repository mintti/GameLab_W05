using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    public GameObject Player { get; set; }
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
    
    
}
