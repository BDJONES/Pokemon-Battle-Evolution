public class BrickBreak : Attack
{
    public BrickBreak()
    {
        this.attackName = "Brick Break";
        this.description = "The user attacks with a swift chop. This move can also break barriers, such as Light Screen and Reflect.";
        this.type = StaticTypeObjects.Fighting;
        this.moveCategory = AttackCategory.Physical;
        this.power = 75;
        this.accuracy = 100;
        this.priority = 0;
        this.currPowerPoints = 24;
        this.maxPowerPoints = 24;
        this.isContact = true;
    }

    protected override void TriggerEffect(Pokemon attacker, Pokemon target)
    {
        base.TriggerEffect(attacker, target);
        // Add the ability to remove screen effects
    }
}