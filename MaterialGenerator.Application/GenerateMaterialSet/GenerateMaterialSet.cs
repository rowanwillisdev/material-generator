using MaterialGenerator.Application.Contracts.GenerateMaterialSet;
using Microsoft.Extensions.Logging;
using RowanWillis.Common.Application;
using RowanWillis.Common.LanguageExtensions.Process;
using Void = RowanWillis.Common.LanguageExtensions.Void;

namespace MaterialGenerator.Application.GenerateMaterialSet;

internal sealed record GenerateMaterialSetAction
{
    public required string Name { get; init; }
    public required string BaseColour { get; init; }
}

internal sealed class GenerateMaterialSetHandler : BaseCommandHandler<
    GenerateMaterialSetCommand,
    GenerateMaterialSetAction,
    GenerateMaterialSetResult>
{
    public GenerateMaterialSetHandler(
        IAuthorizer<GenerateMaterialSetCommand> authorizer,
        IValidator<GenerateMaterialSetCommand, GenerateMaterialSetAction> validator,
        IExecutor<GenerateMaterialSetAction, GenerateMaterialSetResult> executor,
        IUnitOfWork unitOfWork, ILogger<ICommandHandler<GenerateMaterialSetCommand, GenerateMaterialSetResult>> logger)
        : base(authorizer, validator, executor, unitOfWork, logger)
    {
    }
}

internal sealed class GenerateMaterialSetExectutor : IExecutor<GenerateMaterialSetAction, GenerateMaterialSetResult>
{
    public Task<ProcessResult<GenerateMaterialSetResult>> Execute(GenerateMaterialSetAction action, CancellationToken cancellationToken)
    {
        return Task.FromResult(ProcessResult<GenerateMaterialSetResult>.Success(new()));
    }
}

internal sealed class GenerateMaterialSetValidator : IValidator<GenerateMaterialSetCommand, GenerateMaterialSetAction>
{
    public Task<ProcessResult<GenerateMaterialSetAction>> Validate(GenerateMaterialSetCommand input, CancellationToken cancellationToken) => Task.FromResult(
        ProcessResult<GenerateMaterialSetAction>.Success(new() {
            Name = input.Name,
            BaseColour = input.BaseColour,
        }));
}

internal sealed class GenerateMaterialSetAuthorizer : IAuthorizer<GenerateMaterialSetCommand>
{
    public Task<ProcessResult<Void>> Authorize(GenerateMaterialSetCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult(ProcessResult.Success());
    }
}