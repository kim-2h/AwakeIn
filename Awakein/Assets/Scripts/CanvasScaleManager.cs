using UnityEngine;
using UnityEngine.UI;

// #if UNITY_IOS
// #define TARGET_IOS
// #elif UNITY_STANDALONE_WIN
// #define TARGET_WINDOWS
// #endif

public class CanvasScalerManager : MonoBehaviour
{
    // 원하는 기준 해상도 설정 (에디터 해상도와 동일하게 설정)
    public Vector2 referenceResolution = new Vector2(1920, 1080);
    
    // CanvasScaler의 Scale Mode 설정
    public CanvasScaler.ScaleMode targetScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

    // Screen Match Mode 설정 (0: 너비에 맞춤, 1: 높이에 맞춤)
    [Range(0, 1)]
    public float matchWidthOrHeight = 0.5f; // 기본값은 너비와 높이를 균등하게 맞춤

    void Awake()
    {
        ApplyCanvasScalerSettings();
    }

    void ApplyCanvasScalerSettings()
    {
        // 모든 Canvas 찾기
        Canvas[] allCanvases = FindObjectsOfType<Canvas>(true);
        foreach (Canvas canvas in allCanvases)
        {
            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler != null)
            {
                // Scale Mode 설정
                scaler.uiScaleMode = targetScaleMode;

                if (targetScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
                {
                    // 플랫폼별 Reference Resolution 설정
#if TARGET_IOS
                    scaler.referenceResolution = new Vector2(1334, 750); // 예: iPhone 6 해상도
#elif TARGET_WINDOWS
                    scaler.referenceResolution = referenceResolution; // 기본값 사용
#else
                    scaler.referenceResolution = referenceResolution; // 기본값 사용
#endif
                    // Screen Match Mode 설정
                    scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    scaler.matchWidthOrHeight = matchWidthOrHeight;
                }

                // 필요시 추가 설정 가능
            }
            else
            {
                Debug.LogWarning($"CanvasScaler가 없는 Canvas가 발견되었습니다: {canvas.name}");
            }
        }
    }
}
