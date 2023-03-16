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
    [SerializeField] private GameObject AddElementPanel;
    [SerializeField] private GameObject EditElementPanel;
    [SerializeField] private GameObject ConfirmationPanel;
    [SerializeField] private Transform MainPanelTransform;
    [SerializeField] private Transform Content;
    [SerializeField] private VerticalLayoutGroup itemLayout;

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

    // Start is called before the first frame update
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

        switch (sortType)
        {
            case SortType.All:

                foreach (var element in SpawnedItems)
                {
                    element.backPanel.SetActive(true);
                }

                sortTypeText.text = "¬—≈ «¿ƒ¿◊»:";
                break;

            case SortType.Fullfilled:

                foreach (var element in SpawnedItems)
                {
                    if(element.item.Checked == true)
                        element.backPanel.SetActive(true);
                    else
                        element.backPanel.SetActive(false);
                }

                sortTypeText.text = "¬€œŒÀÕ≈ÕÕ€≈:";
                break;

            case SortType.Unfulfilled:

                foreach (var element in SpawnedItems)
                {
                    if (element.item.Checked == false)
                        element.backPanel.SetActive(true);
                    else
                        element.backPanel.SetActive(false);
                }

                sortTypeText.text = "Õ≈ ¬€œŒÀÕ≈ÕÕ€≈:";
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
        var a = Instantiate(AddElementPanel);
        a.transform.SetParent(MainPanelTransform);
        a.transform.localScale = Vector3.one;

        CreateTaskWindow tskWin = a.GetComponent<CreateTaskWindow>();

        tskWin.AddButton.onClick.AddListener(() => 
        {
            string guid = Guid.NewGuid().ToString();
            
            CreateElementInView(new Item(guid, tskWin.taskText.text, false, DateTime.Now));

            SortItems(sortType: currentSortType);

            Destroy(a);
        });

        tskWin.BackgroundButton.onClick.AddListener(() => 
        {
            Destroy(a);
        });

        tskWin.CloseButton.onClick.AddListener(() =>
        {
            Destroy(a);
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
        var a = Instantiate(EditElementPanel);
        a.transform.SetParent(MainPanelTransform);
        a.transform.localScale = Vector3.one;

        EditElementPanel editElementPanel = a.GetComponent<EditElementPanel>();

        editElementPanel.Init(item);

        editElementPanel.CloseButton.onClick.AddListener(() =>
        {
            Destroy(a);
        });

        editElementPanel.BackgroundButton.onClick.AddListener(() =>
        {
            Destroy(a);
        });

        editElementPanel.ChangeButton.onClick.AddListener(() =>
        {
            var conf = Instantiate(ConfirmationPanel);
            conf.transform.SetParent(MainPanelTransform);
            conf.transform.localScale = Vector3.one;

            ConfirmationPanel con = conf.GetComponent<ConfirmationPanel>();

            con.YesButton.onClick.AddListener(() =>
            {
                Destroy(conf);
                Destroy(a);

                var elementUI = (SpawnedItems.Find(i => i.item.guid == item.guid));

                item.itemText = editElementPanel.taskText.text;
                SaveItem();

                elementUI.item.itemText = editElementPanel.taskText.text;
                elementUI.textField.text = item.itemText;
            });
        });

        editElementPanel.DeleteButton.onClick.AddListener(() =>
        {
            var conf = Instantiate(ConfirmationPanel);
            conf.transform.SetParent(MainPanelTransform);
            conf.transform.localScale = Vector3.one;

            ConfirmationPanel con = conf.GetComponent<ConfirmationPanel>();

            con.YesButton.onClick.AddListener(() =>
            {
                Destroy(conf);
                Destroy(a);

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

    //”ƒ¿À»“‹
    public void Secret()
    {
        sortTypeText.text = "»ƒ» Õ¿’”…!!!";
    }
}
