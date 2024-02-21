using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TimerBarController
{
    [SerializeField] private GeneralBattleUIElements generalBattleUIElements;
    [SerializeField] private MoveSelectionUIElements moveSelectionUIElements;

    private Label matchTimeGB;
    private Label turnTimeGB;
    private Label matchTimeMS;
    private Label turnTimeMS;

    private void OnEnable()
    {
        
    }
}