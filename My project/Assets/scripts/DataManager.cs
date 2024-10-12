using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private string todoFilePath;
    private string scheduleFilePath;

    public List<TodoItem> todoList = new List<TodoItem>();
    public List<ScheduleItem> scheduleList = new List<ScheduleItem>();

    void Start()
    {
        todoFilePath = Application.persistentDataPath + "/todoData.json";
        scheduleFilePath = Application.persistentDataPath + "/scheduleData.json";
        LoadTodoData();
        LoadScheduleData();
    }

    public void SaveTodoData()
    {
        string json = JsonUtility.ToJson(new TodoDataWrapper { todoList = this.todoList });
        File.WriteAllText(todoFilePath, json);
    }

    public void SaveScheduleData()
    {
        string json = JsonUtility.ToJson(new ScheduleDataWrapper { scheduleList = this.scheduleList });
        File.WriteAllText(scheduleFilePath, json);
    }

    public void LoadTodoData()
    {
        if (File.Exists(todoFilePath))
        {
            string json = File.ReadAllText(todoFilePath);
            TodoDataWrapper wrapper = JsonUtility.FromJson<TodoDataWrapper>(json);
            todoList = wrapper.todoList;
        }
    }

    public void LoadScheduleData()
    {
        if (File.Exists(scheduleFilePath))
        {
            string json = File.ReadAllText(scheduleFilePath);
            ScheduleDataWrapper wrapper = JsonUtility.FromJson<ScheduleDataWrapper>(json);
            scheduleList = wrapper.scheduleList;
        }
    }
}

[Serializable]
public class TodoDataWrapper
{
    public List<TodoItem> todoList;
}

[Serializable]
public class ScheduleDataWrapper
{
    public List<ScheduleItem> scheduleList;
}
