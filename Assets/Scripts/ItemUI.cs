using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] public GameObject backPanel;
    [SerializeField] public TMP_Text textField;
    [SerializeField] public Button checkBtn;
    [SerializeField] public Image img;
    [SerializeField] public Sprite ifChecked;
    [SerializeField] public Sprite ifUnchecked;

    public delegate void MethodContainer(Item item);

    public static Action<Item> onImagePress;
    public static Action<Item> onPanelPress;

    public Item item;

    //����� ������� �������
    public void OnImagePress()
    {
        if (item != null)
        {
            item.Checked = (item.Checked == true) ? false : true;
            img.sprite = (item.Checked == true) ? ifChecked : ifUnchecked;
            ToDoListEvents.OnItemCheckBoxPress?.Invoke(item);
        }
    }

    //�������� EditPanel
    public void OnPanelPress()
    {
        if (item != null)
        {
            ToDoListEvents.OnItemPanelPress?.Invoke(this);
        }
    }
}
