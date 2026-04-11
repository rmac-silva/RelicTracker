using MegaCrit.Sts2.Core.ValueProps;

public static class PublicPropExtensions
{
    public static bool IsPoweredAttackRelicTracker(this ValueProp props)
    {
        if (props.HasFlag(ValueProp.Move))
        {
            return !props.HasFlag(ValueProp.Unpowered);
        }

        return false;
    }

    public static bool IsPoweredCardOrMonsterMoveBlockRelicTracker(this ValueProp props)
    {
        if (props.HasFlag(ValueProp.Move))
        {
            return !props.HasFlag(ValueProp.Unpowered);
        }

        return false;
    }

    public static bool IsCardOrMonsterMoveRelicTracker(this ValueProp props)
    {
        return props.HasFlag(ValueProp.Move);
    }
}