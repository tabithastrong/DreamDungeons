using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryLoader : MonoBehaviour
{
    public Accessory[] accessories;
    public Transform accessoryPanel;

    void Start()
    {
        for(int i = 0; i < accessories.Length; i++) {
            Transform panel = Instantiate(accessoryPanel);
            panel.SetParent(transform);

            RectTransform rectTransform = panel.GetComponent<RectTransform>();
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.x = i * 270;
            anchoredPosition.y = 0;
            rectTransform.anchoredPosition = anchoredPosition;

            PanelForItem panelForItem = panel.GetComponent<PanelForItem>();
            panelForItem.accessory = accessories[i];
        }

        GetComponent<RectTransform>().anchoredPosition = new Vector2((accessories.Length * 265f) / 2f, 0f);
        GetComponent<RectTransform>().sizeDelta = new Vector2((accessories.Length * 265f) + 100, 265f);    
    }
}
