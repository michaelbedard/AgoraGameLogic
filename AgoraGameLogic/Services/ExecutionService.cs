using System.Collections.Generic;
using AgoraGameLogic.Blocks;
using AgoraGameLogic.Interfaces.Services;

namespace AgoraGameLogic.Services;

public class ExecutionService : IExecutionService
{
    private List<StatementBlock> _executionList;
}