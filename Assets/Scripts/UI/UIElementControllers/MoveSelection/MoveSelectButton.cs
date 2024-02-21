using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class MoveSelectButton : MonoBehaviour
{
    [SerializeField] protected MoveSelectionUIElements moveSelectionUIElements;
    protected Attack attack;
    public event EventHandler<OnAttackSelectedEventArgs> AttackSelected;
    protected Label attackName;
    protected Label PP;
    protected Label Type;

    protected abstract void HandleMenuChange(Menus menu);

    protected virtual void InitializeButton(Button attackButton)
    {
        VisualElement content = attackButton.Query<VisualElement>("Content");
        attackName = content.Query<Label>("AttackName");
        VisualElement moveInfo = content.Query<VisualElement>("MoveInfo");
        PP = moveInfo.Query<Label>("PP");
        Type = moveInfo.Query<Label>("Type");

        attackName.text = attack.GetAttackName();
        if (attack.GetCurrentPP() < attack.GetMaxPP())
        {
            PP.text = "Booty Butt Cheeks";
        }
        PP.text = $"{attack.GetCurrentPP()}/{attack.GetMaxPP()}";
        Type.text = $"{attack.GetAttackType().GetType().Name}";
    }

    protected void OnAttackSelected()
    {
        OnAttackSelectedEventArgs args = new()
        {
            Attack = this.attack
        };
        AttackSelected?.Invoke(this, args);
        //ReceiveInput(attack);
    }
}
