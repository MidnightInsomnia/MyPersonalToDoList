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

    private void Start()
    {
        BackgroundButton.onClick.AddListener(() =>
        {
            anim.SetBool("IsOpened", false);
        });

        CloseButton.onClick.AddListener(() =>
        {
            anim.SetBool("IsOpened", false);
        });

        AddButton.onClick.AddListener(() =>
        {
            if (taskText.text.Equals(string.Empty))
            {
                RequestWarning("Текст заметки не может быть пустым!");
                return;
            }

            string guid = Guid.NewGuid().ToString();
            
            ToDoListEvents.OnItemCreate(new Item(guid, taskText.text, false, DateTime.Now));
            anim.SetBool("IsOpened", false);
        });
    }

    public void RequestWarning(string message)
    {
        ToDoListEvents.OnWarningRequest?.Invoke(message);
    }
}
