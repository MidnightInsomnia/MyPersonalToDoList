using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class ToDoListEvents
{
    public static Action<Item> OnItemCheckBoxPress;
    public static Action<ItemUI> OnItemPanelPress;
    public static Action<ItemUI> OnItemDelete;
    public static Action OnItemChange;
    public static Action OnConfirmationRequest;
    public static Action OnConfirm;
}
