using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTweener : MonoBehaviour
{
    public Vector2 openPosition = Vector2.zero;
    public Vector2 closedPosition = new Vector2(1664, 0);

    public bool open = false;
    public float tweenSpeed = 1.2f;

    void Update() {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 anchorPos = rectTransform.anchoredPosition;

        if(open) {
            anchorPos = Vector2.Lerp(anchorPos, openPosition, tweenSpeed * Time.deltaTime);   
        } else {
            anchorPos = Vector2.Lerp(anchorPos, closedPosition, tweenSpeed * Time.deltaTime);
        }

        rectTransform.anchoredPosition = anchorPos;
    }
}
