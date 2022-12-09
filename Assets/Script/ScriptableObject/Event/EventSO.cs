using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Events/New Event", order = 1)]
public class EventSO : ScriptableObject
{
    public delegate void LaunchEvent();
    public LaunchEvent OnLaunchEvent;
}