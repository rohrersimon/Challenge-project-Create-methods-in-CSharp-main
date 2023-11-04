using System;

/*
Console minigame
*/

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Tracks eaten parts of the food
bool part1 = true;
bool part2 = true;
bool part3 = true;
bool part4 = true;
bool part5 = true;

// Player speed
int speed = 1;

// Available player and food strings
string[] states = { "('-')", "(^-^)", "(X_X)" };
string[] foods = { "@@@@@", "$$$$$", "#####" };

// Current player string displayed in the Console
string player = states[0];

// Index of the current food
int food = 0;

InitializeGame();
while (!shouldExit)
{
    Move();
}

bool AllFoodEaten()
{
    if (!part1 && !part2 && !part3 && !part4 && !part5)
    {
        // Change player to new outlook based on food eaten
        // Freeze Player movment based on food eaten
        if (food == 0 || food == 1)
        {
            ChangePlayer();
        }
        else
        {
            ChangePlayer();
            FreezePlayer();
        }
        return true;
    }
    return false;
}

void FoodTracker()
{
    if (playerY == foodY)
    {
        if (playerX == foodX)
        {
            part1 = false;
            part2 = false;
            part3 = false;
            part4 = false;
            part5 = false;
        }
        else if (playerX == foodX + 1)
        {
            part2 = false;
            part3 = false;
            part4 = false;
            part5 = false;
        }
        else if (playerX == foodX + 2)
        {
            part3 = false;
            part4 = false;
            part5 = false;
        }
        else if (playerX == foodX + 3)
        {
            part4 = false;
            part5 = false;
        }
        else if (playerX == foodX + 4)
        {
            part5 = false;
        }
        else if (playerX == foodX - 1)
        {
            part1 = false;
            part2 = false;
            part3 = false;
            part4 = false;
        }
        else if (playerX == foodX - 2)
        {
            part1 = false;
            part2 = false;
            part3 = false;
        }
        else if (playerX == foodX - 3)
        {
            part1 = false;
            part2 = false;
        }
        else if (playerX == foodX - 4)
        {
            part1 = false;
        }
    }
}

// Exits game if the Terminal was resized 
void ConsoleResizeCheck()
{
    if (height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5)
    {
        ExitGame("Console was resized. Program exiting.");
    }
}

// Displays random food at a random location
void ShowFood()
{
    // Update food to a random index
    food = random.Next(0, foods.Length);

    // Update food position to a random location
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);

    part1 = true;
    part2 = true;
    part3 = true;
    part4 = true;
    part5 = true;
}

// Changes the player to match the food consumed
void ChangePlayer()
{
    player = states[food];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Temporarily stops the player from moving
void FreezePlayer()
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

void AdjustSpeed(bool fast = true)
{
    if (!fast)
    {
        return;
    }
    
    if (player == states[0])
    {
        speed = 1;
    }
    else if (player == states[1])
    {
        speed = 3;
    }
}

// Reads directional input from the Console and moves the player
void Move(bool fussy = false)
{
    int lastX = playerX;
    int lastY = playerY;

    AdjustSpeed();

    switch (Console.ReadKey(true).Key)
    {
        case ConsoleKey.UpArrow:
            playerY--;
            break;
        case ConsoleKey.DownArrow:
            playerY++;
            break;
        case ConsoleKey.LeftArrow:
            playerX -=speed;
            break;
        case ConsoleKey.RightArrow:
            playerX +=speed;
            break;
        case ConsoleKey.Escape:
            shouldExit = true;
            break;
        default:
            if (fussy)
            {
                ExitGame("Invalid key was pressed. Program exiting.");
                return;
            }
            break;
    }

    // Clear the characters at the previous position
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++)
    {
        Console.Write(" ");
    }

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Draw the player at the new location
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);

    FoodTracker();
    if (AllFoodEaten())
    {
        ShowFood();
    }
    ConsoleResizeCheck();
}

// Clears the console, displays the food and player
void InitializeGame()
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}

void ExitGame(string message = "")
{
    Console.Clear();
    Console.WriteLine(message);
    shouldExit = true;
}