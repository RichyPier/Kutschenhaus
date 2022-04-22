using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetAdPrivacy : MonoBehaviour
{
    [SerializeField] Toggle adPrivacy;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SaveAdPrivacy);
    }

    void SaveAdPrivacy()
    {
        if (adPrivacy.isOn)
            FindObjectOfType<SaveManager>().SetAdPrivacy(0);
        else
            FindObjectOfType<SaveManager>().SetAdPrivacy(1);

        FindObjectOfType<AdMobRewardedAds>().Init();
        gameObject.GetComponentInParent<GenericWindow>().Close();
    }
}
