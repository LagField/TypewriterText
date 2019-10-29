using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private TypewriterText text;

    private void Start()
    {
        text = GetComponent<TypewriterText>();
        if (text == null)
        {
            enabled = false;
            return;
        }

        if (text.DisplayType == TypewriterText.DisplyaType.OneByOne)
        {
            StartCoroutine(OneByOneTestCoroutine());
        }
        else
        {
            StartCoroutine(SmoothFadeTestCoroutine());
        }
    }

    IEnumerator OneByOneTestCoroutine()
    {
        string s = text.text;
        int charCount = s.Length;

        for (int i = 0; i < charCount; i++)
        {
            text.VisibleCharacterCount = i + 1;
            yield return null;
        }
    }

    IEnumerator SmoothFadeTestCoroutine()
    {
        string s = text.text;
        int charCount = s.Length;

        text.FadeCharLength = 10;
        text.HeadCharSmoothIndex = 0f;

        //HeadCharSmoothIndex表示开始fade out的字符index,可以超出字符长度,建议直接加上fadeCharLength,这样结束后就可以显示完整字符,
        //不用再去设置fadeCharLength了
        while (text.HeadCharSmoothIndex < charCount + text.FadeCharLength)
        {
            {
                text.HeadCharSmoothIndex += Time.deltaTime * 40;
                yield return null;
            }
        }
    }
}