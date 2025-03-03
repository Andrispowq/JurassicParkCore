using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.Services;

public class PositionService : IPositionService
{
    public IEnumerable<Position> GetPositions(JurassicParkDbContext context)
    {
        return context.Positions.All;
    }

    public async Task<Result<Position, ServiceError>> GetPositionById(JurassicParkDbContext context, long id)
    {
        var position = await context.Positions.Get(id);
        return position.Map<Result<Position, ServiceError>>(p => p, e => new NotFoundError(e.Message));
    }

    public async Task<Option<ServiceError>> CreatePosition(JurassicParkDbContext context, Position position)
    {
        var result = await context.Positions.Create(position);
        return result.MapOption<ServiceError>(error => new ConflictError(error.Message));
    }

    public async Task<Option<ServiceError>> UpdatePosition(JurassicParkDbContext context, Position position)
    {
        var result = await context.Positions.Update(position);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }

    public async Task<Option<ServiceError>> DeletePosition(JurassicParkDbContext context, Position position)
    {
        var result = await context.Positions.Delete(position);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }
}