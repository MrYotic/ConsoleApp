using static ConsoleApp;
using static MenuItem;

public class ButtonItem : BaseMenuItem
{
    public ButtonItem(string line, Action action, MenuOptions options = MenuOptions.Hide) : base(line, options)
    {
        Line = line;
        Action = action;
        Options = options;
    }
    public Action Action;
    public MenuOptions Options { get; set; }

    public override int Length => Line.Length;

    public void Invoke(Menu menu = null)
    {
        if (Options == MenuOptions.Hide)
            menu?.Hide();
        Action?.Invoke();
        if (Options == MenuOptions.Close)
            menu?.Close();
    }
    
    public override void Render(Color color)
    {
        CCP = (X, Y);
        CFC = ConsoleColors[color];
        W = Line;
    }
}
