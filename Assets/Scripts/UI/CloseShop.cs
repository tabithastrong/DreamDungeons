using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseShop : MonoBehaviour
{
    public ShopTweener shopTweener;

    public void CloseShopButton() {
        shopTweener.open = false;
    }
}
