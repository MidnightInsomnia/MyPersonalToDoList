using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
    public Button NoButton;
    public Button YesButton;
    
    public Button BackgroundButton;

    void Start()
    {
        NoButton.onClick.AddListener(() =>
        {
            Destroy(this.gameObject);
        });

        BackgroundButton.onClick.AddListener(() => 
        {
            Destroy(this.gameObject);
        });
    }
}
