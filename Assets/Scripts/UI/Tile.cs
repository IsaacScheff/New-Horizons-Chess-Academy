using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    [SerializeField] private Color _light, _dark;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _highlight;

    public void SetColor(bool isOffSet) {
        _spriteRenderer.color = isOffSet ? _dark : _light;
    }

    void OnMouseEnter() {
        _highlight.SetActive(true);
    }

    void OnMouseExit() {
        _highlight.SetActive(false);
    }
}
