using System.Text.Json.Serialization;

namespace TestManagment.Shared.Result
{
    public class SuccessNote
    {
        public string Note { get; }
        [JsonIgnore]
        public SuccessType Type;

        public SuccessNote(SuccessType type, string note)
        {
            Note = note;
            Type = type;
        }

        public int ToStatusCode()
        {
            return Type switch
            {
                SuccessType.Ok => StatusCodes.Status200OK,
                SuccessType.Created => StatusCodes.Status201Created,
                SuccessType.NoContent => StatusCodes.Status204NoContent,
                SuccessType.Accepted => StatusCodes.Status202Accepted,
                _ => StatusCodes.Status200OK
            };
        }
    }

    public enum SuccessType
    {
        Ok,
        Created,
        NoContent,
        Accepted
    }
}