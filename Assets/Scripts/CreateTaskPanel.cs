using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateTaskPanel : MonoBehaviour
{
    [SerializeField] private Button BackgroundButton;
    [SerializeField] private Button CloseButton;
    [SerializeField] private Button AddButton;
    [SerializeField] private TMP_InputField taskText;
    [SerializeField] private Animator anim;
}
