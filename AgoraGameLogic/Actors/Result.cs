using System.Text;
using AgoraGameLogic.Domain.Entities.Models;
using AgoraGameLogic.Domain.Interfaces;

namespace AgoraGameLogic.Entities
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
        public IContext? Context { get; set; }
        public GameModule? GameModule { get; set; }
        public Type? Type { get; set; }
        public Scope? Scope { get; set; }
        
        public string GetErrorMessage(string error)
        {
            var builder = new StringBuilder();
            builder.AppendLine(error);
            
            if (GameModule != null)
            {
                builder.AppendLine($"GameModule: " + GameModule.ToString());
            }
            
            if (Type != null)
            {
                builder.AppendLine($"Type: " + nameof(Type));
            }
            
            if (Context != null)
            {
                builder.AppendLine($"Context: " + Context.ToString());
            }

            if (Scope != null)
            {
                builder.AppendLine($"Context: " + Scope.ToString());
            }

            return builder.ToString();
        }
    }
}