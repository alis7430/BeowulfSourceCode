using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DeadCurtain : MonoBehaviour
{
    public GameObject curtain;
    public TMP_Text dieText;
    public Image img;
    public GameObject ReturnMainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        curtain = this.gameObject;
        dieText = this.transform.GetChild(0).GetComponent<TMP_Text>();
        img = curtain.GetComponent<Image>();

        curtain.SetActive(false);
    }
    public void ShowDeadCurtain()
    {
        curtain.SetActive(true);

        StartCoroutine(ImageAlphaRender(img, 0f, 0.5f, 3f, 0f));
        StartCoroutine(TmpTextAlphaRender(dieText, 0f, 1f, 1.5f, 3f));

        ReturnMainMenuButton.SetActive(false);

        Invoke("ShowButton", 6.0f);
    }

    public IEnumerator ImageAlphaRender(Image image, float startAlpha, float endAlpha, float time, float duration)
    {

        float t = 0.0f;
        float delayTime = 0.0f;
        Color color = image.color;
        color.a = startAlpha;

        while (color.a < endAlpha)
        {
            delayTime += Time.deltaTime;

            if (delayTime > duration)
            {
                t += Time.deltaTime / time;
                color.a = Mathf.Lerp(startAlpha, endAlpha, t);
                image.color = color;
            }

            yield return null;
        }
        // DestroyImmediate(this.gameObject);
    }

    public IEnumerator TmpTextAlphaRender(TMP_Text text, float startAlpha, float endAlpha, float time, float duration)
    {

        float t = 0.0f;
        float delayTime = 0.0f;
        text.alpha = startAlpha;

        while (text.alpha != endAlpha)
        {
            delayTime += Time.deltaTime;

            if (delayTime > duration)
            {
                t += Time.deltaTime / time;
                text.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            }

            yield return null;

        }
    }
    private void ShowButton()
    {
        ReturnMainMenuButton.SetActive(true);
    }
}
