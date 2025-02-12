using JurassicParkCore.DataSchemas;
using JurassicParkCore.Functional;

namespace JurassicParkCore.Services.Interfaces;

public interface IPositionService
{
    IEnumerable<Position> GetPositions(JurassicParkDbContext context);
    Task<Option<ServiceError>> CreatePosition(JurassicParkDbContext context, Position position);
    Task<Option<ServiceError>> UpdatePosition(JurassicParkDbContext context, Position position);
    Task<Option<ServiceError>> DeletePosition(JurassicParkDbContext context, Position position);
}