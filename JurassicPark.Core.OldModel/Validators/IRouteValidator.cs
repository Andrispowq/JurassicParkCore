using System.Collections.Generic;
using System.Threading.Tasks;
using JurassicPark.Core.OldModel.Dto;

namespace JurassicPark.Core.OldModel.Validators
{
    public interface IRouteValidator
    {
        /// <summary>
        /// Returns whether the route provided is a valid connection of the start and end positions
        /// </summary>
        /// <param name="route">The route to check</param>
        /// <param name="start">The start position needed</param>
        /// <param name="end">The end position needed</param>
        /// <returns>True if the route is valid, false otherwise</returns>
        bool IsComplete(JeepRoute route, Position start, Position end);

        /// <summary>
        /// Returns whether the route provided is a route without circles or invalid connections
        /// </summary>
        /// <param name="route">The route to check</param>
        /// <returns>Returns either Valid, or the problem</returns>
        RouteValidityReason Validate(JeepRoute route);

        /// <summary>
        /// Returns whether the route can be extended with a route block on the position provided
        /// </summary>
        /// <param name="route">The route to check</param>
        /// <param name="position">The next position for the new road block</param>
        /// <returns>True if the route can be extended, false otherwise</returns>
        bool CanAddRouteBlock(JeepRoute route, Position position);

        /// <summary>
        /// Returns whether the route can be shortened by removing the last route block
        /// </summary>
        /// <param name="route">The route to check</param>
        /// <returns>True if the route can be shortened, false otherwise</returns>
        bool CanRemoveElement(JeepRoute route);

        /// <summary>
        /// Check to see if the specified position is part of the route
        /// </summary>
        /// <param name="route">The route to check</param>
        /// <param name="position">The position to check, rounded down</param>
        /// <returns>False if the position is not rounded or not on the route, true otherwise</returns>
        bool IsPositionOnRoute(JeepRoute route, Position position);

        /// <summary>
        /// Checks to see if there are any other road blocks at the given position
        /// </summary>
        /// <param name="position">The position in question. Must be rounded to the nearest integer before testing</param>
        /// <param name="positions">All other positions already existing, and having JeepRoutes assotiated with them</param>
        /// <returns>Returns true if the position is already taken by another route block, false otherwise, or in case the position parameter is not rounded.</returns>
        bool IsPositionTaken(Position position, List<Position> positions);
    }
}