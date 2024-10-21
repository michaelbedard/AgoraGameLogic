using System.Text;
using AgoraGameLogic.Interfaces.Actors;

namespace AgoraGameLogic.Actors
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Error => GetErrorMessage();

        private string _errorDescription;
        private ErrorBuilder? _errorBuilder;

        protected Result(bool isSuccess, string error, ErrorBuilder? errorBuilder = null)
        {
            IsSuccess = isSuccess;
            _errorDescription = error;
            _errorBuilder = errorBuilder;
        }

        public static Result Success()
        {
            return new Result(true, string.Empty, null);
        }

        public static Result Failure(string error, ErrorBuilder? errorBuilder = null)
        {
            return new Result(false, error, errorBuilder);
        }
        
        public string GetErrorMessage()
        {
            if (_errorBuilder != null)
            {
                return _errorBuilder.GetErrorMessage(_errorDescription);
            }

            return _errorDescription;
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; }

        protected Result(bool isSuccess, string error, T value, ErrorBuilder? errorBuilder = null) : base(isSuccess, error, errorBuilder)
        {
            Value = value;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(true, string.Empty, value, null);
        }

        public new static Result<T> Failure(string error, ErrorBuilder? errorBuilder = null)
        {
            return new Result<T>(false, error, default, errorBuilder);
        }
    }

    public class ErrorBuilder
    {
        public string? ClassName { get; set; }
        public string? MethodName { get; set; }
        public GameModule? GameModule { get; set; }
        public TurnScope? Scope { get; set; }
        public string? Details { get; set; }

        public string GetErrorMessage(string error)
        {
            var builder = new StringBuilder();
            builder.AppendLine(error);
            
            if (ClassName != null)
            {
                builder.AppendLine($"ClassName: " + ClassName);
            }
            
            if (MethodName != null)
            {
                builder.AppendLine($"MethodName: " + MethodName);
            }
            
            if (GameModule != null)
            {
                builder.AppendLine($"GameModule: " + GameModule.ToString());
            }
            
            if (Scope != null)
            {
                builder.AppendLine($"Context: " + Scope.ToString());
            }
            
            if (Details != null)
            {
                builder.AppendLine($"Details: " + Details);
            }

            return builder.ToString();
        }
    }
}