using UnityEditor;
using UnityEngine;

public class Spot : MonoBehaviour {
    public SpotsGroup _spotGroup;

    public enum SpotType {
        NONE,
        PLANT,
        TOP,
        SIDE
    }

    [SerializeField] public SpotType _spotType;

    public enum SpotSubType {
        NONE,
        LEFT,
        RIGHT
    }
    public SpotSubType _spotSubType;

    public enum ProtectionType {
        NONE,
        LIGHT,
        DARKNESS,
        ALL
    }

    private ProtectionType _protectionType = ProtectionType.NONE;

    bool _enable = false;

    SpriteRenderer _spriteRenderer;

    [SerializeField] float _fadeAlpha;
    [SerializeField] float _highlightAlpha;

    bool _occupied = false;

    private void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Enable(false);
        _spotGroup = GetComponentInParent<SpotsGroup>();
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

    public void ReleaseSpot() {
        _occupied = false;
        _protectionType = ProtectionType.NONE;
    }

    public bool IsItTaken() {
        return _occupied || (_spotType != SpotType.PLANT ? !_spotGroup.IsPlantInSpot() : false);
    }

    public void SetProtectionType(ProtectionType protectionType) {
        _protectionType = protectionType;
    }

    public ProtectionType GetProtectionType() {
        return _protectionType;
    }
}
