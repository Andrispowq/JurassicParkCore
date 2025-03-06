namespace JurassicPark.Core.Model.Validators;

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