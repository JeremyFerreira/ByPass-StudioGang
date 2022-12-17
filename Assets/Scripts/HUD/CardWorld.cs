using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CardWorld : MonoBehaviour
{
    [SerializeField] Color colorImageLock;
    [SerializeField] Sprite imageLock;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] TextMeshProUGUI posPlayer;
    [SerializeField] TextMeshProUGUI starPlayer;

    [SerializeField] EventSO _eventLaunchLevel;
    bool unlock;
    SceneSO objRef;
    int _levelIndex;

    // Start is called before the first frame update

    public void ChangeInformation(SceneSO obj, float timerSave, int star, bool isUnlock, int levelIndex)
    {
        _levelIndex = levelIndex;
        objRef = obj;
        levelName.text = obj.MapName;

        unlock = isUnlock;
        if (!isUnlock)
        {
            image.sprite = obj.SpriteCard;
            image.color = colorImageLock;
            timer.text = "";
            starPlayer.text = "";
        }
        else
        {
            
            image.sprite = obj.SpriteCard;
            timer.text = TimerFormat.FormatTime(timerSave);            
            starPlayer.text = star.ToString() + " / 5";
        }
    }

    public void ClickButton()
    {
        _eventLaunchLevel.OnLauchEventInt?.Invoke(objRef.IndexScene);
    }
}
