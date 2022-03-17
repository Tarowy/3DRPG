using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvas : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        DontDestroyOnLoad(this); //防止动画换场景的时候协程未执行完就被销毁
    }

    public IEnumerator StartFade()
    {
        yield return FadeOut(2f);
        yield return FadeIn(2f);
    }

    public IEnumerator FadeIn(float time)
    {
        while (_canvasGroup.alpha != 0)
        {
            _canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }

        Destroy(gameObject);
    }

    public IEnumerator FadeOut(float time)
    {
        while (_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }
    }
}
