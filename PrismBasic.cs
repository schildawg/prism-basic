

using System.Text;


class PrismBasic
{
    static void Main(string[] args)
    {
        var screen = new Screen();

        screen.PrintLn("PRISM BASIC");
        screen.PrintLn("(c) Copyright Deksai 2024");
        screen.PrintLn("Ok");
        screen.Build();

        screen.PrintFK(1, "LIST", 0);
        screen.PrintFK(2, "RUN ", 8);
        screen.PrintFK(3, "LOAD\"", 16);
        screen.PrintFK(4, "SAVE\"", 24);
        screen.PrintFK(5, "CONT ", 32);
        screen.PrintFK(6, ",\"LPT1", 40);
        screen.PrintFK(7, "TRON ", 48);
        screen.PrintFK(8, "TROFF ", 56);
        screen.PrintFK(9, "KEY ", 64);
        screen.PrintFK(0, "SCREEN", 72);

        screen.Locate(0, 3);
        screen.SetBackground(ConsoleColor.Black);
        screen.SetForeground(ConsoleColor.White);
        screen.Build();

        ConsoleKeyInfo cki;
        Console.SetCursorPosition(0, 3);

        do
            {
            cki = Console.ReadKey(true);
            switch (cki.Key)
                {
                case ConsoleKey.LeftArrow:
                    screen.MovePrevious();
                    break;
                case ConsoleKey.UpArrow:
                    screen.MoveUp();
                    break;
                case ConsoleKey.RightArrow:
                    screen.MoveNext();
                    break;

                case ConsoleKey.DownArrow:
                    screen.MoveDown();
                    break;


                case ConsoleKey.Backspace:
                    screen.Backspace();
                    break;

                case ConsoleKey.Enter:
                    screen.Enter();
                    break;

                default:    
                    screen.Display(cki.KeyChar);
                    break;
                }
            }
        while (cki.Key != ConsoleKey.Escape);  // end do-while

    }
}