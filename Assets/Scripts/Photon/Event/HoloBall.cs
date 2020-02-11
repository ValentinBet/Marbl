using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloBall : MonoBehaviour
{
    Renderer myRenderer;
    public Animator myAnimator;

    private void Start()
    {
        myRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        myAnimator.SetBool("Blink", true);
        if (other.tag != "Ball")
        {
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        myAnimator.SetBool("Blink", false);
    }

    private void OnTriggerStay(Collider other)
    {
        myAnimator.SetBool("Blink", true);
    }
}
