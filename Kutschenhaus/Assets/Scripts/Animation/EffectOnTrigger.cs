using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectOnTrigger : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] EasyTransition moveTransition;
    [SerializeField] EasyTransition fadeOutTransition;
    [SerializeField] AudioSource collectSound;
    [SerializeField] string triggerTagName;
    [SerializeField] string targetTagName;

    [SerializeField] RotateGameObject rotateGameObjectCoin;

    GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            var worldPoint = Camera.main.ScreenToWorldPoint(target.transform.position);
            moveTransition.SetTargetTransformation(worldPoint);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(triggerTagName))
        {
            if (moveTransition != null)
            {
            target = GameObject.FindGameObjectWithTag(targetTagName);
            moveTransition.StartTransition();
            }

            if (fadeOutTransition != null)
            {
                fadeOutTransition.StartTransition();
            }

            if (particle != null)
            {
                particle.Play();
            }

            if (collectSound != null)
            {
                collectSound.Play();
            }

        }
    }
}
