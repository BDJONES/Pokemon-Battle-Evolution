using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
    protected UIController uIController;

    protected abstract void HandleMenuChange(Menus menu);

    protected virtual void InitializeButton(Button attackButton)
    {
        //if (IsOwner)
        //{
            VisualElement content = attackButton.Query<VisualElement>("Content");
            attackName = content.Query<Label>("AttackName");
            VisualElement moveInfo = content.Query<VisualElement>("MoveInfo");
            PP = moveInfo.Query<Label>("PP");
            Type = moveInfo.Query<Label>("Type");

            attackName.text = attack.GetAttackName();
            PP.text = $"{attack.GetCurrentPP()}/{attack.GetMaxPP()}";
            Type.text = $"{attack.GetAttackType().GetType().Name}";
        //}
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
