using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Toolkit;

public class MainView : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup itemLayout;
    [SerializeField] private GameObject ConfirmationPanelPrefub;
    [SerializeField] private GameObject EditElementPanelPrefub;
    [SerializeField] private GameObject AddElementPanelPrefub;
    [SerializeField] private GameObject WarningPanelPrefub;
    [SerializeField] private GameObject InfoPanelPrefub;
    [SerializeField] private GameObject ElementPrefub;
    [SerializeField] private Transform CanvasTransform;
    [SerializeField] private Transform Content;
    [SerializeField] private TMP_Text StatusText;

    [SerializeField] private MainController mainController;

    public List<ItemUI> SpawnedItems = new List<ItemUI>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        ToDoListEvents.OnItemDelete += RemoveDeletedItem;
    }

    private void OnDisable()
    {
        ToDoListEvents.OnItemDelete -= RemoveDeletedItem;
    }

    private void RemoveDeletedItem(ItemUI itemUI)
    {
        SpawnedItems.Remove(itemUI);
    }

    public void SortAllButtonPress()
    {
        mainController.SortItems(SortType.All);
    }

    public void SortFullfilledButtonPress()
    {
        mainController.SortItems(SortType.Fullfilled);
    }

    public void SortUnfulfilledButtonPress()
    {
        mainController.SortItems(SortType.Unfulfilled);
    }

    public void OnInfoButtonPress()
    {
        Instantiate(InfoPanelPrefub, CanvasTransform);
    }

    public void CreateElementInView(Item tempItem)
    {
        Debug.Log("ITEM TEXT " + tempItem.itemText);

        var el = Instantiate(ElementPrefub, Content);

        var itemUI = el.GetComponent<ItemUI>();

        itemUI.item = tempItem;

        itemUI.textField.text = tempItem.itemText;

        Debug.Log("itemEl " + itemUI.textField.text.ToString() + tempItem.timeStamp.ToString());

        itemUI.img.sprite = itemUI.item.Checked ? itemUI.ifChecked : itemUI.ifUnchecked;

        SpawnedItems.Add(itemUI);

        UpdateLayoutSpacing(itemLayout, itemLayout.spacing);
    }

    public void AddEventButtonPress()
    {
        Instantiate(AddElementPanelPrefub, CanvasTransform);
    }

    public void SpawnItems(List<Item> list)
    {
        foreach (var item in list)
        {
            CreateElementInView(item);
        }

        UpdateLayoutSpacing(itemLayout, itemLayout.spacing);
    }

    public void ChangeItemsDisplay(SortType sortType)
    {
        int itemCount = 0;

        switch (sortType)
        {
            case SortType.All:

                foreach (var element in SpawnedItems)
                {
                    element.backPanel.SetActive(true);
                    itemCount++;
                }
                break;

            case SortType.Fullfilled:

                foreach (var element in SpawnedItems)
                {
                    if (element.item.Checked == true)
                    {
                        element.backPanel.SetActive(true);
                        itemCount++;
                    }
                    else
                        element.backPanel.SetActive(false);
                }
                break;

            case SortType.Unfulfilled:

                foreach (var element in SpawnedItems)
                {
                    if (element.item.Checked == false)
                    {
                        element.backPanel.SetActive(true);
                        itemCount++;
                    }
                    else
                        element.backPanel.SetActive(false);
                }
                break;
        }

        UpdateStatusText(sortType, itemCount);
    }

    public void UpdateStatusText(SortType type, int itemCount)
    {
        switch (type)
        {
            case SortType.All:
                StatusText.text = string.Format("¬—≈√Œ «¿ƒ¿◊ ({0}):", itemCount);
                break;
            case SortType.Fullfilled:
                StatusText.text = string.Format("¬€œŒÀÕ≈ÕÕ€≈ ({0}):", itemCount);
                break;
            case SortType.Unfulfilled:
                StatusText.text = string.Format("Õ≈ ¬€œŒÀÕ≈ÕÕ€≈ ({0}):", itemCount);
                break;
        }
    }

    public void SpawnEditPanel(ItemUI itemUI)
    {
        var editElementPrefub = Instantiate(EditElementPanelPrefub, CanvasTransform);

        EditElementPanel editElementPanel = editElementPrefub.GetComponent<EditElementPanel>();

        editElementPanel.Init(itemUI);
    }

    public void SpawnConfirmationPanel()
    {
        Instantiate(ConfirmationPanelPrefub, CanvasTransform);
    }

    public void SpawnWarningPanel(string message)
    {
        var warn = Instantiate(WarningPanelPrefub, CanvasTransform);

        WarningPanel warning = warn.GetComponent<WarningPanel>();
        warning.SetWarningText(message);
    }
}
