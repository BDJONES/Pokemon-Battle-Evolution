using UnityEditor;
using UnityEngine;

static class StaticAbilityObjects
{
	public static Intimidate Intimidate = ScriptableObject.CreateInstance<Intimidate>();
    public static Levitate Levitate = ScriptableObject.CreateInstance<Levitate>();
    public static Pressure Pressure = ScriptableObject.CreateInstance<Pressure>();
    public static Static Static = ScriptableObject.CreateInstance<Static>();
    public static Justified Justified = ScriptableObject.CreateInstance<Justified>();
    public static Synchronize Synchronize = ScriptableObject.CreateInstance<Synchronize>();
}