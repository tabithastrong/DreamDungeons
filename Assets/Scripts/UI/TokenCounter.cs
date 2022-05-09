using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenCounter : MonoBehaviour
{
    TMPro.TMP_Text text;

    void Start()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = string.Format("{0}", PlayerPrefs.GetInt("Tokens", 0));
    }
}
