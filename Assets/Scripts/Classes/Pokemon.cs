#nullable enable

using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public abstract class Pokemon: NetworkBehaviour
{
    protected string speciesName = null!;
    protected string nickname = null!;
    protected int level;
    protected List<Ability> abilityList= null!;
    [SerializeField] protected Ability ability = null!;
    [SerializeField] protected Gender gender;
    [SerializeField] protected Type Type1 = null!;
    protected Type? Type2;
    [SerializeField] private NetworkVariable<StatusConditions> status = new NetworkVariable<StatusConditions>(StatusConditions.Healthy);
    public StatusConditions Status { 
        get
        {
            return status.Value;
        }
        set
        {
            //bool canBeStatused = CanBeStatused(value);
            //if (canBeStatused)
            //{
            status.Value = value;
            //this.StatusEffect();
            StatusChanged?.Invoke(value);
            AlertNewStatusOnClientRpc(status.Value);
            //}
        }
    }
    public event Action<StatusConditions>? StatusChanged;
    [SerializeField] protected List<Attack> moveSet = null!;
    [SerializeField] protected List<Attack> learnSet = null!;
    public bool isActive = false; // Is the pokemon currently on the field
    public bool ActiveState
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
        }
    }
    [SerializeField] protected Item? heldItem;
    protected int baseHP;
    [SerializeField] protected NetworkVariable<int> hpStat = new NetworkVariable<int>();

    protected int baseAttack;
    [SerializeField] protected int attackStat;
    [SerializeField] private NetworkVariable<int> attackStage = new NetworkVariable<int>(0);
    public int AttackStage
    {
        get
        {
            return attackStage.Value;
        }
        set
        {
            
            if (value < -6)
            {
                GameManager.Instance.SendDialogueToClientRpc($"This stat can't be lowered any further");
                GameManager.Instance.SendDialogueToHostRpc($"This stat can't be lowered any further");
                return;
            }
            if (value > 6)
            {
                GameManager.Instance.SendDialogueToClientRpc($"This stat can't be increased any further");
                GameManager.Instance.SendDialogueToHostRpc($"This stat can't be increased any further");
                return;
            }
            attackStage.Value = value;
            //Debug.Log($"Attack Stage = {attackStage}");
            //Debug.Log($"Base Attack = {baseAttack}");
            //Debug.Log($"Attack IVs = {ivs.attack}");
            //Debug.Log($"Attack Evs = {evs.attack}");
            //Debug.Log($"Level = {level}");
            attackStat = Mathf.FloorToInt(0.01f * (2 * baseAttack + ivs.attack + Mathf.FloorToInt(0.25f * evs.attack)) * level) + 5;
            attackStat = Mathf.FloorToInt(attackStat * stageConversionDictionary[attackStage.Value]);
        }
    }
    protected int baseDefense;
    [SerializeField] protected int defenseStat;
    [SerializeField] private NetworkVariable<int> defenseStage = new NetworkVariable<int>(0);
    public int DefenseStage
    {
        get
        {
            return defenseStage.Value;
        }
        set
        {

            if (value < -6)
            {
                GameManager.Instance.SendDialogueToClientRpc($"This stat can't be lowered any further");
                GameManager.Instance.SendDialogueToHostRpc($"This stat can't be lowered any further");
                return;
            }
            if (value > 6)
            {
                GameManager.Instance.SendDialogueToClientRpc($"This stat can't be increased any further");
                GameManager.Instance.SendDialogueToHostRpc($"This stat can't be increased any further");
                return;
            }
            defenseStage.Value = value;
            defenseStat = Mathf.FloorToInt(0.01f * (2 * baseDefense + ivs.defense + Mathf.FloorToInt(0.25f * evs.defense)) * level) + 5;
            defenseStat = Mathf.FloorToInt(defenseStat * stageConversionDictionary[defenseStage.Value]);
        }
    }
    protected int baseSpecialAttack;
    [SerializeField] protected int specialAttackStat;
    [SerializeField] private NetworkVariable<int> specialAttackStage = new NetworkVariable<int>(0);
    public int SpecialAttackStage
    {
        get
        {
            return specialAttackStage.Value;
        }
        set
        {

            if (value < -6) {
                GameManager.Instance.SendDialogueToClientRpc($"This stat can't be lowered any further");
                GameManager.Instance.SendDialogueToHostRpc($"This stat can't be lowered any further");
                return;
            }
            if (value > 6)
            {
                GameManager.Instance.SendDialogueToClientRpc($"This stat can't be increased any further");
                GameManager.Instance.SendDialogueToHostRpc($"This stat can't be increased any further");
                return;
            }
            specialAttackStage.Value = value;
            specialAttackStat = Mathf.FloorToInt(0.01f * (2 * baseSpecialAttack + ivs.specialAttack + Mathf.FloorToInt(0.25f * evs.specialAttack)) * level) + 5;
            specialAttackStat = Mathf.FloorToInt(specialAttackStat * stageConversionDictionary[specialAttackStage.Value]);
        }
    }
    protected int baseSpecialDefense;
    [SerializeField] protected int specialDefenseStat;
    [SerializeField] private NetworkVariable<int> specialDefenseStage = new NetworkVariable<int>(0);
    public int SpecialDefenseStage
    {
        get
        {
            return specialDefenseStage.Value;
        }
        set
        {

            if (value < -6)
            {
                Debug.Log("This stat can't be lowered any further");
                return;
            }
            if (value > 6)
            {
                GameManager.Instance.SendDialogueToClientRpc($"This stat can't be increased any further");
                GameManager.Instance.SendDialogueToHostRpc($"This stat can't be increased any further");
                return;
            }
            specialDefenseStage.Value = value;
            specialDefenseStat = Mathf.FloorToInt(0.01f * (2 * baseSpecialDefense + ivs.specialDefense + Mathf.FloorToInt(0.25f * evs.specialDefense)) * level) + 5;
            specialDefenseStat = Mathf.FloorToInt(specialDefenseStat * stageConversionDictionary[specialDefenseStage.Value]);
        }
    }
    protected int baseSpeed;
    [SerializeField] protected int speedStat;
    [SerializeField] private NetworkVariable<int> speedStage = new NetworkVariable<int>(0);
    public int SpeedStage
    {
        get
        {
            return speedStage.Value;
        }
        set
        {
            if (value < -6)
            {
                Debug.Log("This stat can't be lowered any further");
                return;
            }
            if (value > 6)
            {
                GameManager.Instance.SendDialogueToClientRpc($"This stat can't be increased any further");
                GameManager.Instance.SendDialogueToHostRpc($"This stat can't be increased any further");
                return;
            }
            speedStage.Value = value;
            speedStat = Mathf.FloorToInt(0.01f * (2 * baseSpeed + ivs.speed + Mathf.FloorToInt(0.25f * evs.speed)) * level) + 5;
            speedStat = Mathf.FloorToInt(speedStat * stageConversionDictionary[speedStage.Value]);
        }
    }
    [SerializeField] protected Ivs ivs = null!;
    [SerializeField] protected Evs evs = null!;
    
    // Stage Conversions resource: https://bulbapedia.bulbagarden.net/wiki/Stat_modifier
    private readonly Dictionary<int, float> stageConversionDictionary = new()
    {
        { 6, 4f },
        { 5, 3.5f },
        { 4, 3f },
        { 3, 2.5f },
        { 2, 2f },
        { 1, 1.5f },
        { 0, 1f },
        { -1, 0.67f },
        { -2, 0.5f },
        { -3, 0.4f },
        { -4, 0.33f },
        { -5, 0.29f },
        { -6, 0.25f }
    };
    private NetworkVariable<bool> isPressured = new NetworkVariable<bool>(false);
    public bool IsPressured
    {
        get 
        { 
            return isPressured.Value; 
        }
        set
        {
            isPressured.Value = value;
        }
    }
    public NetworkVariable<bool> isLevitating = new NetworkVariable<bool>(false);
    public bool IsLevitating
    {
        get
        {
            return isLevitating.Value;
        }
        set
        {
            isLevitating.Value = value;
        }
    }
    protected Attack? lastAttack = null;
    protected EventsToTriggerManager eventsToTriggerManager;

    protected virtual void OnEnable()
    {
        NetworkCommands.UIControllerCreated += () =>
        {
            eventsToTriggerManager = GameObject.Find("EventsToTriggerManager").GetComponent<EventsToTriggerManager>();
            eventsToTriggerManager.OnTriggerEvent += HandleTriggeredEvent;
            this.StatusChanged += HandleStatusChange;
        };            
        GameManager.OnStateChange += UpdatePokemonInfo;
    }

    private void OnDisable()
    {
        eventsToTriggerManager.OnTriggerEvent -= HandleTriggeredEvent;
        this.StatusChanged -= HandleStatusChange;
        GameManager.OnStateChange -= UpdatePokemonInfo;
    }

    protected virtual void UpdatePokemonInfo(GameState state)
    {
        return;
    }

    public string GetSpeciesName()
    {
        return speciesName;
    }
    public string GetNickname()
    {
        return nickname;
    }
    public int GetLevel()
    {
        return level;
    }
    public Ability GetAbility()
    {
        return ability;
    }
    public List<Ability> GetAbilityList()
    {
        return abilityList;
    }
    public Gender GetGender()
    {
        return gender;
    }
    public Type GetType1()
    {
        return Type1;
    }
    public Type? GetType2()
    {
        return Type2;
    }
    public bool IsHealthy()
    {
        return this.status.Value == StatusConditions.Healthy;
    }
    public bool IsBurned()
    {
        return this.status.Value == StatusConditions.Burn;
    }
    public bool IsFrozen()
    {
        return this.status.Value == StatusConditions.Freeze;
    }
    public bool IsParalyzed()
    {
        return this.status.Value == StatusConditions.Paralysis;
    }
    public bool IsPoisoned()
    {
        return this.status.Value == StatusConditions.Poison;
    }
    public bool IsBadlyPoisoned()
    {
        return this.status.Value == StatusConditions.BadPoison;
    }
    public bool IsAsleep()
    {
        return this.status.Value == StatusConditions.Asleep;
    }
    public bool IsDead()
    {
        return this.hpStat.Value == 0;
    }
    public List<Attack> GetMoveset()
    {
        return moveSet;
    }
    public List<Attack> GetLearnset()
    {
        return learnSet;
    }
    public bool GetActiveStatus()
    {
        return ActiveState;
    }
    public Item? GetItem()
    {
        return heldItem;
    }
    public int GetBaseHP()
    {
        return baseHP;
    }
    public int GetBaseAttack()
    {
        return baseAttack;
    }
    public int GetBaseDefense()
    {
        return baseDefense;
    }
    public int GetBaseSpecialAttack()
    {
        return baseSpecialAttack;
    }
    public int GetBaseSpecialDefense()
    {
        return baseSpecialDefense;
    }
    public int GetBaseSpeed()
    {
        return baseSpeed;
    }
    public int GetMaxHPStat()
    {
        int maxHP = Mathf.FloorToInt(0.01f * (2 * this.baseHP + this.ivs.hp + Mathf.FloorToInt(0.25f * this.evs.hp)) * this.level) + this.level + 10;
        return maxHP;
    }
    public int GetHPStat()
    {
        return hpStat.Value;
    }
    public int GetAttackStat()
    {
        return attackStat;
    }
    public int GetDefenseStat()
    {
        return defenseStat;
    }
    public int GetSpecialAttackStat()
    {
        return specialAttackStat;
    }
    public int GetSpecialDefenseStat()
    {
        return specialDefenseStat;
    }
    public int GetSpeedStat()
    {
        return speedStat;
    }
    public Ivs GetIvs()
    {
        return ivs;
    }
    public Evs GetEvs()
    {
        return evs;
    }
    public Attack? GetLastAttack()
    {
        return lastAttack;
    }
    public void RemoveItem()
    {
        heldItem = null;
    }
    public void SetNickname(string newNickname)
    {
        nickname = newNickname;
    }
    public NetworkVariable<int> GetHPNetworkVariable()
    {
        return hpStat;
    }
    public void SetHPStat(int newHP)
    {
        if (newHP > GetMaxHPStat())
        {
            hpStat.Value = GetMaxHPStat();
        }
        else if (newHP < 0)
        {
            hpStat.Value = 0;
        }
        else
        {
            hpStat.Value = newHP;
        }
    }
    public void SetAttackStat(int newAttack)
    {
        attackStat = newAttack;
    }
    public void SetDefenseStat(int newDefense)
    {
        defenseStat = newDefense;
    }
    public void SetSpecialAttackStat(int newSpecialAttack)
    {
        specialAttackStat = newSpecialAttack;
    }
    public void SetSpecialDefenseStat(int newSpecialDefense)
    {
        specialDefenseStat = newSpecialDefense;
    }
    public void SetSpeedStat(int newSpeed)
    {
        speedStat = newSpeed;
    }
    public void SetLastAttack(Attack attack)
    {
        this.lastAttack = attack;
        Debug.Log($"The last attack used by {this.nickname} was {lastAttack.GetAttackName()}");
        Debug.Log($"The last attack is a contact move {lastAttack.IsContact()}");
        GameManager.Instance.FinishRPCTaskRpc();
    }
    private bool CanBeStatused(StatusConditions statusCondition)
    {
        if (statusCondition == StatusConditions.Paralysis)
        {
            if (Type1 == StaticTypeObjects.Electric || (Type2 != null && Type2 == StaticTypeObjects.Electric))
            {
                return false;
            }
        }
        return true;
    }
    public void StatusEffect()
    {
        //if (IsHost)
        //{
        Debug.Log("Causing Status Effects");
        if (this.status.Value == StatusConditions.Burn)
        {
            // Fix Burn effect based on info here: https://the-episodes-and-movie-yveltal-and-more.fandom.com/wiki/Burn_(Pok%C3%A9mon_Status_Condition)#Generation_7
            // Damage Calculation already takes burn into effect
            int burnDamage = this.GetMaxHPStat() / 16;
            this.SetHPStat(Mathf.FloorToInt(hpStat.Value - burnDamage));
            return;
        }
        else if (this.status.Value == StatusConditions.Paralysis)
        {
            this.SetSpeedStat(Mathf.FloorToInt(0.01f * (2 * this.baseSpeed + this.ivs.speed + Mathf.FloorToInt(0.25f * this.evs.speed)) * level) + 5);
            this.SetSpeedStat(this.speedStat / 2);
            this.SetSpeedStat(Mathf.FloorToInt(this.speedStat * stageConversionDictionary[speedStage.Value]));
        }
        //}
    }
    public void ResetStatStages()
    {
        if (IsHost)
        {
            this.AttackStage = 0;
            this.DefenseStage = 0;
            this.SpecialAttackStage = 0;
            this.SpecialDefenseStage = 0;
            this.SpeedStage = 0;
        }
        else
        {
            RequestStatChangeRpc(Stats.Attack, 0);
            RequestStatChangeRpc(Stats.SpecialAttack, 0);
            RequestStatChangeRpc(Stats.Defense, 0);
            RequestStatChangeRpc(Stats.SpecialDefense, 0);
            RequestStatChangeRpc(Stats.Speed, 0);
        }
    }
    public void ResetBattleEffects()
    {
        isPressured.Value = false;
        isLevitating.Value = false;
    }
    private void HandleStatusChange(StatusConditions conditions)
    {
        if (conditions != StatusConditions.Healthy)
        {
            this.StatusEffect();
            TriggerStatusEffectOnHostRpc();
        }
    }

    private void HandleTriggeredEvent(EventsToTrigger e)
    {
        if (e == EventsToTrigger.YourPokemonSwitched)
        {
            if (this.Status != StatusConditions.Healthy)
            {
                this.StatusEffect();
                TriggerStatusEffectOnHostRpc();
            }
        }
    }
    [Rpc(SendTo.Server)]
    public void RequestStatChangeRpc(Stats stat, int value)
    {
        switch (stat)
        {
            case Stats.Attack:
                this.AttackStage = value; 
                break;
            case Stats.SpecialAttack:
                this.SpecialAttackStage = value;
                break;
            case Stats.Defense:
                this.DefenseStage = value;
                break;
            case Stats.SpecialDefense:
                this.SpecialDefenseStage = value;
                break;
            case Stats.Speed:
                this.SpeedStage = value;
                break;
            case Stats.Evasion:
                break;
            case Stats.Accuracy:
                break;
        }
    }
    [Rpc(SendTo.Server)]
    public void RequestFieldChangeRpc(PokemonFields field, bool value)
    {
        switch(field)
        {
            case PokemonFields.isPressured:
                this.IsPressured = value;
                break;
            case PokemonFields.isLevitating: 
                this.IsLevitating = value;
                Debug.Log($"Changed value of IsLevitating to {value}");
                break;
        }
    }

    [Rpc(SendTo.Server)]
    public void RequestStatusChangeRpc(StatusConditions statusCondition)
    {
        Debug.Log($"{this.nickname} is being changed to {statusCondition}");
        this.Status = statusCondition;
    }

    [Rpc(SendTo.NotServer)]
    private void AlertNewStatusOnClientRpc(StatusConditions statusCondition)
    {
        //RequestStatusChangeRpc(statusCondition);
        StatusChanged?.Invoke(statusCondition);
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void SendLastAttackFromThisPokemonRpc(string attackName)
    {
        GameManager.Instance.AddRPCTaskRpc();
        Attack attack = (Attack)Activator.CreateInstance(System.Type.GetType(attackName.ToString().Replace(" ", "")));
        SetLastAttack(attack);
    }

    [Rpc(SendTo.Server)]
    public void TriggerStatusEffectOnHostRpc()
    {
        this.StatusEffect();
    }
}