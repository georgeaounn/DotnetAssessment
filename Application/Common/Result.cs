using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public IReadOnlyList<string>? Errors { get; }

        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string? error, IReadOnlyList<string>? errors)
        {
            IsSuccess = isSuccess;
            Error = error;
            Errors = errors;
        }

        public static Result Success() =>
            new Result(true, null, null);

        public static Result Failure(string error) =>
            new Result(false, error, new[] { error });

        public static Result Failure(IEnumerable<string> errors)
        {
            var list = errors.ToList();
            var first = list.FirstOrDefault();
            return new Result(false, first, list);
        }
    }
    public class Result<T> : Result
    {
        public T? Data { get; }

        private Result(bool isSuccess, T? data, string? error, IReadOnlyList<string>? errors)
            : base(isSuccess, error, errors)
        {
            Data = data;
        }

        public static Result<T> Success(T data) =>
            new Result<T>(true, data, null, null);

        public new static Result<T> Failure(string error) =>
            new Result<T>(false, default, error, new[] { error });

        public new static Result<T> Failure(IEnumerable<string> errors)
        {
            var list = errors.ToList();
            var first = list.FirstOrDefault();
            return new Result<T>(false, default, first, list);
        }
    }
}
