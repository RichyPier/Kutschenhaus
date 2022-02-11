using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// To show it in the inspector
[System.Serializable]
public class TransitionSize
{
    public bool use;
    [HideIfFalse("use")] public AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [HideIfFalse("use")] public Vector3 size = new Vector3(1.1f, 1.1f, 1.1f);
    [HideIfFalse("use")] public float duration = 0.3f;
}

[System.Serializable]
public class TransitionPosition
{
    [HideIfFalse("use")] public bool use;
    [HideIfFalse("use")] public AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [HideIfFalse("use")] public Vector3 positionOffset = new Vector3(0 - 3, -3);
    [HideIfFalse("use")] public float duration = 0.3f;
}

[System.Serializable]
public class TransitionRotation
{
    public bool use;
    [HideIfFalse("use")] public AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [HideIfFalse("use")] public Vector3 rotation = new Vector3(0, 0, 3);
    [HideIfFalse("use")] public float duration = 0.3f;
}

[System.Serializable]
public class CanvasFade
{
    public bool use;
    [HideIfFalse("use")] public AnimationCurve animCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [HideIfFalse("use")] public float duration;
    [HideIfFalse("use")] public float startAlpha;
    [HideIfFalse("use")] public float targetAlpha;
}

[System.Serializable]
public class SoundEffect
{
    public bool use;
    [HideIfFalse("use")] public AudioClip startClip;
    [Range(0, 1)]
    [HideIfFalse("use")] public float startClipVolume;
    [Space(10)]
    [HideIfFalse("use")] public AudioClip endClip;
    [Range(0, 1)]
    [HideIfFalse("use")] public float endClipVolume;
}

[System.Serializable]
public class ButtonEventEffect
{
    public TransitionSize sizeTransition;
    public TransitionPosition positionTransition;
    public TransitionRotation rotationTransition;
    public CanvasFade canvasFadeTransition;
    public SoundEffect sound;
}

// The actual class
public class UIEventEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Fields for the button effect
    [SerializeField] ButtonEventEffect onEnableEffect;
    [SerializeField] ButtonEventEffect onHoverEffect;
    [SerializeField] ButtonEventEffect onClickEffect;

    // To save the initial situation so that after the effect you can animate back to the initial situation
    Vector3 startRotationEuler;
    Vector3 startPosition;
    Vector3 startScale;
    // Audiosource is initialized at startup
    AudioSource audioSrc;
    CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = gameObject.AddComponent<AudioSource>();
    }

    private void OnEnable()
    {
        startRotationEuler = transform.rotation.eulerAngles;
        startPosition = transform.position;
        startScale = transform.localScale;

        if ((onEnableEffect.sound.use || onHoverEffect.sound.use || onClickEffect.sound.use) && (audioSrc == null))

            if ((onEnableEffect.canvasFadeTransition.use || onHoverEffect.canvasFadeTransition.use || onClickEffect.canvasFadeTransition.use) && canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (onEnableEffect.rotationTransition.use )
        {
            var tran = onEnableEffect.rotationTransition;
            StartCoroutine(RunRotationTransition(tran.duration, tran.animCurve, tran.rotation));
        }

        if (onEnableEffect.positionTransition.use)
        {
            var tran = onEnableEffect.positionTransition;
            StartCoroutine(RunPositionTransition(tran.duration, tran.animCurve, startPosition + tran.positionOffset));
        }

        if (onEnableEffect.sizeTransition.use)
        {
            var tran = onEnableEffect.sizeTransition;
            StartCoroutine(RunScaleTransition(tran.duration, tran.animCurve, tran.size));
        }

        if (onEnableEffect.canvasFadeTransition.use)
        {
            var tran = onEnableEffect.canvasFadeTransition;
            StartCoroutine(RunCanvasFadeTransition(tran.duration, tran.animCurve, tran.targetAlpha, tran.startAlpha));
        }

        if (onEnableEffect.sound.startClip !=null && onEnableEffect.sound.use)
        {
            audioSrc.clip = onEnableEffect.sound.startClip;
            audioSrc.volume = onEnableEffect.sound.startClipVolume;
            audioSrc.Play();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (onClickEffect.rotationTransition.use)
        {
            var tran = onClickEffect.rotationTransition;
            StartCoroutine(RunRotationTransition(tran.duration, tran.animCurve, tran.rotation));
        }

        if (onClickEffect.positionTransition.use)
        {
            var tran = onClickEffect.positionTransition;
            StartCoroutine(RunPositionTransition(tran.duration, tran.animCurve, startPosition + tran.positionOffset));
        }

        if (onClickEffect.sizeTransition.use)
        {
            var tran = onClickEffect.sizeTransition;
            StartCoroutine(RunScaleTransition(tran.duration, tran.animCurve, tran.size));
        }

        if (onClickEffect.sizeTransition.use)
        {
            var tran = onClickEffect.canvasFadeTransition;
            StartCoroutine(RunCanvasFadeTransition(tran.duration, tran.animCurve, tran.targetAlpha, tran.startAlpha));
        }

        if (onClickEffect.sound.startClip != null && onClickEffect.sound.use)
        {
            audioSrc.clip = onClickEffect.sound.startClip;
            audioSrc.volume = onClickEffect.sound.startClipVolume;
            audioSrc.Play();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onClickEffect.rotationTransition.use)
        {
            var tran = onClickEffect.rotationTransition;
            StartCoroutine(RunRotationTransition(tran.duration, tran.animCurve, startRotationEuler));
        }

        if (onClickEffect.positionTransition.use)
        {
            var tran = onClickEffect.positionTransition;
            StartCoroutine(RunPositionTransition(tran.duration, tran.animCurve, startPosition));
        }

        if (onClickEffect.sizeTransition.use)
        {
            var tran = onClickEffect.sizeTransition;
            StartCoroutine(RunScaleTransition(tran.duration, tran.animCurve, startScale));
        }

        if (onClickEffect.canvasFadeTransition.use)
        {
            var tran = onEnableEffect.canvasFadeTransition;
            StartCoroutine(RunCanvasFadeTransition(tran.duration, tran.animCurve, tran.targetAlpha, tran.startAlpha));
        }

        if (onClickEffect.sound.endClip != null && onClickEffect.sound.use)
        {
            audioSrc.clip = onClickEffect.sound.endClip;
            audioSrc.volume = onClickEffect.sound.endClipVolume;
            audioSrc.Play();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (onHoverEffect.rotationTransition.use)
        {
            var tran = onHoverEffect.rotationTransition;
            StartCoroutine(RunRotationTransition(tran.duration, tran.animCurve, tran.rotation));
        }

        if (onHoverEffect.positionTransition.use)
        {
            var tran = onHoverEffect.positionTransition;
            StartCoroutine(RunPositionTransition(tran.duration, tran.animCurve, startPosition + tran.positionOffset));
        }

        if (onHoverEffect.sizeTransition.use)
        {
            var tran = onHoverEffect.sizeTransition;
            StartCoroutine(RunScaleTransition(tran.duration, tran.animCurve, tran.size));
        }

        if (onHoverEffect.canvasFadeTransition.use)
        {
            var tran = onClickEffect.canvasFadeTransition;
            StartCoroutine(RunCanvasFadeTransition(tran.duration, tran.animCurve, tran.targetAlpha, tran.startAlpha));
        }

        if (onHoverEffect.sound.startClip != null && onHoverEffect.sound.use)
        {
            audioSrc.clip = onHoverEffect.sound.startClip;
            audioSrc.volume = onHoverEffect.sound.startClipVolume;
            audioSrc.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (onHoverEffect.rotationTransition.use)
        {
            var tran = onHoverEffect.rotationTransition;
            StartCoroutine(RunRotationTransition(tran.duration, tran.animCurve, startRotationEuler));
        }

        if (onHoverEffect.positionTransition.use)
        {
            var tran = onHoverEffect.positionTransition;
            StartCoroutine(RunPositionTransition(tran.duration, tran.animCurve, startPosition));
        }

        if (onHoverEffect.sizeTransition.use)
        {
            var tran = onHoverEffect.sizeTransition;
            StartCoroutine(RunScaleTransition(tran.duration, tran.animCurve, startScale));
        }

        if (onHoverEffect.canvasFadeTransition.use)
        {
            var tran = onEnableEffect.canvasFadeTransition;
            StartCoroutine(RunCanvasFadeTransition(tran.duration, tran.animCurve, tran.startAlpha, tran.targetAlpha));
        }

        if (onHoverEffect.sound.endClip != null && onHoverEffect.sound.use)
        {
            audioSrc.clip = onHoverEffect.sound.endClip;
            audioSrc.volume = onHoverEffect.sound.endClipVolume;
            audioSrc.Play();
        }
    }

    IEnumerator RunPositionTransition(float duration, AnimationCurve animationCurve, Vector3 targetTransformation)
    {
        float timeElapsed = 0f;
        Vector3 startVector = new Vector3();
        startVector = transform.position;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float curveT = animationCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startVector, targetTransformation, curveT);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetTransformation;
    }

    IEnumerator RunScaleTransition(float duration, AnimationCurve animationCurve, Vector3 targetTransformation)
    {
        float timeElapsed = 0f;
        Vector3 startVector = new Vector3();
        startVector = transform.localScale;
        
        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float curveT = animationCurve.Evaluate(t);
            transform.localScale = Vector3.Lerp(startVector, targetTransformation, curveT);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetTransformation;
    }

    IEnumerator RunRotationTransition(float duration, AnimationCurve animationCurve, Vector3 targetTransformation)
    {
        float timeElapsed = 0f;
        Quaternion startQuaternion = new Quaternion();
        startQuaternion = transform.rotation;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float curveT = animationCurve.Evaluate(t);
            transform.rotation = Quaternion.Lerp(startQuaternion, Quaternion.Euler(targetTransformation), curveT);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.Euler(targetTransformation);
    }

    IEnumerator RunCanvasFadeTransition(float duration, AnimationCurve animationCurve, float targetAlpha, float startAlpha)
    {
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float curveT = animationCurve.Evaluate(t);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, curveT);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }
    
}
