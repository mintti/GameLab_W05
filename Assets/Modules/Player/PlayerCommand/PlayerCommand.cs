using System.Collections;
using UnityEngine;

public interface IPlayerCommand
{
    Vector3 MovePosition { get; set; }
    IEnumerator Execute();
}