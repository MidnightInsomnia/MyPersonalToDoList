using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Toolkit;
using static DataService;

public class MainScript : MonoBehaviour
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
    [SerializeField] private TMP_Text sortTypeText;

    private List<ItemUI> SpawnedItems = new List<ItemUI>();
    private List<Item> UserToDoList = new List<Item>();

    private SortType currentSortType = SortType.All;

    void Start()
    {
        Debug.Log("PATH " + Application.persistentDataPath);

        SpawnItems(LoadData());

        currentSortType = LoadSortType();

        SortItems(sortType: currentSortType);
    }

    private void SpawnItems(List<Item>list)
    {
        foreach (var item in list)
        {
            CreateElementInView(item);
        }

        UpdateLayoutSpacing(itemLayout, itemLayout.spacing);
    }
    private void SortItems(SortType sortType)
    {
        currentSortType = sortType;
        PlayerPrefs.SetString("SortType", Enum.GetName(typeof(SortType), sortType));
        PlayerPrefs.Save();

        int itemCount = 0;

        switch (sortType)
        {
            case SortType.All:

                foreach (var element in SpawnedItems)
                {
                    element.backPanel.SetActive(true);
                    itemCount++;
                }

                sortTypeText.text = string.Format("¬—≈√Œ «¿ƒ¿◊ ({0}):", SpawnedItems.Count);
                break;

            case SortType.Fullfilled:

                foreach (var element in SpawnedItems)
                {
                    if(element.item.Checked == true)
                    {
                        element.backPanel.SetActive(true);
                        itemCount++;
                    }
                    else
                        element.backPanel.SetActive(false);
                }

                sortTypeText.text = string.Format("¬€œŒÀÕ≈ÕÕ€≈ ({0}):", itemCount);
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

                sortTypeText.text = string.Format("Õ≈ ¬€œŒÀÕ≈ÕÕ€≈ ({0}):", itemCount);
                break;
        }
    }

    private void CreateElementInView(Item tempItem)
    {
        Debug.Log("ITEM TEXT " + tempItem.itemText);

        var el = Instantiate(ElementPrefub, Content);

        var itemUI = el.GetComponent<ItemUI>();

        itemUI.item = tempItem;


        itemUI.textField.text = tempItem.itemText;

        Debug.Log("itemEl " + itemUI.textField.text.ToString() + tempItem.timeStamp.ToString());

        UserToDoList.Add(tempItem);

        itemUI.onImgPrs += SwitchItemStatus;
        itemUI.OnPnlPrs += OpenEditElementPanel;

        itemUI.img.sprite = itemUI.item.Checked ? itemUI.ifChecked : itemUI.ifUnchecked;

        SpawnedItems.Add(itemUI);

        SaveData(UserToDoList);

        UpdateLayoutSpacing(itemLayout, itemLayout.spacing);

        Debug.Log("ElementAdded");
    }

    public void AddEventButtonPress()
    {
        var a = Instantiate(AddElementPanelPrefub, CanvasTransform);

        CreateTaskPanel tskWin = a.GetComponent<CreateTaskPanel>();

        tskWin.AddButton.onClick.AddListener(() => 
        {
            if (tskWin.taskText.text.Equals(string.Empty))
            {
                ShowWarning("“ÂÍÒÚ Á‡ÏÂÚÍË ÌÂ ÏÓÊÂÚ ·˚Ú¸ ÔÛÒÚ˚Ï!");

                return;
            }

            string guid = Guid.NewGuid().ToString();
            
            CreateElementInView(new Item(guid, tskWin.taskText.text, false, DateTime.Now));

            SortItems(sortType: currentSortType);

            tskWin.anim.SetBool("IsOpened", false);
        });

        tskWin.BackgroundButton.onClick.AddListener(() => 
        {
            tskWin.anim.SetBool("IsOpened", false);
        });

        tskWin.CloseButton.onClick.AddListener(() =>
        {
            tskWin.anim.SetBool("IsOpened", false);
        });
    }

    public void SwitchItemStatus(Item item)
    {
        var itemToChange = UserToDoList.Find(el => el.guid.Equals(item.guid));
        itemToChange = item;
        SaveData(UserToDoList);
        SortItems(sortType: currentSortType);
    }

    public void OpenEditElementPanel(Item item)
    {
        string oldItemText = item.itemText;

        var a = Instantiate(EditElementPanelPrefub, CanvasTransform);

        EditElementPanel editElementPanel = a.GetComponent<EditElementPanel>();

        editElementPanel.Init(item);

        editElementPanel.CloseButton.onClick.AddListener(() =>
        {
            editElementPanel.anim.SetBool("IsOpened", false);
        });

        editElementPanel.BackgroundButton.onClick.AddListener(() =>
        {
            editElementPanel.anim.SetBool("IsOpened", false);
        });

        editElementPanel.ChangeButton.onClick.AddListener(() =>
        {
            if (editElementPanel.taskText.text.Equals(oldItemText))
            {
                ShowWarning("¬˚ ÌÂ ‚ÌÂÒÎË ËÁÏÂÌÂÌËÈ!");
                return;
            }

            var conf = Instantiate(ConfirmationPanelPrefub, CanvasTransform);

            ConfirmationPanel ÒonfirmationPanel = conf.GetComponent<ConfirmationPanel>();

            ÒonfirmationPanel.YesButton.onClick.AddListener(() =>
            {
                ÒonfirmationPanel.anim.SetBool("IsOpened", false);
                editElementPanel.anim.SetBool("IsOpened", false);

                var elementUI = (SpawnedItems.Find(i => i.item.guid == item.guid));

                item.itemText = editElementPanel.taskText.text;
                SaveData(UserToDoList);

                elementUI.item.itemText = editElementPanel.taskText.text;
                elementUI.textField.text = item.itemText;
            });
        });

        editElementPanel.DeleteButton.onClick.AddListener(() =>
        {
            var conf = Instantiate(ConfirmationPanelPrefub, CanvasTransform);

            ConfirmationPanel con = conf.GetComponent<ConfirmationPanel>();

            con.YesButton.onClick.AddListener(() =>
            {
                Destroy(conf);
                editElementPanel.anim.SetBool("IsOpened", false);

                var elementPanel = (SpawnedItems.Find(i => i.item.guid == item.guid));

                Destroy(elementPanel.backPanel);
                UserToDoList.Remove(item);
                SpawnedItems.Remove(elementPanel);
                SaveData(UserToDoList);

                SortItems(sortType: currentSortType);
            });
        });
    }

    public void SortAllButtonPress()
    {
        SortItems(SortType.All);
    }

    public void SortFullfilledButtonPress()
    {
        SortItems(SortType.Fullfilled);
    }

    public void SortUnfulfilledButtonPress()
    {
        SortItems(SortType.Unfulfilled);
    }

    public void ShowWarning(string message)
    {
        var warn = Instantiate(WarningPanelPrefub, CanvasTransform);

        WarningPanel warning = warn.GetComponent<WarningPanel>();
        warning.SetWarningText(message);
    }

    public void OnInfoButtonPress()
    {
        Instantiate(InfoPanelPrefub, CanvasTransform);
    }
}
