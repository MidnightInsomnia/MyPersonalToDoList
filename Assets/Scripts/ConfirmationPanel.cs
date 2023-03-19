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

    public Animator anim;

    void Start()
    {
        NoButton.onClick.AddListener(() =>
        {
            anim.SetBool("IsOpened", false);
        });

        BackgroundButton.onClick.AddListener(() => 
        {
            anim.SetBool("IsOpened", false);
        });
    }
}
