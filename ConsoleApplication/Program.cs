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

// setup logic handler
CancellationTokenSource? inputCancellationTokenSource = null;
var gameHasEnd = false;

gameLogic.OnGameStateChange += stateChangeResult =>
{
    if (gameHasEnd) return;
    
    inputCancellationTokenSource?.Cancel();
    
    inputCancellationTokenSource = new CancellationTokenSource();
    var token = inputCancellationTokenSource.Token;
    
    Console.WriteLine("\nResult: ");
    Console.WriteLine(stateChangeResult.ToString(true, true));
    
    try
    {
        while (true)
        {
            // Ask for user input unless cancelled
            Console.Write("\nEnter a commandId to perform: ");
            
            while (!Console.KeyAvailable)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("\nInput request cancelled. Awaiting new state...");
                    return;
                }
                Thread.Sleep(100); // Small delay to avoid busy waiting
            }
            
            var input = Console.ReadLine();
            if (int.TryParse(input, out var actionId) && actionId >= 0)
            {
                // Execute the selected action
                Console.Write("\n#####################################################\n");

                var playerId = "";
                foreach (var entry in stateChangeResult.Actions)
                {
                    foreach (var command in entry.Value)
                    {
                        if (command.Id == actionId)
                        {
                            playerId = entry.Key;
                            continue;
                        }
                    }
                    
                    if (!string.IsNullOrEmpty(playerId)) continue;
                }
                
                gameLogic.PerformAction(playerId, actionId);
                break; // Exit the loop after valid input
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

gameLogic.StartGame();
