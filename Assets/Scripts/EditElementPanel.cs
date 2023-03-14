using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EditElementPanel : MonoBehaviour
{
    public Button CloseButton;
    public Button BackgroundButton;
    public Button ChangeButton;
    public Button DeleteButton;
    public TMP_InputField taskText;
    public TMP_Text timeStamp;
    public TMP_Text statusText;
    public Button statusCheckButton;
    public Image statusImage;
    [SerializeField] public Sprite ifChecked;
    [SerializeField] public Sprite ifUnchecked;

    public void Init(Item item)
    {
        if(item!= null)
        {
            taskText.text = item.itemText;
            timeStamp.text = "���� � ����� ��������: " + item.timeStamp.ToString();

            if (item.Checked)
            {
                statusImage.sprite = ifChecked;
                statusText.text = "������: ���������";
            }
            else
            {
                statusImage.sprite = ifUnchecked;
                statusText.text = "������: �� ���������";
            }
        }
    }

    public void OnImagePress(Item item)
    {
        if(item != null)
        {
            item.Checked = (item.Checked == true) ? false : true;
            statusImage.sprite = (item.Checked == true) ? ifChecked : ifUnchecked;
            statusText.text = (item.Checked == true) ? "������: ���������" : "������: �� ���������";
        }
    }
}
