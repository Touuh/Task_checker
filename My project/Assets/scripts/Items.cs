using System;

[Serializable] //JSON形式に変換するためのアトリビュート
public class TodoItem
{
    public string taskName;
    public string descriptopn;
    public DateTime Deadline;
    public DateTime Alarm;
    public bool isCompleted;
}

[Serializable]
public class ScheduleItem
{
    public string title;
    public DateTime dueDate;
    public DateTime alarm;
}