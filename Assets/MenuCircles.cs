using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCircles : MonoBehaviour
{
    public float lifetime = 5.0f;
    Vector2 direction;
    Vector2 firstPos;
    float timer;
    RectTransform myTr;
    void Start()
    {
        myTr = GetComponent<RectTransform>();
        firstPos = myTr.anchoredPosition;
        direction = -myTr.anchoredPosition;
    }

    void Update()
    {
        timer += Time.deltaTime;
        myTr.anchoredPosition = Vector3.Lerp(firstPos, direction, timer / lifetime);
    }
}
