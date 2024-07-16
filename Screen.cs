using System.Text;

class Screen
{
    private int cursorX = 0;
    private int cursorY = 0;

    private ConsoleColor currentForeground = ConsoleColor.White;
    private ConsoleColor currentBackground = ConsoleColor.Black;

    private const int NUMBER_OF_COLUMNS = 80;
    private const int NUMBER_OF_ROWS = 20;

    private const int LAST_ROW = NUMBER_OF_ROWS - 1;
    private const int LAST_COLUMN = NUMBER_OF_COLUMNS - 1;

    private readonly char[,] buffer;
    private readonly ConsoleColor[,] foreground;
    private readonly ConsoleColor[,] background;

    public Screen() {
        buffer = new char[81, 20];
        foreground = new ConsoleColor[81, 20];
        background = new ConsoleColor[81, 20];

        currentForeground = ConsoleColor.White;
        currentBackground = ConsoleColor.Black;
        ClearScreen();
    }

    public void SetForeground(ConsoleColor color) {
        currentForeground = color;
    }

    public void SetBackground(ConsoleColor color) {
        currentBackground = color;
    }

    public void Locate(int x, int y) {
        cursorX = x;
        cursorY = y;
    }

    public void Print(string text)
    {
        for (int i = 0; i < cursorX; i++)
        {
            if (buffer[i, cursorY] == (char) 0)
            {
                buffer[i, cursorY] = ' ';
                foreground[i, cursorY] = currentForeground;
                background[i, cursorY] = currentBackground;
            }
        }

        foreach (char c in text.ToCharArray())
        {
            buffer[cursorX, cursorY] = c;
            foreground[cursorX, cursorY] = currentForeground;
            background[cursorX, cursorY] = currentBackground;
            cursorX++;

            if (cursorX > LAST_COLUMN)
            {
                cursorY++;
                cursorX = 0;
            }
        }
    }

    public void PrintLn(string text)
    {
        Print(text);

        // Move to next line
        cursorY++;
        cursorX = 0;
    }


    public void MoveUp()
    {
        cursorY--;

        if (cursorY < 0) cursorY = 0;

        UpdateCursor();
    }

    public void SetChar(char c) {
        buffer[cursorX, cursorY] = c;
    }

    public void MoveDown()
    {
        cursorY++;

        if (cursorY > LAST_ROW - 1) cursorY = LAST_ROW - 1;

        UpdateCursor();
    }

    public void MoveNext()
    {
        if (cursorY == LAST_ROW - 1 && cursorX == LAST_COLUMN)
        {
            return;
        }
        cursorX++;
        if (cursorX >= NUMBER_OF_COLUMNS)
        {
            cursorX = 0;
            cursorY++;
        }
        UpdateCursor();
    }

    public void MovePrevious()
    {
        if (cursorY == 0 && cursorX == 0)
        {
            return;
        }

        cursorX--;
        if (cursorX < 0)
        {
            cursorX = LAST_COLUMN;
            cursorY--;
        }

        UpdateCursor();
    }


    public void Build()
    {
        Console.SetCursorPosition(0,0);
        var sb = new StringBuilder();

        var lastForeground = currentForeground;
        var lastBackground = currentBackground;

        Console.ResetColor();
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                var currentForeground = foreground[j, i];
                var currentBackground = background[j, i];

                if (currentForeground != lastForeground || currentBackground != lastBackground)
                {
                        Console.Write(sb.ToString());
                        sb = new StringBuilder();

                        Console.ForegroundColor = currentForeground;
                        Console.BackgroundColor = currentBackground;

                        lastForeground = currentForeground;
                        lastBackground = currentBackground;
                }

                if (buffer[j, i] == (char) 0)
                {
                    sb.Append(' ');
                }
                else if (buffer[j, i] == '\r')
                {
                    // skip
                }
                else
                    sb.Append(buffer[j, i]);
            }
            Console.Write(sb.ToString());
            Console.SetCursorPosition(0, i + 1);
            sb = new StringBuilder();
        }

        Console.Write(sb.ToString());
        UpdateCursor();
    }


    public void Display(char c)
    {
        if (!char.IsDigit(c) && !char.IsLetter(c)) {
            return;
        }

        if (cursorX > LAST_COLUMN)
        {
            buffer[cursorX, cursorY] = '\r';
            foreground[cursorX, cursorY] = currentForeground;
            background[cursorX, cursorY] = currentBackground;

            if (cursorY < LAST_ROW - 2)
            {
                for (int y = LAST_ROW - 1; y > cursorY + 1; y--)
                {
                    for (int x = 0; x <= NUMBER_OF_COLUMNS; x++)
                    {
                        buffer[x, y] = buffer[x, y - 1];
                        foreground[x, y] = foreground[x, y - 1];
                        background[x, y] = background[x, y - 1];
                    }
                }
            }

            if (cursorY != LAST_ROW - 1)
            {
                for (int x = 0; x <= NUMBER_OF_COLUMNS; x++)
                {
                    buffer[x, cursorY + 1] = (char)0;
                    foreground[x, cursorY + 1] = 0;
                    background[x, cursorY + 1] = 0;
                }
                cursorY++;
                cursorX = 0;
            }
            else
            {
                cursorX = 0;
                for (int y = 0; y < NUMBER_OF_ROWS - 2; y++)
                {
                    for (int x = 0; x <= NUMBER_OF_COLUMNS; x++)
                    {
                        buffer[x, y] = buffer[x, y + 1];
                        foreground[x, y] = foreground[x, y + 1];
                        background[x, y] = background[x, y + 1];
                    }
                }
                for (int x = 0; x <= NUMBER_OF_COLUMNS; x++)
                {
                    buffer[x, LAST_ROW - 1] = (char)0;
                    foreground[x, LAST_ROW - 1] = 0;
                    background[x, LAST_ROW - 1] = 0;
                }
            }
        }

        for (int i = 0; i < cursorX; i++)
        {
            if (buffer[i, cursorY] == (char)0)
            {
                buffer[i, cursorY] = ' ';
                foreground[i, cursorY] = 0;
                background[i, cursorY] = 0;
            }
        }

        buffer[cursorX, cursorY] = c;
        foreground[cursorX, cursorY] = currentForeground;
        background[cursorX, cursorY] = currentBackground;

        cursorX++;

        Build();
    }

    public void Backspace()
    {
        if (cursorX > 0)
        {
            cursorX--;
        }
        else if (cursorY != 0)
        {
            bool hasLF = false;
            for (int i = 0; i <= NUMBER_OF_COLUMNS; i++)
            {
                if (buffer[i, cursorY - 1] == '\r')
                {
                    hasLF = true;
                    break;
                }
            }

            if (hasLF)
            {
                cursorX = LAST_COLUMN;
                cursorY--;
            }
        }
        Delete();
    }


    public void Delete()
    {
        int currentX = cursorX;
        int currentY = cursorY;

        char nextChar = GetNext();
        var nextForeground  = GetNextForeground();
        var nextBackground = GetNextBackground();

        while (nextChar != (char) 0)
        {
            buffer[cursorX, cursorY] = nextChar;
            foreground[cursorX, cursorY] = nextForeground;
            background[cursorX, cursorY] = nextBackground;

            MoveNext();
            nextChar = GetNext();
            nextForeground = GetNextForeground();
            nextBackground = GetNextBackground();
        }
        buffer[cursorX, cursorY] = (char) 0;
        foreground[cursorX, cursorY] = 0;
        background[cursorX, cursorY] = 0;

        // Eliminate LF indicating continuing logical line
        buffer[NUMBER_OF_COLUMNS, cursorY] = (char) 0;
        foreground[NUMBER_OF_COLUMNS, cursorY] = 0;
        background[NUMBER_OF_COLUMNS, cursorY] = 0;

        cursorX = currentX;
        cursorY = currentY;

        Build();
        UpdateCursor();
    }

    private char GetNext()
    {
        if (cursorY == LAST_ROW - 1 && cursorX == LAST_COLUMN)
        {
            return (char) 0;
        }

        int nextX = cursorX + 1;
        int nextY = cursorY;
        if (nextX == 80)
        {
            nextX = 0;
            nextY++;
        }

        return buffer[nextX, nextY];
    }

    private ConsoleColor GetNextForeground()
    {
        if (cursorY == LAST_ROW - 1 && cursorX == LAST_COLUMN)
        {
            return 0;
        }

        int nextX = cursorX + 1;
        int nextY = cursorY;
        if (nextX == 80)
        {
            nextX = 0;
            nextY++;
        }

        return foreground[nextX, nextY];
    }


    private ConsoleColor GetNextBackground()
    {
        if (cursorY == LAST_ROW - 1 && cursorX == LAST_COLUMN)
        {
            return 0;
        }

        int nextX = cursorX + 1;
        int nextY = cursorY;
        if (nextX == 80)
        {
            nextX = 0;
            nextY++;
        }

        return background[nextX, nextY];
    }

    public void Home(bool ctrlMode)
    {
        if (ctrlMode)
        {
            ClearScreen();
            Build();
        }
        cursorX = 0;
        cursorY = 0;
        UpdateCursor();
    }

    public void UpdateCursor()
    {
        int x = cursorX;
        if (x > LAST_COLUMN)
        {
            x = LAST_COLUMN;
        }
        Console.SetCursorPosition(x, cursorY);
     }

    public void Enter()
    {
        if (cursorY < LAST_ROW - 1)
        {
            cursorY++;
            cursorX = 0;

        }
        else
        {
            cursorX = 0;
            for (int y = 0; y < NUMBER_OF_ROWS - 2; y++)
            {
                for (int x = 0; x <= NUMBER_OF_COLUMNS; x++)
                {
                    buffer[x, y] = buffer[x, y + 1];
                    foreground[x, y] = foreground[x, y + 1];
                    background[x, y] = background[x, y + 1];
                }
            }
            for (int x = 0; x <= NUMBER_OF_COLUMNS; x++)
            {
                buffer[x, LAST_ROW - 1] = (char) 0;
                foreground[x, LAST_ROW - 1] = 0;
                background[x, LAST_ROW - 1] = 0;
            }

            Build();
        }
        UpdateCursor();

        Console.Clear();
        Build();
    }

    public void ClearScreen()
    {
        for (int i = 0; i < 19; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                buffer[j, i] = (char) 0;
                foreground[j, i] = currentForeground;
                background[j, i] = currentBackground;
            }
        }
        Console.Clear();
    }

        public void PrintFK(int number, string text, int x)
        {
            cursorY = 19;
            cursorX = x;

            currentForeground = ConsoleColor.White;
            currentBackground = ConsoleColor.Black;
            Print(number.ToString());

            currentForeground = ConsoleColor.Black;
            currentBackground = ConsoleColor.White;
            Print(text);
        }
   }
