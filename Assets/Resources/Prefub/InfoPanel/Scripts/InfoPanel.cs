using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] public Button BackgroundButton;
    [SerializeField] public Button OkButton;

    public Animator anim;

    void Start()
    {
        BackgroundButton.onClick.AddListener(() =>
        {
            anim.SetBool("IsOpened", false);
        });

        OkButton.onClick.AddListener(() =>
        {
            anim.SetBool("IsOpened", false);
        });
    }
}
