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

    [SerializeField] public MainScript main;

    public delegate void MethodContainer(Item item);

    public event MethodContainer onImgPrs;
    public event MethodContainer OnPnlPrs;

    public Item item;

    public void OnImagePress()
    {
        if (item != null)
        {
            item.Checked = (item.Checked == true) ? false : true;
            img.sprite = (item.Checked == true) ? ifChecked : ifUnchecked;
            //main.SwitchItemStatus(item);
            onImgPrs(item);
        }
    }

    //Открытие редактирования элемента будет тут
    public void OnPanelPress()
    {
        if (item != null)
        {
            OnPnlPrs(item);
        }
    }
}
