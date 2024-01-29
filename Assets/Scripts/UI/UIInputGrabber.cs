using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UIInputGrabber
{
    private IPlayerAction selectedAction;
    Attack1Controller attack1Controller;
    Attack2Controller attack2Controller;
    Attack3Controller attack3Controller;
    Attack4Controller attack4Controller;
    public UIInputGrabber()
    {
        Attack1Controller attack1Controller = GameObject.Find("AttackSelectionControllers").GetComponent<Attack1Controller>();
        Attack2Controller attack2Controller = GameObject.Find("AttackSelectionControllers").GetComponent<Attack2Controller>();
        Attack3Controller attack3Controller = GameObject.Find("AttackSelectionControllers").GetComponent<Attack3Controller>();
        Attack4Controller attack4Controller = GameObject.Find("AttackSelectionControllers").GetComponent<Attack4Controller>();
        attack1Controller.AttackSelected += Attack1Selected;
        attack2Controller.AttackSelected += Attack2Selected;
        attack3Controller.AttackSelected += Attack3Selected;
        attack4Controller.AttackSelected += Attack4Selected;
    }

    ~UIInputGrabber()
    {
        attack1Controller.AttackSelected -= Attack1Selected;
        attack2Controller.AttackSelected -= Attack2Selected;
        attack3Controller.AttackSelected -= Attack3Selected;
        attack4Controller.AttackSelected -= Attack4Selected;
    }

    private void Attack1Selected(object sender, OnAttackSelectedEventArgs args)
    {
        Debug.Log($"{args.Attack.GetAttackName()}");
        selectedAction = args.Attack;
    }
    private void Attack2Selected(object sender, OnAttackSelectedEventArgs args)
    {
        Debug.Log($"{args.Attack.GetAttackName()}");
        selectedAction = args.Attack;
    }
    private void Attack3Selected(object sender, OnAttackSelectedEventArgs args)
    {
        Debug.Log($"{args.Attack.GetAttackName()}");
        selectedAction = args.Attack;
    }
    private void Attack4Selected(object sender, OnAttackSelectedEventArgs args)
    {
        Debug.Log($"{args.Attack.GetAttackName()}");
        selectedAction = args.Attack;
    }

    public IPlayerAction GetSelectedAttack()
    {
        return selectedAction;
    }
    public IPlayerAction HandleInput(IPlayerAction input)
    {
        // Handle the input and return a string
        return input;
    }
}
