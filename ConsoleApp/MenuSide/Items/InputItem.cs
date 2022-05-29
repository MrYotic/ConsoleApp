using static ConsoleApp;
using static MenuItem;

public class InputItem : BaseMenuItem
{
    public InputItem(string line, string value = "", MenuOptions options = MenuOptions.Hide) : base(line, options)
    {
        Line = line;
        Options = options;
        Value = value;
    }
    public string Value { get; set; }
    public int Pos { get; set; }
    public override int Length => 10000; // аахаах, а хитро ты это наговнокодил, я даже сначала и не понял)
    public override void Render(Color color)
    {
        CCP = (X, Y);
        CFC = ConsoleColors[color];
        W = Line;
    }
}
