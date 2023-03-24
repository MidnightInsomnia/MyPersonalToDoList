using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditElementPanel : MonoBehaviour
{
    [SerializeField] private Button CloseButton;
    [SerializeField] private Button BackgroundButton;
    [SerializeField] private Button ChangeButton;
    [SerializeField] private Button DeleteButton;
    [SerializeField] private TMP_InputField taskText;
    [SerializeField] private TMP_Text timeStamp;
    [SerializeField] private TMP_Text statusText;

    public Animator anim;
    private ItemUI itemUI;

    private void Start()
    {
        enabled = false;
    }

    public void Init(ItemUI itemUI)
    {
        this.itemUI = itemUI;

        CloseButton.onClick.AddListener(() =>
        {
            anim.SetBool("IsOpened", false);
        });

        BackgroundButton.onClick.AddListener(() =>
        {
            anim.SetBool("IsOpened", false);
        });

        if (itemUI!= null)
        {
            var item = itemUI.item;

            taskText.text = item.itemText;
            timeStamp.text = "Дата и время создания: " + item.timeStamp.ToString();

            if (item.Checked)
            {
                statusText.text = "Статус: выполнено";
            }
            else
            {
                statusText.text = "Статус: не выполнено";
            }

            string oldItemText = item.itemText;

            ChangeButton.onClick.AddListener(() =>
            {
                if (taskText.text.Equals(oldItemText))
                {
                    MainController.ShowWarning("Вы не внесли изменений!");
                    return;
                }

                RequestConfirmation();

                ToDoListEvents.OnConfirm += ChangeItem;
            });

            DeleteButton.onClick.AddListener(() =>
            {
                RequestConfirmation();

                ToDoListEvents.OnConfirm += DeleteItem;
            });
        }

        enabled = true;
    }

    public void ChangeItem()
    {
        itemUI.item.itemText = taskText.text;
        itemUI.textField.text = taskText.text;

        ToDoListEvents.OnItemChange?.Invoke();

        ToDoListEvents.OnConfirm -= ChangeItem;

        anim.SetBool("IsOpened", false);
    }

    public void DeleteItem()
    {
        ToDoListEvents.OnItemDelete?.Invoke(itemUI);
        ToDoListEvents.OnConfirm -= DeleteItem;

        anim.SetBool("IsOpened", false);
    }

    public void RequestConfirmation()
    {
        ToDoListEvents.OnConfirmationRequest?.Invoke();
    }
}
