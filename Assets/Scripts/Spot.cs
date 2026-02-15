using UnityEditor;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public enum SpotType {
        NONE,
        PLANT,
        LEFT,
        RIGHT,
        TOP
    }

    [SerializeField] public SpotType _spotType;

    bool _enable = false;

    SpriteRenderer _spriteRenderer;

    [SerializeField] float _fadeAlpha;
    [SerializeField] float _highlightAlpha;

    bool _occupied = false;
    private void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Enable(false);
    }
    public void Highlight() {
        if(!_enable)
            return;

        _spriteRenderer.color = new Color(1f, 1f, 1f, _highlightAlpha);
    }

    public void Fade() {
        if(!_enable)
            return;
        _spriteRenderer.color = new Color(1f, 1f, 1f, _fadeAlpha);
    }

    public void Enable(bool enable) {
        _spriteRenderer.color = new Color(1f, 1f, 1f, enable ? _fadeAlpha : 0f);
        _enable = enable;
    }

    public void OccupiedSpot() {
        _occupied = true;
    }

    public bool IsItTaken() {
        return _occupied;
    }
}
