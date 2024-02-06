using UnityEditor;
using UnityEngine;

public class StaticTypeObjects : MonoBehaviour
{

    //public static Normal Normal = ScriptableObject.CreateInstance<Normal>();
    //public static Fire Fire = ScriptableObject.CreateInstance<Fire>();
    //public static Water Water = ScriptableObject.CreateInstance<Water>();
    //public static Electric Electric = ScriptableObject.CreateInstance<Electric>();
    //public static Grass Grass = ScriptableObject.CreateInstance<Grass>();
    //public static Ice Ice = ScriptableObject.CreateInstance<Ice>();
    //public static Fighting Fighting = ScriptableObject.CreateInstance<Fighting>();
    //public static Poison Poison = ScriptableObject.CreateInstance<Poison>();
    //public static Ground Ground = ScriptableObject.CreateInstance<Ground>();
    //public static Flying Flying = ScriptableObject.CreateInstance<Flying>();
    //public static Psychic Psychic = ScriptableObject.CreateInstance<Psychic>();
    //public static Bug Bug = ScriptableObject.CreateInstance<Bug>();
    //public static Rock Rock = ScriptableObject.CreateInstance<Rock>();
    //public static Ghost Ghost = ScriptableObject.CreateInstance<Ghost>();
    //public static Dragon Dragon = ScriptableObject.CreateInstance<Dragon>();
    //public static Dark Dark = ScriptableObject.CreateInstance<Dark>();
    //public static Steel Steel = ScriptableObject.CreateInstance<Steel>();
    //public static Fairy Fairy = ScriptableObject.CreateInstance<Fairy>();

    public static Normal Normal;
    public static Fire Fire;
    public static Water Water;
    public static Electric Electric;
    public static Grass Grass;
    public static Ice Ice;
    public static Fighting Fighting;
    public static Poison Poison;
    public static Ground Ground;
    public static Flying Flying;
    public static Psychic Psychic;
    public static Bug Bug;
    public static Rock Rock;
    public static Ghost Ghost;
    public static Dragon Dragon;
    public static Dark Dark;
    public static Steel Steel;
    public static Fairy Fairy;

    private void Start()
    {
        Normal = new Normal();
        Fire = new Fire();
        Water = new Water();
        Electric = new Electric();
        Grass = new Grass();
        Ice = new Ice();
        Fighting = new Fighting();
        Poison = new Poison();
        Ground = new Ground();
        Flying = new Flying();
        Psychic = new Psychic();
        Bug = new Bug();
        Rock = new Rock();
        Ghost = new Ghost();
        Dragon = new Dragon();
        Dark = new Dark();
        Steel = new Steel();
        Fairy = new Fairy();
        InitializeAllTypes();
    }

    private void InitializeAllTypes()
    {
        Normal.InitializeValues();
        Fire.InitializeValues();
        Water.InitializeValues();
        Electric.InitializeValues();
        Grass.InitializeValues();
        Ice.InitializeValues();
        Fighting.InitializeValues();
        Poison.InitializeValues();
        Ground.InitializeValues();
        Flying.InitializeValues();
        Psychic.InitializeValues();
        Bug.InitializeValues();
        Rock.InitializeValues();
        Ghost.InitializeValues();
        Dragon.InitializeValues();
        Dark.InitializeValues(); 
        Steel.InitializeValues(); 
        Fairy.InitializeValues();
    }
}
