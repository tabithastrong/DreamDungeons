using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthBarUpdater : MonoBehaviour
{
    public TMP_Text healthbarText;
    public RawImage healthbarBackground;
    public RawImage healthbarFront;
    public PlayerInputSystem player;
    
    void Update() {
        float healthPercentage = (1f / player.startingHealth) * Mathf.Max(0f, player.health);
        float size = healthbarBackground.rectTransform.sizeDelta.x * healthPercentage;
        healthbarFront.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);

        healthbarText.text = string.Format("{0}/{1}", Mathf.Max(player.health, 0), player.startingHealth);
    }
}
