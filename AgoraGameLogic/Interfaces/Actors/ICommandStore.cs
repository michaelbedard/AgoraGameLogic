using System.Collections.Generic;
using AgoraGameLogic.Actors;
using AgoraGameLogic.Utility.Commands;

namespace AgoraGameLogic.Interfaces.Actors;

public interface ICommandStore<T> where T : Command
{
    /// <summary>
    /// Registers a command in the store.
    /// </summary>
    /// <param name="command">The command to register.</param>
    /// <returns>A Result object indicating success or failure.</returns>
    Result RegisterCommand(T command);

    /// <summary>
    /// Retrieves a command by its ID.
    /// </summary>
    /// <param name="id">The ID of the command.</param>
    /// <returns>A Result containing the command if found, or a failure message.</returns>
    Result<T> GetCommand(int id);

    /// <summary>
    /// Retrieves all registered commands.
    /// </summary>
    /// <returns>A Result containing an IEnumerable of commands.</returns>
    Result<IEnumerable<T>> GetAllCommands();

    /// <summary>
    /// Checks if a command with the specified ID exists.
    /// </summary>
    /// <param name="id">The ID of the command.</param>
    /// <returns>True if the command exists, false otherwise.</returns>
    Result<bool> ContainsCommand(int id);

    /// <summary>
    /// Removes a command by its ID.
    /// </summary>
    /// <param name="id">The ID of the command to remove.</param>
    Result RemoveCommand(int id);

    /// <summary>
    /// Removes the specified command.
    /// </summary>
    /// <param name="command">The command to remove.</param>
    Result RemoveCommand(T command);
}