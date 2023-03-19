using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarningPanel : MonoBehaviour
{
    [SerializeField] public Button BackgroundButton;
    [SerializeField] public Button OkButton;
    [SerializeField] public TMP_Text WarningText;

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

    // Update is called once per frame
    public void SetWarningText(string s)
    {
        WarningText.text = s;
    }


}
