using System.Text.Json.Serialization;

namespace TestManagment.Shared.Result
{
    public class Result
    {
        public bool IsSuccess { get; }
        [JsonIgnore]
        public bool IsFailure  => !IsSuccess;
        public ErrorNote Error {  get; } 
        public int StatusCode {get;}
        public SuccessNote SuccessNote { get; }

        protected Result(bool isSuccess, ErrorNote error, SuccessNote successNote) 
        {
            IsSuccess = isSuccess;
            Error = error;
            SuccessNote = successNote;
            if (IsSuccess == true && successNote != null)
            {
                StatusCode = successNote.ToStatusCode();
            }
            else if(isSuccess == false && error != null)
            {
                StatusCode = Error.ToStatusCode();
            }
        }

        public static Result Success(SuccessNote successNote)
        {
            return new Result(true, null, successNote);
        }
        public static Result Failure(ErrorNote error)
        {
            return new Result(false, error, null);
        }
    }

    public class Result<T> : Result
    {
        public T Data {  get; }
        private Result(bool isSuccess,T data, ErrorNote error, SuccessNote successNote) : base(isSuccess, error, successNote)
        {
            Data = data;
        }

        public static Result<T> Success(T data, SuccessNote successNote)
        {
            return new Result<T>(true, data, null, successNote);
        }
        public static Result<T> Failure(ErrorNote error)
        {
            return new Result<T>(false, default, error, null);
        }
    }
}
