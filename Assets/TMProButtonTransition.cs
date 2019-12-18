using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMProButtonTransition : MonoBehaviour
{
    TextMeshProUGUI text;

    /// <summary>
    /// The selectable Component.
    /// </summary>
    public Selectable Selectable;

    /// <summary>
    /// The color of the normal of the transition state.
    /// </summary>
    public Color NormalColor = Color.white;

    /// <summary>
    /// The color of the hover of the transition state.
    /// </summary>
    public Color HoverColor = Color.black;

    public void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnEnable()
    {
        text.color = NormalColor;
    }

    public void OnDisable()
    {
        text.color = NormalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Selectable == null || Selectable.IsInteractable())
        {
            text.color = HoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Selectable == null || Selectable.IsInteractable())
        {
            text.color = NormalColor;
        }
    }
}
