using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DrawWithMouse : MonoBehaviour
{
    private PlayerController _controller;
    
    [Header("Component")]
    private ArrowSpawner _arrowSpawner;
    private EdgeCollider2D _edgeCollider2D;
    private Vector3 _previousPosition;
    public LineRenderer LineRenderer { get; private set; }

    [Header("Setting")]
    private float _minDistance = 0.1f;
    [SerializeField, Range(0.01f, 2f) ] private float _width;
    
    private List<Vector3> _list = new();
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<PlayerController>();
        _arrowSpawner = GetComponent<ArrowSpawner>();
        LineRenderer = GetComponent<LineRenderer>();

        _edgeCollider2D = CreateEdge();
        
        LineRenderer.positionCount = 1;
        _previousPosition = transform.position;
        LineRenderer.startWidth = LineRenderer.endWidth = _width;
    }

    EdgeCollider2D CreateEdge()
    {
        var obj = new GameObject();
        obj.tag = "Edge";
        obj.name = $"{_controller.gameObject.name}'s Edge";
        
        var col = obj.AddComponent<EdgeCollider2D>();
        col.isTrigger = true;
        
        return col;
    }

    private void OnMouseDown()
    {
        // 기존 항목 초기화
        _controller.ClearAction();
        _arrowSpawner.ResetPool();
        _list.Clear();
        
        LineRenderer.positionCount = 1;
        LineRenderer.SetPosition(0, transform.position);
    }
    
    private void OnMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0f;

            if (Vector3.Distance(currentPosition, _previousPosition) > _minDistance)
            {
                if (_previousPosition == transform.position)
                {
                    LineRenderer.SetPosition(0, currentPosition);
                }
                else
                {
                    LineRenderer.positionCount++;
                    LineRenderer.SetPosition(LineRenderer.positionCount - 1, currentPosition); 
                }

                _previousPosition = currentPosition;
            }
        }
    }
    private void OnMouseUp()
    {
        for (int i = 0, cnt = LineRenderer.positionCount; i < cnt; i++)
        {
            var pos = LineRenderer.GetPosition(i);
            _list.Add(pos);
            _controller.AddMoveAction(pos, i, End);
        }
        SetEdgeCollider();
    }
    
    void End(int index)
    {
        if (index < _list.Count - 1)
        {
            var pos = LineRenderer.GetPosition(index + 1);
            for (int i = 0; i <= index; i++)
            {
                _list[i] = pos;
                LineRenderer.SetPosition(i, pos);
            }
            
            SetEdgeCollider();
        }
        else
        {
            LineRenderer.positionCount = 0; 
        }
    }
    
    
    void SetEdgeCollider()
    {
        _edgeCollider2D.SetPoints(_list.Select(x=> (Vector2)x).ToList());
    }
}
