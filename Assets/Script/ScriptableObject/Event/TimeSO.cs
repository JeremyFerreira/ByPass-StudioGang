using UnityEngine;
[CreateAssetMenu(fileName = "New TimerData", menuName = "Data/New Timer Data", order = 1)]
public class TimeSO : ScriptableObject
{
    public float TotalTime { get; private set; }
    private void OnEnable()
    {
        TotalTime = 0f;
    }

    public delegate void OnValueChangeEvent();
    public OnValueChangeEvent OnValueChange;

    public void ChangeTimer(float time)
    {
        TotalTime=time;
        OnValueChange?.Invoke();
    }
}