using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class UIInputGrabber //: IDisposable
{
    private IPlayerAction selectedAction;
    public static event Action MoveSelected;
    Attack1Controller attack1Controller;
    Attack2Controller attack2Controller;
    Attack3Controller attack3Controller;
    Attack4Controller attack4Controller;
    Pokemon1Controller pokemon1Controller;
    Pokemon2Controller pokemon2Controller;
    Pokemon3Controller pokemon3Controller;
    Pokemon4Controller pokemon4Controller;
    Pokemon5Controller pokemon5Controller;
    Pokemon6Controller pokemon6Controller;
    private Menus previousMenu;

    public UIInputGrabber()
    {
        UIController.OnMenuChange += HandleMenuChange;
    }



    ~UIInputGrabber()
    {
        attack1Controller.AttackSelected -= Attack1Selected;
        attack2Controller.AttackSelected -= Attack2Selected;
        attack3Controller.AttackSelected -= Attack3Selected;
        attack4Controller.AttackSelected -= Attack4Selected;
    }

    private async void Attack1Selected(object sender, OnAttackSelectedEventArgs args)
    {
        Debug.Log($"{args.Attack.GetAttackName()}");
        selectedAction = args.Attack;
        UIController.Instance.UpdateMenu(Menus.DialogueScreen);
        await DialogueBoxController.RequestForTextChange("Awaiting Player Input");
        UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
        HandleInput();
    }

    private async void Attack2Selected(object sender, OnAttackSelectedEventArgs args)
    {
        Debug.Log($"{args.Attack.GetAttackName()}");
        selectedAction = args.Attack;
        UIController.Instance.UpdateMenu(Menus.DialogueScreen);
        await DialogueBoxController.RequestForTextChange("Awaiting Player Input");
        UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
        HandleInput();
    }

    private async void Attack3Selected(object sender, OnAttackSelectedEventArgs args)
    {
        Debug.Log($"{args.Attack.GetAttackName()}");
        selectedAction = args.Attack;
        UIController.Instance.UpdateMenu(Menus.DialogueScreen);
        await DialogueBoxController.RequestForTextChange("Awaiting Player Input");
        UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
        HandleInput();
    }

    private async void Attack4Selected(object sender, OnAttackSelectedEventArgs args)
    {
        Debug.Log($"{args.Attack.GetAttackName()}");
        selectedAction = args.Attack;
        UIController.Instance.UpdateMenu(Menus.DialogueScreen);
        await DialogueBoxController.RequestForTextChange("Awaiting Player Input");
        UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
        HandleInput();
    }

    private async void Switch1Selected(object sender, OnSwitchEventArgs e)
    {
        var prevMenu = UIController.Instance.GetCurrentMenu();
        selectedAction = e.Switch;
        if (prevMenu != Menus.PokemonFaintedScreen)
        {
            UIController.Instance.UpdateMenu(Menus.DialogueScreen);
            await DialogueBoxController.RequestForTextChange("Awaiting Player Input");
            UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
        }
        HandleInput();
    }

    private async void Switch2Selected(object sender, OnSwitchEventArgs e)
    {
        var prevMenu = UIController.Instance.GetCurrentMenu();
        selectedAction = e.Switch;
        if (prevMenu != Menus.PokemonFaintedScreen)
        {
            UIController.Instance.UpdateMenu(Menus.DialogueScreen);
            await DialogueBoxController.RequestForTextChange("Awaiting Player Input");
            UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
        }
        HandleInput();
    }

    private async void Switch3Selected(object sender, OnSwitchEventArgs e)
    {
        var prevMenu = UIController.Instance.GetCurrentMenu();
        selectedAction = e.Switch;
        if (prevMenu != Menus.PokemonFaintedScreen)
        {
            UIController.Instance.UpdateMenu(Menus.DialogueScreen);
            await DialogueBoxController.RequestForTextChange("Awaiting Player Input");
            UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
        }
        HandleInput();
    }

    private async void Switch4Selected(object sender, OnSwitchEventArgs e)
    {
        var prevMenu = UIController.Instance.GetCurrentMenu();
        selectedAction = e.Switch;
        if (prevMenu != Menus.PokemonFaintedScreen)
        {
            UIController.Instance.UpdateMenu(Menus.DialogueScreen);
            await DialogueBoxController.RequestForTextChange("Awaiting Player Input");
            UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
        }
        HandleInput();
    }

    private async void Switch5Selected(object sender, OnSwitchEventArgs e)
    {
        var prevMenu = UIController.Instance.GetCurrentMenu();
        selectedAction = e.Switch;
        if (prevMenu != Menus.PokemonFaintedScreen)
        {
            UIController.Instance.UpdateMenu(Menus.DialogueScreen);
            await DialogueBoxController.RequestForTextChange("Awaiting Player Input");
            UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
        }
        HandleInput();
    }

    private async void Switch6Selected(object sender, OnSwitchEventArgs e)
    {
        var prevMenu = UIController.Instance.GetCurrentMenu();
        selectedAction = e.Switch;
        if (prevMenu != Menus.PokemonFaintedScreen)
        {
            UIController.Instance.UpdateMenu(Menus.DialogueScreen);
            await DialogueBoxController.RequestForTextChange("Awaiting Player Input");
            UIController.Instance.UpdateMenu(Menus.GeneralBattleMenu);
        }
        HandleInput();
    }

    public IPlayerAction GetSelectedAction()
    {
        
        return selectedAction;
    }

    private void HandleInput()
    {
        // Handle the input and return a string
        MoveSelected.Invoke();
    }

    private void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.MoveSelectionMenu)
        {
            attack1Controller = GameObject.Find("AttackSelectionControllers").GetComponent<Attack1Controller>();
            attack2Controller = GameObject.Find("AttackSelectionControllers").GetComponent<Attack2Controller>();
            attack3Controller = GameObject.Find("AttackSelectionControllers").GetComponent<Attack3Controller>();
            attack4Controller = GameObject.Find("AttackSelectionControllers").GetComponent<Attack4Controller>();
            if (attack1Controller != null)
            {
                attack1Controller.AttackSelected += Attack1Selected;
            }
            if (attack2Controller != null)
            {
                attack2Controller.AttackSelected += Attack2Selected;
            }
            if (attack3Controller != null)
            {
                attack3Controller.AttackSelected += Attack3Selected;
            }
            if (attack4Controller != null)
            {
                attack4Controller.AttackSelected += Attack4Selected;
            }
        }
        else if (menu == Menus.InBattlePartyMenu || menu == Menus.PokemonFaintedScreen)
        {
            pokemon1Controller = GameObject.Find("WidgetHolder").GetComponent<Pokemon1Controller>();
            pokemon2Controller = GameObject.Find("WidgetHolder").GetComponent<Pokemon2Controller>();
            pokemon3Controller = GameObject.Find("WidgetHolder").GetComponent<Pokemon3Controller>();
            pokemon4Controller = GameObject.Find("WidgetHolder").GetComponent<Pokemon4Controller>();
            pokemon5Controller = GameObject.Find("WidgetHolder").GetComponent<Pokemon5Controller>();
            pokemon6Controller = GameObject.Find("WidgetHolder").GetComponent<Pokemon6Controller>();

            if (pokemon1Controller != null)
            {
                //Debug.Log("Subscribing to Controller1");
                pokemon1Controller.SwitchSelected += Switch1Selected;
            }
            if (pokemon2Controller != null)
            {
                //Debug.Log("Subscribing to Controller2");
                pokemon2Controller.SwitchSelected += Switch2Selected;
            }
            if (pokemon3Controller != null)
            {
                //Debug.Log("Subscribing to Controller3");
                pokemon3Controller.SwitchSelected += Switch3Selected;
            }
            if (pokemon4Controller != null)
            {
                //Debug.Log("Subscribing to Controller4");
                pokemon4Controller.SwitchSelected += Switch4Selected;
            }
            if (pokemon5Controller != null)
            {
                //Debug.Log("Subscribing to Controller5");
                pokemon5Controller.SwitchSelected += Switch5Selected;
            }
            if (pokemon6Controller != null)
            {
                //Debug.Log("Subscribing to Controller6");
                pokemon6Controller.SwitchSelected += Switch6Selected;
            }
        }
        else
        {
            if (previousMenu == Menus.MoveSelectionMenu)
            {
                if (attack1Controller != null)
                {
                    attack1Controller.AttackSelected -= Attack1Selected;
                }
                if (attack2Controller != null)
                {
                    attack2Controller.AttackSelected -= Attack2Selected;
                }
                if (attack3Controller != null)
                {
                    attack3Controller.AttackSelected -= Attack3Selected;
                }
                if (attack4Controller != null)
                {
                    attack4Controller.AttackSelected -= Attack4Selected;
                }
            }
            else if (previousMenu == Menus.InBattlePartyMenu || previousMenu == Menus.PokemonFaintedScreen)
            {
                if (pokemon1Controller != null)
                {
                    //Debug.Log("Unsubscribing from Controller 1");
                    pokemon1Controller.SwitchSelected -= Switch1Selected;
                }
                if (pokemon2Controller != null)
                {
                    //Debug.Log("Unsubscribing from Controller 2");
                    pokemon2Controller.SwitchSelected -= Switch2Selected;
                }
                if (pokemon3Controller != null)
                {
                    pokemon3Controller.SwitchSelected -= Switch3Selected;
                }
                if (pokemon4Controller != null)
                {
                    pokemon4Controller.SwitchSelected -= Switch4Selected;
                }
                if (pokemon5Controller != null)
                {
                    pokemon5Controller.SwitchSelected -= Switch5Selected;
                }
                if (pokemon6Controller != null)
                {
                    pokemon6Controller.SwitchSelected -= Switch6Selected;
                }
            }
        }
        previousMenu = menu;
    }

    //public void Dispose()
    //{
    //    Dispose(true);
    //    // any other unmanaged resource cleanups you can do here
    //    GC.SuppressFinalize(this);
    //}

    //protected virtual void Dispose(bool disposing)
    //{
    //    if (!_disposed)
    //    {
    //        if (disposing)
    //        {
    //            if (_stream != null)
    //            {
    //                _stream.Dispose(); // say you have to dispose a stream
    //            }
    //        }

    //        _stream = null;
    //        _disposed = true;
    //    }
    //}
}
