using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField] private MainView mainView;
    [SerializeField] private MainModel mainModel;

    private List<Item> UserToDoList = new List<Item>();

    public SortType currentSortType = SortType.All;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PATH " + Application.persistentDataPath);

        currentSortType = mainModel.LoadSortType();
        UserToDoList = mainModel.LoadData();

        mainView.SpawnItems(UserToDoList);
        mainView.ChangeItemsDisplay(sortType: currentSortType);
    }

    private void OnEnable()
    {
        ToDoListEvents.OnItemCreate += AddItem;
        ToDoListEvents.OnWarningRequest += ShowWarning;
        ToDoListEvents.OnItemCheckBoxPress += SwitchItemStatus;
        ToDoListEvents.OnItemPanelPress += EditItemPressed;
        ToDoListEvents.OnConfirmationRequest += RequestConfirmation;
        ToDoListEvents.OnItemChange += SaveChanges;
        ToDoListEvents.OnItemDelete += DeleteItem;
    }

    private void OnDisable()
    {
        ToDoListEvents.OnItemCreate += AddItem;
        ToDoListEvents.OnWarningRequest -= ShowWarning;
        ToDoListEvents.OnItemCheckBoxPress -= SwitchItemStatus;
        ToDoListEvents.OnItemPanelPress -= EditItemPressed;
        ToDoListEvents.OnConfirmationRequest -= RequestConfirmation;
        ToDoListEvents.OnItemChange -= SaveChanges;
        ToDoListEvents.OnItemDelete -= DeleteItem;
    }

    public void ShowWarning(string message)
    {
        mainView.SpawnWarningPanel(message);
    }

    public void SortItems(SortType sortType)
    {
        currentSortType = sortType;

        PlayerPrefs.SetString("SortType", Enum.GetName(typeof(SortType), sortType));
        PlayerPrefs.Save();

        mainView.ChangeItemsDisplay(sortType);
    }

    public void EditItemPressed(ItemUI itemUI)
    {
        mainView.SpawnEditPanel(itemUI);
    }

    public void RequestConfirmation()
    {
        mainView.SpawnConfirmationPanel();
    }

    public void AddItem(Item item)
    {
        UserToDoList.Add(item);
        mainView.CreateElementInView(item);
        SortItems(sortType: currentSortType);
    }

    public void DeleteItem(ItemUI itemUI)
    {
        Destroy(itemUI.backPanel);
        UserToDoList.Remove(itemUI.item);
        //SpawnedItems.Remove(itemUI);
        SaveChanges();

        SortItems(sortType: currentSortType);
    }

    public void SaveChanges()
    {
        mainModel.SaveData(UserToDoList);
    }

    public void SwitchItemStatus(Item item)
    {
        var itemToChange = UserToDoList.Find(el => el.guid.Equals(item.guid));
        itemToChange = item;
        SaveChanges();
        SortItems(sortType: currentSortType);
    }
}
