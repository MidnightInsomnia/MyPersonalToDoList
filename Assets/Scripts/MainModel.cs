using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class MainModel : MonoBehaviour
{
    //Загрузка списка задач из файла
    public List<Item> LoadData()
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

    //Сохранение списка задач в файл
    public void SaveData(List<Item> UserToDoList)
    {
        if (UserToDoList.Count > 0)
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

    //Загрузка сохранённого типа сортировки или его инициализация в случае отсутствия
    public SortType LoadSortType()
    {
        SortType result = SortType.All;

        if (PlayerPrefs.HasKey("SortType"))
        {
            switch (PlayerPrefs.GetString("SortType"))
            {
                case "All":
                    result = SortType.All;
                    break;

                case "Fullfilled":
                    result = SortType.Fullfilled;
                    break;

                case "Unfulfilled":
                    result = SortType.Unfulfilled;
                    break;
            }
        }
        else
        {
            PlayerPrefs.SetString("SortType", "All");
            PlayerPrefs.Save();
        }

        return result;
    }
}
