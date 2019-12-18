using UnityEngine;
using System.Collections;

public class bl_ChatLine : MonoBehaviour
{
    private CanvasGroup m_canvas;

    public void FadeInTime(float t,float speed)
    {
        m_canvas = GetComponent<CanvasGroup>();
        StopAllCoroutines();
        StartCoroutine(Fade(t,speed));
    }

    IEnumerator Fade(float t,float speed)
    {
        yield return new WaitForSeconds(t);
        while(m_canvas.alpha > 0)
        {
            m_canvas.alpha -= Time.deltaTime * speed;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void ShowObj(float speed)
    {
        StopAllCoroutines();
        StartCoroutine(Show(speed));
    }

    IEnumerator Show(float speed)
    {
        while (m_canvas.alpha < 1)
        {
            m_canvas.alpha += Time.deltaTime * speed;
            yield return null;
        }
    }
}