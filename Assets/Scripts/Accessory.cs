using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DreamJam/Create Accessory")]
public class Accessory : ScriptableObject
{
    public bool sideAndBackSprites = false;
    public bool hideOnSideAndBack = false;
    public Sprite sprite;
    public Sprite frontSprite;
    public Sprite backSprite;
    
    public Vector2 positionOffset = Vector2.zero;
    public Vector2 scale = Vector2.one;
    public string name = "";
    public int price = 2;
}
