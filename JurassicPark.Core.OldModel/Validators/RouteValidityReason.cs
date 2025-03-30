namespace JurassicPark.Core.OldModel.Validators
{
    public enum RouteValidityReason
    {
        Valid,
        InsufficientPositions,
        NotRoundedCoordinates,
        PositionRepeated,
        InvalidLineAngle,
        BackwardsTurn,
        InvalidProblem,
        SelfIntersecting
    }
}