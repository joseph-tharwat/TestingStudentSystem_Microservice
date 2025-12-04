using System.Text.Json.Serialization;

namespace TestManagment.Shared.Result
{
    public class ErrorNote
    {
        public string Note { get; }
        [JsonIgnore]
        private ErrorType Type;

        public ErrorNote(ErrorType type, string message) 
        {
            Note = message;
            Type = type;
        }
        public int ToStatusCode()
        {
            return Type switch 
            {
                ErrorType.Validation=> StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.DomainRuleViolation => StatusCodes.Status422UnprocessableEntity,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };
        }
    }

    public enum ErrorType
    {
        Validation,
        NotFound,
        Conflict,
        DomainRuleViolation,
        Unauthorized,
        Forbidden,
        Unexpected
    }
}
