using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;

namespace JurassicPark.Core.Services.Interfaces;

public interface IPositionService
{
    IEnumerable<Position> GetPositions(JurassicParkDbContext context);
    Task<Result<Position, ServiceError>> GetPositionById(JurassicParkDbContext context, long id);
    Task<Option<ServiceError>> CreatePosition(JurassicParkDbContext context, Position position);
    Task<Option<ServiceError>> UpdatePosition(JurassicParkDbContext context, Position position);
    Task<Option<ServiceError>> DeletePosition(JurassicParkDbContext context, Position position);
}