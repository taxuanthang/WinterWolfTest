using UnityEngine;
using DG.Tweening;

public class UIFloatUp : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float moveDistance = 100f;
    [SerializeField] private float duration = 0.8f;
    [SerializeField] private Ease easeType = Ease.OutCubic;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        Play();
    }

    public void Play()
    {
        rectTransform.DOKill();
        canvasGroup.DOKill();

        Vector2 startPos = rectTransform.anchoredPosition;

        Sequence seq = DOTween.Sequence();

        seq.Append(
            rectTransform.DOAnchorPosY(startPos.y + moveDistance, duration)
                .SetEase(easeType)
        );

        seq.Join(
            canvasGroup.DOFade(0f, duration)
        );

        seq.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}