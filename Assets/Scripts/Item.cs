using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Item
{
    public string guid = "";
    public string itemText = "";
    public bool Checked = false;
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
