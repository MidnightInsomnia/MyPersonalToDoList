using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
//Шаблон задачи
public class Item
{
    //Уникальный идентификатор
    public string guid = "";
    //Текст задачи
    public string itemText = "";
    //Статус выполнения
    public bool Checked = false;
    //Дата создания
    public DateTime timeStamp;

    public Item() { }

    public Item(string guid, string itemText, bool Checked, DateTime timeStamp)
    {
        this.guid = guid;
        this.itemText = itemText;
        this.Checked = Checked;
        this.timeStamp = timeStamp;
    }
}
