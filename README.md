# Sieve of Eratosthenes

An animated console visualization of the Sieve of Eratosthenes, the classic algorithm for finding every prime up to a given limit.

## How the sieve works

1. Write down every number from 2 up to the limit.
2. Take the smallest number that hasn't been crossed out. It's prime.
3. Cross out all of its multiples, starting at its square. Smaller multiples were already crossed out by smaller primes.
4. Repeat until the next prime's square is bigger than the limit. Every number still standing is prime.

## What you'll see

The numbers are laid out in rows of 10, so each prime's multiples line up in visible column patterns.

| Color | Meaning |
|---|---|
| White | Not yet decided |
| **Green** | Prime |
| **Red** | Being crossed out right now |
| Dark gray | Composite (crossed out) |

A yellow status line narrates each step, for example *"3 is prime — crossing out its multiples starting at 3² = 9"*. When the sieve finishes, the program prints the full list of primes with a count.

## Requirements

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- A real console window (the animation positions the cursor, so it won't work with redirected output)

## Running

From this folder:

```bash
dotnet run
```

Optional arguments:

```bash
dotnet run -- [limit] [delayMs]
```

| Argument | Default | Description |
|---|---|---|
| `limit` | 120 | Largest number to sieve (minimum 2) |
| `delayMs` | 40 | Pause in milliseconds between crossing out numbers. Use 0 for no animation delay. |

Example: sieve up to 300 at a faster pace:

```bash
dotnet run -- 300 20
```

## Notes

- The grid needs about `limit / 10` rows plus a few more for the header and status line. If the grid is taller than your console window, the program throws an error, so make the window taller or use a smaller limit.
- On narrow windows the grid uses fewer than 10 columns so it still fits.
