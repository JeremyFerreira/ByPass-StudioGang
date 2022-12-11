using Doozy.Runtime.Nody;
using UnityEngine;
[CreateAssetMenu(fileName = "New TimerData", menuName = "Data/New Timer Data", order = 1)]
public class TimeSO : ScriptableObject
{
    [field: SerializeField] public float TotalTime { get; private set; }

    public delegate void OnValueChangeEvent(float value);
    public OnValueChangeEvent OnValueChange;

    public void ChangeTimer(float time)
    {
        TotalTime=time;
        OnValueChange?.Invoke(time);
    }
}