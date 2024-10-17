using AgoraGameLogic.Domain.Interfaces;
using AgoraGameLogic.Logic.Blocks;

namespace AgoraGameLogic.Control.Services;

public class ExecutionService : IExecutionService
{
    private List<StatementBlockBase> _executionList;
}