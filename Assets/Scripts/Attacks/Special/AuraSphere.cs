public class AuraSphere : Attack
{
    public AuraSphere()
    {
        this.attackName = "Aura Sphere";
        this.description = "The user lets loose a pulse of aura power from deep within its body at the target. This attack never misses.";
        this.type = StaticTypeObjects.Fighting;
        this.moveCategory = AttackCategory.Special;
        this.power = 80;
        this.accuracy = 101;
        this.priority = 0;
        this.currPowerPoints = 32;
        this.maxPowerPoints = 32;
    }
}