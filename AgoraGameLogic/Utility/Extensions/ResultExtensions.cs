using System;
using System.Threading.Tasks;
using AgoraGameLogic.Actors;

namespace AgoraGameLogic.Utility.Extensions;

public static class ResultExtensions
{
    public static Result Then(this Result result, Func<Result> next)
    {
        return result.IsSuccess ? next() : result;
    }

    public static Result<T> Then<T>(this Result result, Func<Result<T>> next)
    {
        return result.IsSuccess ? next() : Result<T>.Failure(result.Error);
    }

    public static Result Then<T>(this Result<T> result, Func<T, Result> next)
    {
        return result.IsSuccess ? next(result.Value) : Result.Failure(result.Error);
    }

    public static Result<TOutput> Then<TInput, TOutput>(
        this Result<TInput> result,
        Func<TInput, Result<TOutput>> next)
    {
        return result.IsSuccess ? next(result.Value) : Result<TOutput>.Failure(result.Error);
    }
    
    // async
    
    public static async Task<Result> ThenAsync(this Task<Result> resultTask, Func<Task<Result>> next)
    {
        var result = await resultTask;
        return result.IsSuccess ? await next() : result;
    }

    public static async Task<Result<T>> ThenAsync<T>(this Task<Result> resultTask, Func<Task<Result<T>>> next)
    {
        var result = await resultTask;
        return result.IsSuccess ? await next() : Result<T>.Failure(result.Error);
    }

    public static async Task<Result> ThenAsync<T>(this Task<Result<T>> resultTask, Func<T, Task<Result>> next)
    {
        var result = await resultTask;
        return result.IsSuccess ? await next(result.Value) : Result.Failure(result.Error);
    }

    public static async Task<Result<TOutput>> ThenAsync<TInput, TOutput>(
        this Task<Result<TInput>> resultTask,
        Func<TInput, Task<Result<TOutput>>> next)
    {
        var result = await resultTask;
        return result.IsSuccess ? await next(result.Value) : Result<TOutput>.Failure(result.Error);
    }
}
