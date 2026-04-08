using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonHoverScale : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float speed = 10f;

    private Vector3 _originalScale;
    private Vector3 _targetScale;

    private void Awake()
    {
        _originalScale = transform.localScale;
        _targetScale = _originalScale;
    }

    private void OnEnable()
    {
        _targetScale = _originalScale;
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            _targetScale,
            Time.unscaledDeltaTime * speed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _targetScale = _originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _targetScale = _originalScale;
    }
}
