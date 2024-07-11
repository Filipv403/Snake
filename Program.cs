namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Snake snakeGame = new Snake();
            snakeGame.Start();
        }
    }

    public class Snake
    {
        public char Character { get; } = '■';
        public SnakeDirection Direction { get; set; } = SnakeDirection.Right;

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public int GameSpeed { get; set; }
        public ConsoleColor BerryColor { get; set; }
        public ConsoleColor HeadColor { get; set; }
        public ConsoleColor BodyColor { get; set; }


        public Random RandomNumber { get; set; }
        public int Score { get; set; }
        public bool IsGameOver { get; set; }
        public Pixel Head {  get; set; }
        public List<Pixel> Body { get; set; }
        public Pixel Berry { get; set; }

        public Snake(int screenWidth = 32, int screenHeight = 16, int gameSpeed = 500,
            ConsoleColor berryColor = ConsoleColor.Cyan, ConsoleColor headColor = ConsoleColor.Red, ConsoleColor bodyColor = ConsoleColor.Green)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            GameSpeed = gameSpeed;
            BerryColor = berryColor;
            HeadColor = headColor;
            BodyColor = bodyColor;

            RandomNumber = new Random();
            Body = new List<Pixel>();

            Console.WindowHeight = ScreenHeight;
            Console.WindowWidth = ScreenWidth;
            Head = new Pixel(ScreenWidth / 2, ScreenHeight / 2, HeadColor);
            Berry = new Pixel(RandomNumber.Next(1, ScreenWidth - 2), RandomNumber.Next(1, ScreenHeight - 2), BerryColor);
            Score = 5;
        }

        public void Start()
        {
            while (!IsGameOver)
            {
                DrawBorders();
                DrawGameObjects();
                ProcessInput();
                MoveSnake();
                CheckForCollisions();
                Thread.Sleep(GameSpeed);
            }
            EndGame();
        }

        private void DrawBorders()
        {
            Console.Clear();
            DrawLine(0, 0, ScreenWidth - 1, 0, Character);
            DrawLine(0, ScreenHeight - 1, ScreenWidth - 1, ScreenHeight - 1, Character);
            DrawLine(0, 0, 0, ScreenHeight - 1, Character);
            DrawLine(ScreenWidth - 1, 0, ScreenWidth - 1, ScreenHeight - 1, Character);
        }

        private void DrawLine(int xStart, int yStart, int xEnd, int yEnd, char character)
        {
            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = yStart; y <= yEnd; y++)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.SetCursorPosition(x, y);
                    Console.Write(character);
                }
            }
        }

        private void DrawGameObjects()
        {          
            foreach (var part in Body)
            {
                Console.SetCursorPosition(part.X, part.Y);
                Console.ForegroundColor = part.Color;
                Console.Write(Character);
            }

            Console.SetCursorPosition(Head.X, Head.Y);
            Console.ForegroundColor = Head.Color;
            Console.Write(Character);

            Console.SetCursorPosition(Berry.X, Berry.Y);
            Console.ForegroundColor = Berry.Color;
            Console.Write(Character);
        }

        private void ProcessInput()
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow when Direction != SnakeDirection.Down:
                        Direction = SnakeDirection.Up;
                        break;
                    case ConsoleKey.DownArrow when Direction != SnakeDirection.Up:
                        Direction = SnakeDirection.Down;
                        break;
                    case ConsoleKey.LeftArrow when Direction != SnakeDirection.Right:
                        Direction = SnakeDirection.Left;
                        break;
                    case ConsoleKey.RightArrow when Direction != SnakeDirection.Left:
                        Direction = SnakeDirection.Right;
                        break;
                }
            }
        }

        private void MoveSnake()
        {
            int xPos = Head.X;
            int yPos = Head.Y;
            switch (Direction)
            {
                case SnakeDirection.Up:
                    yPos--;
                    break;
                case SnakeDirection.Down:
                    yPos++;
                    break;
                case SnakeDirection.Left:
                    xPos--;
                    break;
                case SnakeDirection.Right:
                    xPos++;
                    break;
            }
            Body.Add(new Pixel(Head.X, Head.Y, BodyColor));
            Head = new Pixel(xPos, yPos, Head.Color);

            if (Body.Count > Score)
            {
                Body.RemoveAt(0);
            }
        }

        private void CheckForCollisions()
        {
            if (Head.X == 0 || Head.X == ScreenWidth - 1 || Head.Y == 0 || Head.Y == ScreenHeight - 1)
            {
                IsGameOver = true;
                return;
            }

            foreach (var part in Body)
            {
                if (part.X == Head.X && part.Y == Head.Y)
                {
                    IsGameOver = true;
                    return;
                }
            }

            if (Head.X == Berry.X && Head.Y == Berry.Y)
            {
                Score++;
                Berry = new Pixel(RandomNumber.Next(1, ScreenWidth - 2), RandomNumber.Next(1, ScreenHeight - 2), BerryColor);
            }

            return;
        }

        private void EndGame()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2);
            Console.WriteLine($"Game over, Score: {Score}");
        }

    }

    public class Pixel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleColor Color { get; set; }
        public Pixel()
        {
        }
        public Pixel(int x, int y, ConsoleColor color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }

    public enum SnakeDirection
    {
        Right,
        Left,
        Up,
        Down
    }
}