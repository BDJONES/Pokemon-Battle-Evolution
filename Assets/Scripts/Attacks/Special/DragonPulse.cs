public class DragonPulse : Attack
{
    public DragonPulse()
    {
        this.attackName = "Dragon Pulse";
        this.description = "The target is attacked with a shock wave generated by the user�s gaping mouth.";
        this.type = StaticTypeObjects.Dragon;
        this.moveCategory = AttackCategory.Special;
        this.power = 85;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 16;
        this.maxPowerPoints = 16;
    }
}