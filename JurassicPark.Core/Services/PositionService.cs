using JurassicParkCore.DataSchemas;
using JurassicParkCore.Functional;
using JurassicParkCore.Services.Interfaces;

namespace JurassicParkCore.Services;

public class PositionService : IPositionService
{
    public IEnumerable<Position> GetPositions(JurassicParkDbContext context)
    {
        return context.Positions.All;
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