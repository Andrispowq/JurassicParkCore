namespace JurassicPark.Core.OldModel.Functional
{
    public abstract class ServiceError : Error
    {
        protected ServiceError(string message) : base(message)
        {
        }
    }

    public class NotFoundError : ServiceError
    {
        public NotFoundError(string message) : base(message)
        {
        }
    }

    public class BadRequestError : ServiceError
    {
        public BadRequestError(string message) : base(message)
        {
        }
    }

    public class ConflictError : ServiceError
    {
        public ConflictError(string message) : base(message)
        {
        }
    }

    public class UnauthorizedError : ServiceError
    {
        public UnauthorizedError(string message) : base(message)
        {
        }
    }

    public class UnprocessableEntityError : ServiceError
    {
        public UnprocessableEntityError() : base("Unprocessable entity")
        {
        }
    }
}