using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class MainScript : MonoBehaviour
{
    [SerializeField] private Button AddButton;
    [SerializeField] private GameObject ElementPrefub;
    [SerializeField] private GameObject AddElementWindow;
    [SerializeField] private Transform MainPanelTransform;
    [SerializeField] private Transform Content;

    private List<Item> UserToDoList = new List<Item>();

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

        LoadData();

        SpawnItems();

        SortItems(SortType.All);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnItems()
    {
        foreach (var item in UserToDoList)
        {
            CreateElementInView(item);
        }
    }
    private void SortItems(SortType sortType)
    {
        
    }

    private void LoadData()
    {
        /*if (!PlayerPrefs.HasKey("UserToDoList"))
        {
            PlayerPrefs.SetInt("lastItemID", 0);
        }*/

        /*if (PlayerPrefs.HasKey("UserToDoList"))
        {
            var json = PlayerPrefs.GetString("UserToDoList");

            var jobject = JObject.Parse(json);

            UserToDoList = JsonConvert.DeserializeObject<List<Item>>(jobject.ToString());

            Debug.Log("TEST 1 " + jobject.ToString());
            Debug.Log("TEST 2 " + UserToDoList.ToString());

            //UserToDoList = JObject.Parse(json);

        }*/

        try
        {
            using (Stream stream = File.Open("data.bin", FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();

                UserToDoList = (List<Item>)bin.Deserialize(stream);

                /*var item2 = (List<Item>)bin.Deserialize(stream);
                foreach (Item item in item2)
                {
                    Debug.Log("ITEM DESCRIBE " + item.timeStamp + item.itemText);
                    CreateElementInView(item);
                }*/
            }
        }
        catch (Exception ex)
        {
            Debug.Log("EXC 1 " + ex);
        }
    }

    private void CreateElementInView(Item tempItem)
    {
        Debug.Log("ITEM TEXT " + tempItem.itemText);

        var el = Instantiate(ElementPrefub);
        el.transform.SetParent(Content);
        el.transform.localScale = Vector3.one;

        var itemUI = el.GetComponent<ItemUI>();

        itemUI.item = tempItem;

        /*var itemEl = itemUI.item;

        itemEl.timeStamp = DateTime.Now;
        itemEl.ItemText = tempItem.ItemText;
        itemEl.Checked = false;*/

        itemUI.textField.text = tempItem.itemText;

        Debug.Log("itemEl " + itemUI.textField.text.ToString() + tempItem.timeStamp.ToString());

        //UserToDoList.Add(tempItem);

        itemUI.onImgPrs += SwitchItemStatus;

        itemUI.img.sprite = itemUI.item.Checked ? itemUI.ifChecked : itemUI.ifUnchecked;

        SaveItem();

        Debug.Log("ElementAdded");
    }

    public void OnButtonPress()
    {
       var a = Instantiate(AddElementWindow);
       a.transform.SetParent(MainPanelTransform);
        a.transform.localScale = Vector3.one;

        CreateTaskWindow tskWin = a.GetComponent<CreateTaskWindow>();

        tskWin.AddButton.onClick.AddListener(() => 
        {
            string guid = Guid.NewGuid().ToString();
            //CreateElementInView(tskWin.taskText.text);
            CreateElementInView(new Item(guid, tskWin.taskText.text, false, DateTime.Now));

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
            Debug.Log("LOL 1" + UserToDoList[0].ToString());

            //var str = JsonConvert.SerializeObject(UserToDoList);

            /*var str = JsonConvert.SerializeObject(UserToDoList, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });*/

            /*Debug.Log("LOL 2 " + str);
            PlayerPrefs.SetString("UserToDoList", str);
            Debug.Log("LOL 3");
            Debug.Log("SAVE " + str);
            Debug.Log("LOL 4");
            PlayerPrefs.Save();
            Debug.Log("LOL 5");*/

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

    public void DeleteItem()
    {

    }

    public void SwitchItemStatus(Item item)
    {
        Debug.Log("ÐÀÁÎÒÀÅÒ ÀÕÀÕÀÕÀÕ");
        var itemToChange = UserToDoList.Find(el => el.guid.Equals(item.guid));
        itemToChange = item;
        SaveItem();
    }
}
