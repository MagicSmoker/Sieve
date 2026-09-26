// Sieve of Eratosthenes — animated console visualization.
// Usage: dotnet run [limit] [delayMs]
//   limit   : largest number to sieve (default 120)
//   delayMs : pause between crossing out numbers (default 40)

using System.Text;

int limit = args.Length > 0 && int.TryParse(args[0], out var l) && l >= 2 ? l : 120;
int delay = args.Length > 1 && int.TryParse(args[1], out var d) && d >= 0 ? d : 40;

Console.OutputEncoding = Encoding.UTF8;
Console.CursorVisible = false;

var state = new CellState[limit + 1];
for (int i = 2; i <= limit; i++) state[i] = CellState.Unknown;

int cellWidth = limit.ToString().Length + 1;
// Rows of 10 make the multiples' patterns easy to see; shrink if the window is narrow.
int columns = Math.Max(1, Math.Min(10, (Console.WindowWidth - 1) / cellWidth));
int gridTop = 2;
int gridRows = (limit - 1 + columns - 1) / columns;
int statusTop = gridTop + gridRows + 1;

Console.Clear();
WriteAt(0, 0, $"Sieve of Eratosthenes: primes up to {limit}", ConsoleColor.White);
for (int n = 2; n <= limit; n++) DrawCell(n);

try
{
    for (int p = 2; p * p <= limit; p++)
    {
        if (state[p] == CellState.Composite) continue;

        // p survived every earlier pass, so it's prime.
        state[p] = CellState.Prime;
        DrawCell(p);
        Status($"{p} is prime — crossing out its multiples starting at {p}² = {p * p}");
        Thread.Sleep(delay * 10);

        for (int m = p * p; m <= limit; m += p)
        {
            if (state[m] == CellState.Composite) continue; // already removed by a smaller prime
            state[m] = CellState.Current;
            DrawCell(m);
            Thread.Sleep(delay);
            state[m] = CellState.Composite;
            DrawCell(m);
        }
    }

    // Everything left unmarked past sqrt(limit) is prime.
    Status($"No primes ≤ √{limit} left — every remaining number is prime");
    Thread.Sleep(delay * 10);
    for (int n = 2; n <= limit; n++)
    {
        if (state[n] != CellState.Unknown) continue;
        state[n] = CellState.Prime;
        DrawCell(n);
        Thread.Sleep(delay / 2);
    }

    var primes = Enumerable.Range(2, limit - 1).Where(n => state[n] == CellState.Prime).ToList();
    Status($"Found {primes.Count} primes:");
    Console.SetCursorPosition(0, statusTop + 1);
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(string.Join(", ", primes));
}
finally
{
    Console.ResetColor();
    Console.CursorVisible = true;
}

void DrawCell(int n)
{
    int idx = n - 2;
    int col = idx % columns, row = idx / columns;
    var (fg, bg) = state[n] switch
    {
        CellState.Prime => (ConsoleColor.Black, ConsoleColor.Green),
        CellState.Composite => (ConsoleColor.DarkGray, ConsoleColor.Black),
        CellState.Current => (ConsoleColor.Black, ConsoleColor.Red),
        _ => (ConsoleColor.White, ConsoleColor.Black),
    };
    Console.SetCursorPosition(col * cellWidth, gridTop + row);
    Console.ForegroundColor = fg;
    Console.BackgroundColor = bg;
    Console.Write(n.ToString().PadLeft(cellWidth - 1));
    Console.ResetColor();
}

void Status(string text)
{
    Console.SetCursorPosition(0, statusTop);
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write(text.PadRight(Math.Max(text.Length, Console.WindowWidth - 1)));
    Console.ResetColor();
}

void WriteAt(int x, int y, string text, ConsoleColor color)
{
    Console.SetCursorPosition(x, y);
    Console.ForegroundColor = color;
    Console.Write(text);
    Console.ResetColor();
}

enum CellState { Unknown, Prime, Composite, Current }
