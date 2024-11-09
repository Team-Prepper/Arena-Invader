using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f; // WASD 이동 속도
    [SerializeField] private Vector2 moveLimitsX = new Vector2(-20, 20); // X축 이동 범위
    [SerializeField] private Vector2 moveLimitsZ = new Vector2(-20, 20); // Z축 이동 범위

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f; // 줌 속도
    [SerializeField] private float minZoom = 5f; // 최소 줌 거리
    [SerializeField] private float maxZoom = 20f; // 최대 줌 거리

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        MoveCamera();
        ZoomCamera();
    }

    private void MoveCamera()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // WASD 키 입력 처리
        if (Input.GetKey(KeyCode.W)) moveZ += moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) moveZ -= moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) moveX -= moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) moveX += moveSpeed * Time.deltaTime;

        // 현재 위치를 기준으로 이동
        Vector3 newPosition = transform.position + new Vector3(moveX, 0, moveZ);

        // 이동 범위 제한
        newPosition.x = Mathf.Clamp(newPosition.x, moveLimitsX.x, moveLimitsX.y);
        newPosition.z = Mathf.Clamp(newPosition.z, moveLimitsZ.x, moveLimitsZ.y);

        transform.position = newPosition;
    }

    private void ZoomCamera()
    {
        // 마우스 휠 입력으로 줌 제어
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            float newZoom = cam.orthographicSize - scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
        }
    }
}