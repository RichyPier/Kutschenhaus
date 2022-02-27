using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Winch : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] Slider slider;
    [SerializeField] Transform roller;
    [SerializeField] GameObject anchorPrefab;
    [SerializeField] GameObject anchor;
    [SerializeField] float maxDistance;
    [SerializeField] Vector3[] linePointsOffset;
    [SerializeField] float winchForce;
    LineRenderer line;
    Rigidbody2D rB;

    private void Start()
    {
        rB = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        if (anchor != null)
        {
            var direction = ((anchor.transform.position + linePointsOffset[1]) - transform.position).normalized;
            rB.AddForce(direction * slider.value * winchForce);
        }

    }

    // Update is called once per frame
    void Update()
    {
        roller.Rotate(Vector3.back * Time.deltaTime * rotationSpeed * slider.value);

        if (anchor != null)
        {
            var distance = Vector2.Distance(transform.position, anchor.transform.position);
            anchor.GetComponent<WinchAnchor>().Outside = distance > maxDistance;

            if (!line.enabled)
                line.enabled = true;

            line.SetPosition(0, transform.position + linePointsOffset[0]);
            line.SetPosition(1, anchor.transform.position + linePointsOffset[1]);
        }
        else
        {
            if (line.enabled)
                line.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        if (anchor == null)
        {
            slider.value = 0;
            anchor = Instantiate(anchorPrefab);
            anchor.transform.position = transform.position;
        }
    }
}
