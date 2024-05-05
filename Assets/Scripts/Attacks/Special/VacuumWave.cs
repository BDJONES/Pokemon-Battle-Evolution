public class VacuumWave : Attack
{
    public VacuumWave()
    {
        this.attackName = "Vacuum Wave";
        this.description = "The user whirls its fists to send a wave of pure vacuum at the target. This move always goes first.";
        this.type = StaticTypeObjects.Fighting;
        this.moveCategory = AttackCategory.Special;
        this.power = 40;
        this.accuracy = 100;
        this.priority = 1;
        this.currPowerPoints = 48;
        this.maxPowerPoints = 48;
    }
}