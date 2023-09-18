using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrowPrefab; // Arrow 프리팹을 연결하세요.
    private GameObject currentArrow; // 현재 드래그 중인 Arrow
    
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
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, (-mousePosition).normalized);
            
            // 충돌체가 있다면 해당 객체를 출력합니다.
            if (hit.collider != null )
            {
                 if (hit.collider.name == "Edge")
                {
                    isDragging = true;
    
                    // Arrow를 생성하고 초기 위치를 설정합니다.
                    currentArrow = Instantiate(arrowPrefab);
                    initialMousePosition = Input.mousePosition;
                    arrowSpawnPosition = Camera.main.ScreenToWorldPoint(initialMousePosition);
                    arrowSpawnPosition.z = 0f; // z 값을 0으로 설정하여 화면에 표시합니다.
                    currentArrow.transform.position = arrowSpawnPosition;
                }
            }
        }
    
        if (isDragging)
        {
            // 현재 마우스 위치와 초기 마우스 위치를 비교하여 회전값을 계산합니다.
            Vector3 mouseDelta = Input.mousePosition - initialMousePosition;
            float angle = Mathf.Atan2(mouseDelta.y, mouseDelta.x) * Mathf.Rad2Deg;
    
            // Arrow의 회전을 설정합니다.
            currentArrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    
            if (Input.GetMouseButtonUp(1)) // 마우스 왼쪽 버튼을 놓았을 때
            {
                isDragging = false;
            }
        }
    }

    // private GameObject currentArrow;
    // private Vector3 startPos;
    // private bool isDrawing = false;
    //
    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(1)) // 마우스 우클릭을 누를 때
    //     {
    //         StartDrawingArrow();
    //     }
    //     else if (Input.GetMouseButton(1)) // 마우스 우클릭을 누른 상태에서 마우스를 이동할 때
    //     {
    //         UpdateArrowScale();
    //     }
    //     else if (Input.GetMouseButtonUp(1)) // 마우스 우클릭을 뗄 때
    //     {
    //         StopDrawingArrow();
    //     }
    // }
    //
    // void StartDrawingArrow()
    // {
    //     Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //     mousePosition.z = 0f;
    //     
    //     // Raycast를 실행하여 충돌체가 있는지 확인합니다.
    //     RaycastHit2D hit = Physics2D.Raycast(mousePosition, (-mousePosition).normalized);
    //     
    //     // 충돌체가 있다면 해당 객체를 출력합니다.
    //     if (hit.collider != null )
    //     {
    //         if (hit.collider.name == "Edge")
    //         {
    //             startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //             startPos.z = 0;
    //             currentArrow = Instantiate(arrowPrefab, startPos, Quaternion.identity);
    //             isDrawing = true;
    //         }
    //         
    //     }
    // }
    //
    // void UpdateArrowScale()
    // {
    //     if (isDrawing)
    //     {
    //         Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //         currentPos.z = 0;
    //         float arrowLength = (currentPos - startPos).magnitude;
    //         currentArrow.transform.localScale = new Vector3(arrowLength, 1, 1);
    //     }
    // }
    //
    // void StopDrawingArrow()
    // {
    //     isDrawing = false;
    // }
}