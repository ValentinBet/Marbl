using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BallControl : MonoBehaviour
{
    [SerializeField]
    private bool isSelected = false;

    private void Update()
    {
        /*if (this.transform.position.y < -20)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }*/
    }


    public void SetOutline(bool value, Color? c = null)
    {
        Outline _outline = this.GetComponent<Outline>();

        // _outline.enabled = value ?? !_outline.enabled; || Inverseur avec Booléan optionnel

        _outline.enabled = value;
        _outline.OutlineColor = c ?? Color.white;

        isSelected = _outline.enabled;
    }

}
