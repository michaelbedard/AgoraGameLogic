using System;
using System.IO;
using AgoraGameLogic;
using Newtonsoft.Json;

var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "TestBuild.json");

if (!File.Exists(jsonFilePath))
{
    Console.WriteLine($"JSON file not found. {jsonFilePath}");
    return;
}

var jsonContent = File.ReadAllText(jsonFilePath);

// load game
var gameLogic = new GameLogic();
gameLogic.LoadGame(jsonContent, 4);

// setup logic
var gameHasEnd = false;
int executionIndex = 0; // Track execution index

gameLogic.OnGameStateChange += stateChangeResult =>
{
    if (gameHasEnd) return;

    // log stateChangeResult
    Console.WriteLine("\n[RESULT]");
    Console.WriteLine(stateChangeResult.ToString(true, true));
    
    try
    {
        while (true)
        {
            // Ask for user input unless cancelled
            Console.Write("Enter a commandId and optional argument: ");
            
            var input = Console.ReadLine();
            var inputParts = input?.Split(' ', 2); // Split input into commandId and optional argument
            
            if (inputParts != null && int.TryParse(inputParts[0], out var actionId) && actionId >= 0)
            {
                var argument = inputParts.Length > 1 ? inputParts[1] : null; // Optional argument

                var playerId = "";
                foreach (var entry in stateChangeResult.Actions)
                {
                    foreach (var command in entry.Value)
                    {
                        if (command.Id == actionId)
                        {
                            playerId = entry.Key;
                            break;
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(playerId)) break;
                }

                if (!string.IsNullOrEmpty(playerId))
                {
                    // Track execution index only after valid input
                    executionIndex++;
                    Console.Write($"\n######################## EXECUTION {executionIndex}\n");
                    Console.WriteLine("[LOGS]");

                    if (string.IsNullOrEmpty(argument))
                    {
                        // Perform action without arguments
                        gameLogic.PerformAction(playerId, actionId);
                        Console.WriteLine("");
                    }
                    else
                    {
                        // Perform action with argument
                        gameLogic.PerformInput(playerId, actionId, argument);
                        Console.WriteLine("");
                    }
                    break; // Exit the loop after valid input
                }
                else
                {
                    Console.WriteLine("Action not found for the given commandId. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
        }
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("\nPrevious input cancelled.");
    }
};

gameLogic.OnEndGame += r =>
{
    gameHasEnd = true;
    Console.WriteLine(JsonConvert.SerializeObject(r));
};

Console.Write($"\n######################## START");
Console.WriteLine("\n[LOGS]");
gameLogic.StartGame();
