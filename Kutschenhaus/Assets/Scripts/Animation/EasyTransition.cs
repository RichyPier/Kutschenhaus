using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum TransitionType { Scale, Position, Rotation, ImageColor, SpriteColor, MeshRendererColor }

public class EasyTransition : MonoBehaviour
{
    [SerializeField] TransitionType type;
    [SerializeField] AnimationCurve animationCurve;

    [Header("If type: Scale, Position, Rotation")]
    [SerializeField] Vector3 targetTransformation;

    [Header("If type: ImageColor, SpriteColor, MRColor")]
    [SerializeField] Color targetColor;

    [Space(20)]
    [SerializeField] float duration;
    [SerializeField] bool startOnAwake;

    [Header("OnEnd Event")]
    [SerializeField] UnityEvent onEndEvent;
    [SerializeField] float onEndEventDelay;

    System.Action onEndAction;
    Color lastStartColor;
    Vector3 lastStartVector;

    private void OnEnable()
    {
        if (startOnAwake) StartTransition();
    }

    public void StartTransition()
    {
        switch (type)
        {
            case TransitionType.Scale: StartCoroutine(RunScaleTransition());
                break;
            case TransitionType.Position:
                StartCoroutine(RunPositionTransition());
                break;
            case TransitionType.Rotation:
                StartCoroutine(RunRotationTransition());
                break;
            case TransitionType.ImageColor:
                StartCoroutine(RunImageColorTransition());
                break;
            case TransitionType.SpriteColor:
                StartCoroutine(RunSpriteColorTransition());
                break;
            case TransitionType.MeshRendererColor:
                StartCoroutine(RunMeshRendererColorTransition());
                break;
        }
    }

    public void SetTransition(TransitionType type, AnimationCurve animationCurve, Vector3 targetTransformation, float duration, System.Action onEndAction)
    {
        this.type = type;
        this.animationCurve = animationCurve;
        this.targetTransformation = targetTransformation;
        this.duration = duration;
        this.onEndAction = onEndAction;
    }

    public void SetTransition(TransitionType type, AnimationCurve animationCurve, Color targetColor, float duration, System.Action onEndAction)
    {
        this.type = type;
        this.animationCurve = animationCurve;
        this.targetColor = targetColor;
        this.duration = duration;
        this.onEndAction = onEndAction;
    }

    public void RestartBackwards()
    {
        targetColor = lastStartColor;
        targetTransformation = lastStartVector;
        StartTransition();
    }

    IEnumerator RunScaleTransition()
    {
        float timeElapsed = 0f;
        Vector3 startVector = new Vector3();
        startVector = transform.localScale;
        lastStartVector = startVector;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float curveT = animationCurve.Evaluate(t);
            transform.localScale = Vector3.Lerp(startVector, targetTransformation, curveT);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetTransformation;
        StartCoroutine(OnEndTransition());
    }

    IEnumerator RunPositionTransition()
    {
        float timeElapsed = 0f;
        Vector3 startVector = new Vector3();
        startVector = transform.position;
        lastStartVector = startVector;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float curveT = animationCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startVector, targetTransformation, curveT);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetTransformation;
        StartCoroutine(OnEndTransition());
    }

    IEnumerator RunRotationTransition()
    {
        float timeElapsed = 0f;
        Quaternion startQuaternion = new Quaternion();
        startQuaternion = transform.rotation;
        lastStartVector = startQuaternion.eulerAngles;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float curveT = animationCurve.Evaluate(t);
            transform.rotation = Quaternion.Lerp(startQuaternion, Quaternion.Euler(targetTransformation), curveT);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(targetTransformation);
        StartCoroutine(OnEndTransition());
    }

    IEnumerator RunImageColorTransition()
    {
        if (TryGetComponent(out Image image))
        {
            float timeElapsed = 0f;
            Color startColor = new Color();
            startColor = image.color;
            lastStartColor = startColor;

            while (timeElapsed < duration)
            {
                float t = timeElapsed / duration;
                float curveT = animationCurve.Evaluate(t);
                image.color = Color.Lerp(startColor, targetColor, curveT);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            image.color = targetColor;
            StartCoroutine(OnEndTransition());
        }
    }

    IEnumerator RunSpriteColorTransition()
    {
        if (TryGetComponent(out SpriteRenderer sprite))
        {
            float timeElapsed = 0f;
            Color startColor = new Color();
            startColor = sprite.color;
            lastStartColor = startColor;

            while (timeElapsed < duration)
            {
                float t = timeElapsed / duration;
                float curveT = animationCurve.Evaluate(t);
                sprite.color = Color.Lerp(startColor, targetColor, curveT);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            StartCoroutine(OnEndTransition());
        }
    }

    IEnumerator RunMeshRendererColorTransition()
    {
        if (TryGetComponent(out MeshRenderer renderer))
        {
            float timeElapsed = 0f;
            Color startColor = new Color();
            startColor = renderer.material.color;
            lastStartColor = startColor;

            while (timeElapsed < duration)
            {
                float t = timeElapsed / duration;
                float curveT = animationCurve.Evaluate(t);
                renderer.material.color = Color.Lerp(startColor, targetColor, curveT);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            renderer.material.color = targetColor;
            StartCoroutine(OnEndTransition());
        }
    }

    IEnumerator OnEndTransition()
    {
        yield return new WaitForSeconds(onEndEventDelay);
        onEndEvent.Invoke();
        if (onEndAction != null) onEndAction.Invoke();
    }
}
