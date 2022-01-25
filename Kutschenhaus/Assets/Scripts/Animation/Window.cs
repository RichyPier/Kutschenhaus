using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Window : MonoBehaviour
{
    [SerializeField] Button close;
    EasyTransition transition;

    private void OnEnable()
    {
        close.onClick.AddListener(Close);
        transition = gameObject.AddComponent<EasyTransition>();
        transition.SetTransition(TransitionType.Scale, AnimationCurve.EaseInOut(0, 0, 1, 1), Vector3.one, 0.6f, null);
        transition.StartTransition();
    }

    void Close()
    {
        transition.SetTransition(TransitionType.Scale, AnimationCurve.EaseInOut(0, 0, 1, 1), Vector3.zero, 0.6f, Disable);
        transition.StartTransition();
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

}
