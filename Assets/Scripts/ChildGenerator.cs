using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildGenerator : MonoBehaviour
{
    public Sprite[] outfits;
    public Sprite[] skins;
    public Sprite[] hairs;
    public Sprite happy;
    public Sprite sad;
    
    public SpriteRenderer outfitRenderer;
    public SpriteRenderer skinRenderer;
    public SpriteRenderer hairRenderer;
    public SpriteRenderer faceRenderer;

    public bool isHappy = false;

    void Start()
    {
        Sprite outfit = outfits[Random.Range(0, outfits.Length)];
        Sprite skin = skins[Random.Range(0, skins.Length)];
        Sprite hair = hairs[Random.Range(0, hairs.Length)]; 

        outfitRenderer.sprite = outfit;
        skinRenderer.sprite = skin;
        hairRenderer.sprite = hair;
        faceRenderer.sprite = sad;   
    }

    void Update() {
        faceRenderer.sprite = isHappy ? happy : sad;
    }
}
