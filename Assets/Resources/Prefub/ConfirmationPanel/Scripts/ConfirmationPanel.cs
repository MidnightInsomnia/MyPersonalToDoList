using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
    [SerializeField] private Button BackgroundButton;
    [SerializeField] private Button NoButton;
    [SerializeField] private Button YesButton;
    [SerializeField] private Animator anim;

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

        YesButton.onClick.AddListener(() =>
        {
            ToDoListEvents.OnConfirm?.Invoke();
            anim.SetBool("IsOpened", false);
        });
    }
}
