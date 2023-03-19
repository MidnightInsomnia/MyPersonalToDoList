using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
    [SerializeField] private GameObject ElementPrefub;
    [SerializeField] private GameObject AddElementPanelPrefub;
    [SerializeField] private GameObject EditElementPanelPrefub;
    [SerializeField] private GameObject ConfirmationPanelPrefub;
    [SerializeField] private GameObject WarningPanelPrefub;
    [SerializeField] private Transform CanvasTransform;
    [SerializeField] private Transform Content;
    [SerializeField] private VerticalLayoutGroup itemLayout;


    [SerializeField] private Animator refreshPanelAnimator;

    [SerializeField] private TMP_Text sortTypeText;

    private List<Item> UserToDoList = new List<Item>();
    private List<ItemUI> SpawnedItems = new List<ItemUI>();

    public enum SortType
    {
        All,
        Fullfilled,
        Unfulfilled
    }

    private SortType currentSortType = SortType.All;

    void Start()
    {
        Debug.Log("PATH " + Application.persistentDataPath);

        SpawnItems(LoadData());

        LoadSortType();

        SortItems(sortType: currentSortType);
    }

    private void LoadSortType()
    {
        if (PlayerPrefs.HasKey("SortType"))
        {
            switch (PlayerPrefs.GetString("SortType"))
            {
                case "All":
                    currentSortType = SortType.All;
                    break;

                case "Fullfilled":
                    currentSortType = SortType.Fullfilled;
                    break;

                case "Unfulfilled":
                    currentSortType = SortType.Unfulfilled;
                    break;
            }
        }
        else
        {
            PlayerPrefs.SetString("SortType", "All");
        }

        PlayerPrefs.Save();
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

    private List<Item> LoadData()
    {
        try
        {
            using (Stream stream = File.Open("data.bin", FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();

                var result = (List<Item>)bin.Deserialize(stream);

                return result;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("EXC 1 " + ex);
        }

        return new List<Item>();
    }

    private void CreateElementInView(Item tempItem)
    {
        Debug.Log("ITEM TEXT " + tempItem.itemText);

        var el = Instantiate(ElementPrefub);
        el.transform.SetParent(Content);
        el.transform.localScale = Vector3.one;

        var itemUI = el.GetComponent<ItemUI>();

        itemUI.item = tempItem;


        itemUI.textField.text = tempItem.itemText;

        Debug.Log("itemEl " + itemUI.textField.text.ToString() + tempItem.timeStamp.ToString());

        UserToDoList.Add(tempItem);

        itemUI.onImgPrs += SwitchItemStatus;
        itemUI.OnPnlPrs += OpenEditElementPanel;

        itemUI.img.sprite = itemUI.item.Checked ? itemUI.ifChecked : itemUI.ifUnchecked;

        SpawnedItems.Add(itemUI);

        SaveItem();

        Debug.Log("ElementAdded");
    }

    public void AddEventButtonPress()
    {
        var a = Instantiate(AddElementPanelPrefub);
        a.transform.SetParent(CanvasTransform);
        a.transform.localScale = Vector3.one;

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

    public void SaveItem()
    {
        if(UserToDoList.Count > 0)
        {
            try
            {
                using (Stream stream = File.Open("data.bin", FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, UserToDoList);
                }
            }
            catch (Exception ex)
            {
                Debug.Log("EXC " + ex);
            }
        }
    }

    public void SwitchItemStatus(Item item)
    {
        var itemToChange = UserToDoList.Find(el => el.guid.Equals(item.guid));
        itemToChange = item;
        SaveItem();
        SortItems(sortType: currentSortType);
    }

    public void OpenEditElementPanel(Item item)
    {
        string oldItemText = item.itemText;

        var a = Instantiate(EditElementPanelPrefub);
        a.transform.SetParent(CanvasTransform);
        a.transform.localScale = Vector3.one;

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

            var conf = Instantiate(ConfirmationPanelPrefub);
            conf.transform.SetParent(CanvasTransform);
            conf.transform.localScale = Vector3.one;


            ConfirmationPanel ÒonfirmationPanel = conf.GetComponent<ConfirmationPanel>();

            ÒonfirmationPanel.YesButton.onClick.AddListener(() =>
            {
                ÒonfirmationPanel.anim.SetBool("IsOpened", false);
                editElementPanel.anim.SetBool("IsOpened", false);

                var elementUI = (SpawnedItems.Find(i => i.item.guid == item.guid));

                item.itemText = editElementPanel.taskText.text;
                SaveItem();

                elementUI.item.itemText = editElementPanel.taskText.text;
                elementUI.textField.text = item.itemText;
            });
        });

        editElementPanel.DeleteButton.onClick.AddListener(() =>
        {
            var conf = Instantiate(ConfirmationPanelPrefub);
            conf.transform.SetParent(CanvasTransform);
            conf.transform.localScale = Vector3.one;

            ConfirmationPanel con = conf.GetComponent<ConfirmationPanel>();

            con.YesButton.onClick.AddListener(() =>
            {
                Destroy(conf);
                editElementPanel.anim.SetBool("IsOpened", false);

                var elementPanel = (SpawnedItems.Find(i => i.item.guid == item.guid));

                Destroy(elementPanel.backPanel);
                UserToDoList.Remove(item);
                SpawnedItems.Remove(elementPanel);
                SaveItem();

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

    public static IEnumerator UpdateLayoutSpacing(VerticalLayoutGroup v, float spacing)
    {
        v.spacing = spacing + 0.00001f;
        yield return new WaitForSeconds(0.00001f);
        v.spacing = spacing + 0.00002f;
        yield return new WaitForSeconds(0.00001f);
        v.spacing = spacing + 0.00003f;
        yield return new WaitForSeconds(0.00001f);
        v.spacing = spacing + 0.00004f;
        v.spacing = spacing;
    }

    public void ShowWarning(string message)
    {
        var warn = Instantiate(WarningPanelPrefub);
        warn.transform.SetParent(CanvasTransform);
        warn.transform.localScale = Vector3.one;

        WarningPanel warning = warn.GetComponent<WarningPanel>();
        warning.SetWarningText(message);
    }

    //”ƒ¿À»“‹
    public void Secret()
    {
        sortTypeText.text = "»ƒ» Õ¿’”…!!!";
    }
}
