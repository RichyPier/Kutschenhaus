using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaiseEventOnClick : MonoBehaviour
{
    [SerializeField] GameEvent gameEvent;
    [SerializeField] GenericWindow windowToClose;

    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<Button>(out Button button))
        {
            button.onClick.AddListener(Raise);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Raise()
    {
        gameEvent.Raise();
        if (windowToClose)
        {
            windowToClose.Close();
        }
    }
}
