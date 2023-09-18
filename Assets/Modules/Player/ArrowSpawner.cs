using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab; // Arrow 프리팹을 연결하세요.
    private GameObject currentArrow; // 현재 드래그 중인 Arrow

    private List<GameObject> _arrowPool = new();
    
    private bool isDragging = false;
    private Vector3 initialMousePosition;
    private Vector3 arrowSpawnPosition;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isDragging) // 마우스 왼쪽 버튼을 눌렀을 때
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
    
            // Raycast를 실행하여 충돌체가 있는지 확인합니다.
           // RaycastHit2D hit = Physics2D.OverlapCircleAll (mousePosition, (-mousePosition).normalized);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, .5f);
            
            // 충돌체가 있다면 해당 객체를 출력합니다.
            if (colliders.FirstOrDefault() != null )
            {
                var col = colliders.First(); 
                if (col.CompareTag("Edge") || col.CompareTag("Player"))
                {
                    isDragging = true;
    
                    // Arrow를 생성하고 초기 위치를 설정합니다.
                    currentArrow = Instantiate(arrowPrefab);
                    
                    initialMousePosition = Input.mousePosition;
                    arrowSpawnPosition = Camera.main.ScreenToWorldPoint(initialMousePosition);
                    arrowSpawnPosition.z = 0f; // z 값을 0으로 설정하여 화면에 표시합니다.
                    currentArrow.transform.position = arrowSpawnPosition;
                    
                    _arrowPool.Add(currentArrow); // 오브젝트 관리
                }
            }
        }
    
        if (isDragging)
        {
            // 현재 마우스 위치와 초기 마우스 위치를 비교하여 회전값을 계산합니다.
            Vector3 mouseDelta = Input.mousePosition - initialMousePosition;
            float angle = Mathf.Atan2(mouseDelta.y, mouseDelta.x) * Mathf.Rad2Deg;

            try
            {
                // Arrow의 회전을 설정합니다.
                currentArrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            catch (Exception e)
            {
                // nothing
            }

    
            if (Input.GetMouseButtonUp(1)) // 마우스 왼쪽 버튼을 놓았을 때
            {
                isDragging = false;
                currentArrow.GetComponent<UIUPArrow>().IsActive = true;
            }
        }
    }

    public void ResetPool()
    {
        foreach (var arwObj in _arrowPool)
        {
            Destroy(arwObj);
        }
        _arrowPool.Clear();
    }
}