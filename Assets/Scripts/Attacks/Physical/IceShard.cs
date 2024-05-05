public class IceShard : Attack
{
    public IceShard()
    {
        this.attackName = "Ice Shard";
        this.description = "The user flash-freezes chunks of ice and hurls them at the target. This move always goes first.";
        this.type = StaticTypeObjects.Ice;
        this.moveCategory = AttackCategory.Physical;
        this.power = 40;
        this.accuracy = 100;
        this.priority = 1;
        this.currPowerPoints = 48;
        this.maxPowerPoints = 48;
    }
}