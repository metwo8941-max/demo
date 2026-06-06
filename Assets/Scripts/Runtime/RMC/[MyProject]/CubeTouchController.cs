using UnityEngine;

public class CubeTouchController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.02f;

    private Camera _mainCamera;
    private Vector3 _offset;
    private bool _isDragging;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        // 触摸输入
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = _mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform == transform)
                    {
                        _isDragging = true;
                        _offset = transform.position - GetWorldPosition(touch.position);
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && _isDragging)
            {
                transform.position = GetWorldPosition(touch.position) + _offset;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _isDragging = false;
            }
        }
        // 鼠标输入（用于编辑器测试）
        else if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    _isDragging = true;
                    _offset = transform.position - GetWorldPosition(Input.mousePosition);
                }
            }
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            transform.position = GetWorldPosition(Input.mousePosition) + _offset;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }
    }

    private Vector3 GetWorldPosition(Vector3 screenPosition)
    {
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(
            screenPosition.x, 
            screenPosition.y, 
            _mainCamera.WorldToScreenPoint(transform.position).z
        ));
        return worldPosition;
    }
}