using static ConsoleApp;
using static MenuItem;

public class InputItem : BaseMenuItem
{
    public InputItem(string line, string value = "", int x = 0, int y = 0) : base(line, x, y)
    {
        Line = line;
        Value = value;
    }
    public Func<string, string?> ValidAction { get; set; }
    public string Value { get; set; }
    public int Pos { get; set; }
    public override int Length => 1000; // аахаах, а хитро ты это наговнокодил, я даже сначала и не понял)
    public string? Validate(string value) => ValidAction?.Invoke(value);
    public override void Render(Color color)
    {
        CCP = (X, Y);
        CFC = ConsoleColors[color];
        W = Line;
    }
}