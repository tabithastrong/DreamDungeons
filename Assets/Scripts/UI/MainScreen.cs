using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    public Animator faderAnimator;
    public GameObject loadingText;
    public ShopTweener shopTweener;

    void Start() {
        faderAnimator.gameObject.SetActive(true);
    }

    public void OnQuitButton() {
        faderAnimator.SetBool("Fade Out", true);
        Application.Quit();
    }

    public void OnClickStart() {
        faderAnimator.SetBool("Fade Out", true);
        StartCoroutine(LoadLevel(1f, 1));
    }

    public void OnClickShop() {
        shopTweener.open = true;
    }

    IEnumerator LoadLevel(float delay, int level) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(level);
    }
}
