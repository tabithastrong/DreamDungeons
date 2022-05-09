using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PanelForItem : MonoBehaviour
{
    public Accessory accessory;
    public TMP_Text titleText;
    public RawImage itemImage;
    public TMP_Text costText;
    public RawImage lockIcon;
    public TMP_Text equippedText;

    void Update()
    {
        if(PlayerPrefs.GetInt("Accessory/" + accessory.name) == 1) {
            lockIcon.enabled = false;
        } else {
            lockIcon.enabled = true;
        }

        string accessoryEquipped = PlayerPrefs.GetString("AccessoryEquipped", "");
        if(accessoryEquipped != "") {
            if(accessory.name == accessoryEquipped) {
                equippedText.enabled = true;
            } else {
                equippedText.enabled = false;
            }
        } else {
            equippedText.enabled = false;
        }

        titleText.text = accessory.name;
        itemImage.texture = accessory.sprite.texture;
        costText.text = "" + accessory.price;
    }

    public void Clicked() {
        bool locked = PlayerPrefs.GetInt("Accessory/" + accessory.name) == 0;
        int tokens = PlayerPrefs.GetInt("Tokens", 0);

        if(locked && accessory.price <= tokens) {
            PlayerPrefs.SetInt("Tokens", tokens - accessory.price);
            PlayerPrefs.SetInt("Accessory/" + accessory.name, 1);
        } else if(!locked) {
            string accessoryEquipped = PlayerPrefs.GetString("AccessoryEquipped", "");

            if(accessoryEquipped == accessory.name) {
                PlayerPrefs.SetString("AccessoryEquipped", "");
            } else {
                PlayerPrefs.SetString("AccessoryEquipped", accessory.name);
            }
        }
    }
}
