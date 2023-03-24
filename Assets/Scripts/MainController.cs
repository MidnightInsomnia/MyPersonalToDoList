using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField] private MainView mainView;
    [SerializeField] private MainModel mainModel;


    private List<ItemUI> SpawnedItems = new List<ItemUI>();
    private List<Item> UserToDoList = new List<Item>();

    public SortType currentSortType = SortType.All;
    //public static Action<Item> OnItemChange;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PATH " + Application.persistentDataPath);

        /*currentSortType = mainModel.LoadSortType();
        UserToDoList = mainModel.LoadData();

        mainView.SpawnItems(UserToDoList);
        mainView.ChangeItemsDisplay(sortType: currentSortType, SpawnedItems);*/
    }

    private void OnEnable()
    {
        ToDoListEvents.OnItemPanelPress += EditItemPressed;
        ToDoListEvents.OnConfirmationRequest += RequestConfirmation;
        ToDoListEvents.OnItemChange += SaveChanges;
        ToDoListEvents.OnItemDelete += DeleteItem;
    }

    private void OnDisable()
    {
        ToDoListEvents.OnItemPanelPress -= EditItemPressed;
        ToDoListEvents.OnConfirmationRequest -= RequestConfirmation;
        ToDoListEvents.OnItemChange -= SaveChanges;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ShowWarning(string message)
    {
        //Отдать в MainView

        /*var warn = Instantiate(WarningPanelPrefub, CanvasTransform);

        WarningPanel warning = warn.GetComponent<WarningPanel>();
        warning.SetWarningText(message);*/
    }

    public void SortItems(SortType sortType)
    {
        //currentSortType = sortType;

        PlayerPrefs.SetString("SortType", Enum.GetName(typeof(SortType), sortType));
        PlayerPrefs.Save();

        //mainView.ChangeItemsDisplay(sortType, SpawnedItems);
    }

    public void EditItemPressed(ItemUI itemUI)
    {
        mainView.SpawnEditPanel(itemUI);
    }

    public void RequestConfirmation()
    {
        mainView.SpawnConfirmationPanel();
    }

    public void ChangeItem()
    {

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
        /* var itemToChange = UserToDoList.Find(el => el.guid.Equals(item.guid));
        itemToChange = item;
        SaveData(UserToDoList);
        SortItems(sortType: currentSortType);*/
    }
}
