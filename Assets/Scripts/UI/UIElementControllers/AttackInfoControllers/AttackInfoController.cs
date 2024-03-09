using System;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackInfoController : MonoBehaviour
{
    [SerializeField] private TrainerController trainerController;
    [SerializeField] private MoveSelectionUIElements moveSelectionUIElements;
    [SerializeField] private AttackInfoUIElements attackInfoUIElements;
    [SerializeField] private Attack attack1;
    [SerializeField] private Attack attack2;
    [SerializeField] private Attack attack3;
    [SerializeField] private Attack attack4;
    private Label attackNameLabel;
    private Label ppLabel;
    private Label attackPowerLabel;
    private Label accuracyLabel;
    private Label attackDescritionLabel;
    private Image categoryImage;
    private Image typeImage;
    private bool attack1Clicked = false;
    private bool attack2Clicked = false;
    private bool attack3Clicked = false;
    private bool attack4Clicked = false;
    private UIController uIController;

    protected void OnEnable()
    {
        uIController = GameObject.Find("UI Controller").GetComponent<UIController>();
        uIController.OnMenuChange += HandleMenuChange;
        moveSelectionUIElements = uIController.GetComponent<MoveSelectionUIElements>();
        attackInfoUIElements = uIController.GetComponent <AttackInfoUIElements>();
    }

    protected void OnDisable()
    {
        uIController.OnMenuChange -= HandleMenuChange;
    }

    protected void InitalizeFields()
    {
        VisualElement content = attackInfoUIElements.AttackInfo.Query<VisualElement>("Content");
        VisualElement topElements = content.Query<VisualElement>("TopElements");
        VisualElement nameAndPP = topElements.Query<VisualElement>("NameAndPP");
        VisualElement powerAndAccuracy = topElements.Query<VisualElement>("PowerAndAccuracy");
        VisualElement category = topElements.Query<VisualElement>("Category");
        VisualElement categoryInfo = category.Query<VisualElement>("CategoryInfo");

        attackNameLabel = nameAndPP.Query<Label>("AttackName");
        ppLabel = nameAndPP.Query<Label>("PP");
        attackPowerLabel = powerAndAccuracy.Query<Label>("AttackPower");
        accuracyLabel = powerAndAccuracy.Query<Label>("AttackAccuracy");
        categoryImage = categoryInfo.Query<Image>();
        typeImage = category.Query<Image>("TypeImage");
        attackDescritionLabel = content.Query<Label>("AttackDescription");
    }

    protected void UpdateInfo()
    {
        Attack attack;
        if (attack1Clicked)
        {
            attack = attack1;
        }
        else if (attack2Clicked)
        {
            attack = attack2;
        }
        else if (attack3Clicked)
        {
            attack = attack3;
        }
        else
        {
            attack = attack4;
        }
        attackNameLabel.text = attack.GetAttackName();
        ppLabel.text = $"PP: {attack.GetCurrentPP()}/{attack.GetMaxPP()}";
        attackPowerLabel.text = $"Power: {attack.GetAttackPower()}";
        accuracyLabel.text = $"Accuracy: {attack.GetAttackAccuracy()}";
        //categoryImage.image
        //typeImage.image
        attackDescritionLabel.text = attack.GetAttackDescription();
    }

    protected void InfoButton1Clicked()
    {
        attack1Clicked = true;
        uIController.UpdateMenu(Menus.AttackInfoScreen);
    }

    protected void InfoButton2Clicked()
    {
        attack2Clicked = true;
        uIController.UpdateMenu(Menus.AttackInfoScreen);
    }

    protected void InfoButton3Clicked()
    {
        attack3Clicked = true;
        uIController.UpdateMenu(Menus.AttackInfoScreen);
    }

    protected void InfoButton4Clicked()
    {
        attack4Clicked = true;
        uIController.UpdateMenu(Menus.AttackInfoScreen);
    }

    protected void HandleMenuChange(Menus menu)
    {
        if (menu == Menus.MoveSelectionMenu)
        {
            attack1 = trainerController.GetPlayer().GetActivePokemon().GetMoveset()[0];
            attack2 = trainerController.GetPlayer().GetActivePokemon().GetMoveset()[1];
            attack3 = trainerController.GetPlayer().GetActivePokemon().GetMoveset()[2];
            attack4 = trainerController.GetPlayer().GetActivePokemon().GetMoveset()[3];
            UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack1InfoButton, InfoButton1Clicked);
            UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack2InfoButton, InfoButton2Clicked);
            UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack3InfoButton, InfoButton3Clicked);
            UIEventSubscriptionManager.Subscribe(moveSelectionUIElements.Attack4InfoButton, InfoButton4Clicked);
        }
        else if (menu == Menus.AttackInfoScreen)
        {
            UIEventSubscriptionManager.Subscribe(attackInfoUIElements.BackButton, OnBackButtonClick);
            InitalizeFields();
            UpdateInfo();
        }
    }

    private void OnBackButtonClick()
    {
        attack1Clicked = false;
        attack2Clicked = false;
        attack3Clicked = false;
        attack4Clicked = false;
    }
} 