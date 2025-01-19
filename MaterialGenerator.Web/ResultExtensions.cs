using RowanWillis.Common.LanguageExtensions.Process;

namespace MaterialGenerator.Web;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this ProcessResult<T> result) => result switch
    {
        { IsSuccess: true } success => Results.Ok(success.Value),
        { IsValidationFailure: true } validationFailure => Results.Ok(validationFailure.ValidationErrors),
        { IsResourceNotFound: true } => Results.NotFound(),
        { IsAuthenticationFailure: true } => Results.Unauthorized(),
        { IsAuthorizationFailure: true } => Results.Forbid(),
        _ => throw new Exception("Unexpected result type.")
    };
    
    public static IResult ToHttpResult<T, S>(this ProcessResult<T> result, Func<T, S> map) => result switch
    {
        { IsSuccess: true } success => Results.Ok(map(success.Value!)),
        { IsValidationFailure: true } validationFailure => Results.Ok(validationFailure.ValidationErrors),
        { IsResourceNotFound: true } => Results.NotFound(),
        { IsAuthenticationFailure: true } => Results.Unauthorized(),
        { IsAuthorizationFailure: true } => Results.Forbid(),
        _ => throw new Exception("Unexpected result type.")
    };
}