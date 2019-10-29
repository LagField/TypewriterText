using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterText : Text
{
    public enum DisplyaType
    {
        OneByOne,
        SmoothFade,
    }

    public DisplyaType DisplayType
    {
        get => m_DisplayType;
        set
        {
            m_DisplayType = value;
            SetVerticesDirty();
        }
    }

    [SerializeField]
    [HideInInspector]
    private DisplyaType m_DisplayType = DisplyaType.OneByOne;

    public int VisibleCharacterCount
    {
        get => m_VisibleCharacterCount;
        set
        {
            m_VisibleCharacterCount = value;
            SetVerticesDirty();
        }
    }

    [SerializeField]
    [HideInInspector]
    private int m_VisibleCharacterCount;

    public float HeadCharSmoothIndex
    {
        get => m_HeadCharSmoothIndex;
        set
        {
            m_HeadCharSmoothIndex = value;
            SetVerticesDirty();
        }
    }

    [SerializeField]
    [HideInInspector]
    private float m_HeadCharSmoothIndex;

    public float FadeCharLength
    {
        get => m_FadeCharLength;
        set
        {
            m_FadeCharLength = value;
            SetVerticesDirty();
        }
    }
    
    [SerializeField]
    [HideInInspector]
    private float m_FadeCharLength = 2f;

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        base.OnPopulateMesh(toFill);

        if (!IsActive())
        {
            return;
        }

        if (m_DisplayType == DisplyaType.OneByOne)
        {
            PopulateOneByOneMesh(toFill);
        }
        else
        {
            PopulateSmoothFadeMesh(toFill);
        }
    }

    private void PopulateOneByOneMesh(VertexHelper toFill)
    {
        int charCount = toFill.currentVertCount / 4;
        m_VisibleCharacterCount = Mathf.Clamp(m_VisibleCharacterCount, 0, charCount);

        UIVertex uiVertex = new UIVertex();
        for (int i = 0; i < charCount; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int vertIndex = i * 4 + j;
                toFill.PopulateUIVertex(ref uiVertex, vertIndex);
                if (i + 1 <= m_VisibleCharacterCount)
                {
                    Color32 charColor = uiVertex.color;
                    charColor.a = (byte) (color.a * 255);
                    uiVertex.color = charColor;
                }
                else
                {
                    Color32 charColor = uiVertex.color;
                    charColor.a = 0;
                    uiVertex.color = charColor;
                }

                toFill.SetUIVertex(uiVertex, vertIndex);
            }
        }
    }

    private void PopulateSmoothFadeMesh(VertexHelper toFill)
    {
        int charCount = toFill.currentVertCount / 4;
        m_HeadCharSmoothIndex = Mathf.Max(0, m_HeadCharSmoothIndex);
        if (Mathf.Approximately(m_HeadCharSmoothIndex, 0f))
        {
            toFill.Clear();
            return;
        }

        UIVertex uiVertex = new UIVertex();

        int integerPart = Mathf.FloorToInt(m_HeadCharSmoothIndex);
        float fracPart = m_HeadCharSmoothIndex - integerPart;
        float endEdgeIndex = integerPart * 2f + fracPart;
        endEdgeIndex = Mathf.Clamp(endEdgeIndex, 0, m_HeadCharSmoothIndex * 2 + 1);

        m_FadeCharLength = Mathf.Max(0, m_FadeCharLength);
        float startEdgeIndex = endEdgeIndex - m_FadeCharLength * 2;
        startEdgeIndex = Mathf.Clamp(startEdgeIndex, 0, endEdgeIndex);

        if (Mathf.Approximately(startEdgeIndex, endEdgeIndex))
        {
            return;
        }

        for (int i = 0; i < charCount; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int vertIndex = i * 4 + j;
                toFill.PopulateUIVertex(ref uiVertex, vertIndex);

                int vertEdgeIndex = j == 0 || j == 3 ? i * 2 : i * 2 + 1;
                float alpha = 1 - Mathf.InverseLerp(startEdgeIndex, endEdgeIndex, vertEdgeIndex);

                Color32 charColor = uiVertex.color;
                charColor.a = (byte) (alpha * 255);
                uiVertex.color = charColor;

                toFill.SetUIVertex(uiVertex, vertIndex);
            }
        }
    }
}