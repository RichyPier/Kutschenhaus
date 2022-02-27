using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinchAnchor : MonoBehaviour
{
    bool grounded;
    bool outside;

    // Anchor color - if the anchor is outside the allowed range, it will turn red and not settable
    public bool Outside
    {
        private get { return outside; }

        set
        {
            outside = value;

            if (outside)
                GetComponent<SpriteRenderer>().color = Color.red;
            else
                GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void Start()
    {
        GetComponentInChildren<Button>().onClick.AddListener(() => { Destroy(gameObject); });
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void OnMouseUp()
    {
        if (!grounded || outside)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("Grounded");
            grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("Grounded");
            grounded = false;
        }
    }



}
