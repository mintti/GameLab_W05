using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TowardToAngle : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform arrowPivotTr; 

    public void Detect(Quaternion Angles)
    {
        arrowPivotTr.rotation = Quaternion.Slerp(transform.rotation, Angles, 5);
        _spriteRenderer ??= GetComponent<SpriteRenderer>();
        _spriteRenderer.flipX = arrowPivotTr.rotation.eulerAngles.z > 90 && arrowPivotTr.rotation.eulerAngles.z < 270;
    }
}
