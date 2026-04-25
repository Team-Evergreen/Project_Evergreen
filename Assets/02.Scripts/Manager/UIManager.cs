using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }

    [Header("Joystick UI")]
    public RectTransform joystickBG;     // 조이스틱 배경
    public RectTransform joystickHandle; // 조이스틱 핸들
    public float handleRange = 100f;     // 핸들이 움직일 수 있는 최대 반경

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        Init();
    }

    private void Init()
    {

    }

    // 조이스틱을 터치한 위치에 표시
    public void ShowJoystick(Vector2 _startPos)
    {
        //joystickBG.position = _startPos;
        joystickHandle.anchoredPosition = Vector2.zero;
    }

    // 핸들을 드래그 방향으로 이동
    public void UpdateHandle(Vector2 _startPos, Vector2 _currentPos)
    {
        Vector2 offset = _currentPos - _startPos;
        // 거리를 제한하여 핸들이 배경 밖으로 나가지 않도록함
        Vector2 clampedOffset = Vector2.ClampMagnitude(offset, handleRange);
        // UI 좌표계에 맞춰 핸들 위치 갱신
        joystickHandle.anchoredPosition = clampedOffset;
    }

    public void HideJoystick()
    {
        joystickBG.gameObject.SetActive(false);
    }
}