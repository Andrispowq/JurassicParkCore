using System;
using System.Collections.Generic;
using System.Linq;
using JurassicPark.Core.OldModel.Dto;

namespace JurassicPark.Core.OldModel.Validators
{
    public class RouteValidator : IRouteValidator
    {
        public bool IsComplete(JeepRoute route, Position start, Position end)
        {
            var positions = route.RoutePositions;
            if (positions.Count < 2) return false;

            var first = positions.First();
            var last = positions.Last();
            if (!first.EqualsTo(start) || !last.EqualsTo(end)) return false;

            return Validate(route) == RouteValidityReason.Valid;
        }

        public RouteValidityReason Validate(JeepRoute route)
        {
            var positions = route.RoutePositions;
            return IsValidRoute(positions);
        }

        public bool CanAddRouteBlock(JeepRoute route, Position position)
        {
            var positions = route.RoutePositions;
            if (!positions.Any()) return true; // If the route is empty, we can start it

            var last = positions.Last();
            return last.IsAdjacentTo(position);
        }

        public bool CanRemoveElement(JeepRoute route)
        {
            return route.RoutePositions.Count > 0;
        }

        public bool IsPositionOnRoute(JeepRoute route, Position position)
        {
            var positions = route.RoutePositions.ToList();
            if (positions.Count == 0) return false;
            if (positions.Any(p => p.EqualsTo(position))) return true;

            for (int i = 0; i < positions.Count - 1; i++)
            {
                var start = positions[i];
                var end = positions[i + 1];

                // For horizontal segments: Y remains constant.
                if (Math.Abs(start.Y - end.Y) < Position.DoubleDelta
                    && Math.Abs(position.Y - start.Y) < Position.DoubleDelta)
                {
                    if (position.X >= Math.Min(start.X, end.X) && position.X <= Math.Max(start.X, end.X)) return true;
                }
                // For vertical segments: X remains constant.
                else if (Math.Abs(start.X - end.X) < Position.DoubleDelta
                         && Math.Abs(position.X - start.X) < Position.DoubleDelta)
                {
                    if (position.Y >= Math.Min(start.Y, end.Y) && position.Y <= Math.Max(start.Y, end.Y))
                        return true;
                }
            }

            return false;
        }

        public bool IsPositionTaken(Position position, List<Position> positions)
        {
            if (!position.IsRoundedDown) return false;

            return positions
                //.Where(p => p.JeepRouteId != null) //only pass these
                .Any(p => p.EqualsTo(position));
        }

        /// <summary>
        /// Validates the route based on several rules:
        /// 1. All positions must be on the grid (integer values).
        /// 2. Each segment must be horizontal or vertical.
        /// 3. At any turn, segments must be perpendicular.
        /// 4. When segments are collinear, they must not reverse direction.
        /// 5. The route must not contain duplicate points (loops).
        /// 6. Non-adjacent segments must not intersect.
        /// </summary>
        private static RouteValidityReason IsValidRoute(ICollection<Position> positions)
        {
            // A valid route must have at least two points.
            if (positions.Count < 2) return RouteValidityReason.InsufficientPositions;

            var pts = positions.ToList();
            int n = pts.Count;

            // Check grid validity and ensure no point is repeated (no loops).
            var seen = new HashSet<(double, double)>();
            foreach (var p in pts)
            {
                if (p.X % 1 != 0 || p.Y % 1 != 0) return RouteValidityReason.NotRoundedCoordinates;

                var key = (p.X, p.Y);
                if (!seen.Add(key)) return RouteValidityReason.PositionRepeated;
            }

            // Check that each segment is axis aligned (horizontal or vertical).
            for (int i = 0; i < n - 1; i++)
            {
                var p1 = pts[i];
                var p2 = pts[i + 1];

                // Must be horizontal or vertical.
                if (!(Math.Abs(p1.X - p2.X) < Position.DoubleDelta
                      || Math.Abs(p1.Y - p2.Y) < Position.DoubleDelta))
                {
                    return RouteValidityReason.InvalidLineAngle;
                }
            }

            // Validate the "step by step" and turning conditions.
            // For every triple (prev, current, next), if the segments are collinear, ensure the movement
            // continues in the same direction. Otherwise, if there is a turn, it must be 90°.
            for (var i = 1; i < n - 1; i++)
            {
                var prev = pts[i - 1];
                var curr = pts[i];
                var next = pts[i + 1];

                var prevHorizontal = Math.Abs(prev.Y - curr.Y) < Position.DoubleDelta;
                var prevVertical = Math.Abs(prev.X - curr.X) < Position.DoubleDelta;

                var currHorizontal = Math.Abs(curr.Y - next.Y) < Position.DoubleDelta;
                var currVertical = Math.Abs(curr.X - next.X) < Position.DoubleDelta;

                // For collinear segments, check that we are not going backwards.
                if (prevHorizontal && currHorizontal)
                {
                    double dir1 = curr.X - prev.X;
                    double dir2 = next.X - curr.X;
                    // If the product is negative, the directions are opposite.
                    if (dir1 * dir2 < 0) return RouteValidityReason.BackwardsTurn;
                }
                else if (prevVertical && currVertical)
                {
                    double dir1 = curr.Y - prev.Y;
                    double dir2 = next.Y - curr.Y;
                    if (dir1 * dir2 < 0) return RouteValidityReason.BackwardsTurn;
                }
                // For a turn, ensure the segments are perpendicular.
                else if ((prevHorizontal && currVertical) || (prevVertical && currHorizontal))
                {
                    // Valid turn: one segment is horizontal, the other vertical.
                }
                else
                {
                    // This case should not occur because segments are enforced to be axis aligned.
                    return RouteValidityReason.InvalidProblem;
                }
            }

            // Check for self-intersection.
            // Iterate through every pair of non-adjacent segments.
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n - 1; j++)
                {
                    // Skip adjacent segments (they share a common endpoint).
                    if (j == i || j == i + 1) continue;
                    if (DoIntersect(pts[i], pts[i + 1], pts[j], pts[j + 1]))
                        return RouteValidityReason.SelfIntersecting;
                }
            }

            return RouteValidityReason.Valid;
        }

        /// <summary>
        /// Determines if two segments (p1,q1) and (p2,q2) intersect.
        /// This method uses orientation tests and handles collinear cases.
        /// </summary>
        private static bool DoIntersect(Position p1, Position q1, Position p2, Position q2)
        {
            var o1 = Orientation(p1, q1, p2);
            var o2 = Orientation(p1, q1, q2);
            var o3 = Orientation(p2, q2, p1);
            var o4 = Orientation(p2, q2, q1);

            // General case.
            if (o1 != o2 && o3 != o4) return true;

            // Special Cases:
            if (o1 == 0 && OnSegment(p1, q1, p2)) return true;
            if (o2 == 0 && OnSegment(p1, q1, q2)) return true;
            if (o3 == 0 && OnSegment(p2, q2, p1)) return true;
            if (o4 == 0 && OnSegment(p2, q2, q1)) return true;

            return false;
        }

        /// <summary>
        /// Computes the orientation of the triplet (p, q, r).
        /// Returns:
        ///  0 -> Collinear
        ///  1 -> Clockwise
        ///  2 -> Counterclockwise
        /// </summary>
        private static int Orientation(Position p, Position q, Position r)
        {
            double val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
            if (val == 0) return 0;
            return val > 0 ? 1 : 2;
        }

        /// <summary>
        /// Checks if point r lies on the line segment p-q.
        /// </summary>
        private static bool OnSegment(Position p, Position q, Position r)
        {
            return r.X <= Math.Max(p.X, q.X) && r.X >= Math.Min(p.X, q.X) &&
                   r.Y <= Math.Max(p.Y, q.Y) && r.Y >= Math.Min(p.Y, q.Y);
        }
    }
}