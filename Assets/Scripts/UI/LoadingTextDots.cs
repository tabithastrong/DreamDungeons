using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingTextDots : MonoBehaviour
{

    float timer = 0f;

    string dots = "";

    TMPro.TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > 0.6f) {
            timer = 0f;

            if(dots.Length == 3) {
                dots = "";
            } else {
                dots += ".";
            }

            text.text = "Loading" + dots;
        }    
    }
}
