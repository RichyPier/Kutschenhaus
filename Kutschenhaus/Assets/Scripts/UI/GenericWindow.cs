using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GenericWindow : MonoBehaviour
{
    [SerializeField] Button closeButton;
    const float animationDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<GenericWindow>().Length > 1)
            Destroy(gameObject);

        closeButton.onClick.AddListener(Close);
        StartCoroutine(OpenAnimation());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Close()
    {
        Time.timeScale = 1;
        StartCoroutine(CloseAnimation());
    }

    IEnumerator OpenAnimation()
    {
        float time = 0;
        transform.localScale = Vector3.zero;

        while (time < animationDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, time / animationDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.one;
        Time.timeScale = 0;
    }

    IEnumerator CloseAnimation()
    {
        float time = 0;
        transform.localScale = Vector3.one;

        while (time < animationDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, time / animationDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }
}
