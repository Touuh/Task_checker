//タスク管理と通知機能の追加予定地
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{
    public Text taskListText;
    private List<string> tasks = new List<string>();

    void Start()
    {
        AddTask("Finish project report");
        AddTask("Meeting at 9:30 AM");
    }

    void Update()
    {
        taskListText.text = string.Join("\n", tasks.ToArray());
    }

    public void AddTask(string newTask)
    {
        tasks.Add(newTask);
    }
}