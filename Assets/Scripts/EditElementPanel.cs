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
   
    public void Init(Item item)
    {
        if(item!= null)
        {
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
        }
    }
}
