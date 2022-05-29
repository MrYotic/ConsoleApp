using static ConsoleHandler;
using static ConsoleApp;
using static FuckSharp;

public class Menu : IDisposable
{
    public Menu(params (string line, Action action)[] args)
    {
        items = args.Select((item, index) => new MenuItem(item.line, item.line, item.action) { Index = index, Options = MenuItem.MenuOptions.Hide }).ToList();
    }
    private List<MenuItem> items = new List<MenuItem>();
    public List<MenuItem> Items
    {
        get => items;
        set
        {
            items = value.Select((item, index) => item = new MenuItem(item.Name, item.Action) { Index = index, Options = MenuItem.MenuOptions.Hide }).ToList();
        }
    }
    public (int X, int Y) startPoint = (0, 0);
    private Color pressColor = Color.White;
    public Color PressColor 
    {
        get => pressColor;
        set
        {
            pressColor = value;
            Reload();
        }
    }
    private Color enterColor = Color.DarkGray;
    public Color EnterColor
    {
        get => enterColor;
        set
        {
            enterColor = value;
            Reload();
        }
    }
    private Color textColor = Color.Gray;
    public Color EextColor
    {
        get => textColor;
        set
        {
            textColor = value;
            Reload();
        }
    }
    private HorAlign hAlign = HorAlign.Center;
    public HorAlign HAlign
    {
        get => hAlign;
        set
        {
            hAlign = value;
            Reload();
        }
    }
    private VerAlign vAlign = VerAlign.Center;
    public VerAlign VAlign
    {
        get => vAlign;
        set
        {
            vAlign = value;
            Reload();
        }
    }
    public void Reload()
    {
        int cx = Console.WindowWidth / 2;
        int cy = Console.WindowHeight / 2;
        int w = Console.WindowWidth;
        int h = Console.WindowHeight;
        for(int i = 0; i < items.Count; i++)
        {
            switch (hAlign)
            {
                case HorAlign.Left:
                    items[i].X = startPoint.X;
                    break;
                case HorAlign.Center:
                    items[i].X = cx - items[i].Line.Length / 2;
                    break;
                case HorAlign.Right:
                    items[i].X = w - startPoint.X;
                    break;
            }
            switch (vAlign)
            {
                case VerAlign.Top:
                    items[i].Y = items[i].Index + startPoint.Y;
                    break;
                case VerAlign.Center:
                    if(i < items.Count / 2)
                        items[i].Y = cy - (Items.Count / 2 - items[i].Index);
                    else items[i].Y = cy + items[i].Index - Items.Count / 2;      
                    break;
                case VerAlign.Bottom:
                    items[i].Y = h - items[i].Index - startPoint.Y;
                    break;
            }
        }
    }
    public void Render() => items.ForEach(z =>
    {
        CCP = (z.X, z.Y);
        CFC = ConsoleColors[textColor];
        W = z.Line;
    });
    public void Close()
    {
        MouseUpdate -= Update;
        Console.Clear();
        Dispose();
    }
    public void Hide()
    {
        MouseUpdate -= Update;
        Console.Clear();
    }
    public void Show()
    {
        Reload();
        Render();
        MouseUpdate += Update;
    }
    private void RenderItem(int index, Color color)
    {
        if(index != -1)
        {
            MenuItem item = items[index];
            CCP = (item.X, item.Y);
            CFC = ConsoleColors[color];
            W = item.Line;
        }
    }
    private int lastEntered = -1;
    public void Update()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(Intersection(items[i].Y, items[i].X, items[i].X + items[i].Line.Length))
            {
                if(Mouse.State == 0)
                {
                    RenderItem(lastEntered, textColor);
                    lastEntered = i;
                    RenderItem(i, EnterColor);
                }
                else if(Mouse.State == 1)
                {
                    RenderItem(lastEntered, textColor);
                    lastEntered = i;
                    RenderItem(i, pressColor);                    
                    items[i].Invoke(this);
                }
                break;
            }
            RenderItem(lastEntered, textColor);
            lastEntered = -1;
        }
    }
    private bool Intersection(int y, int xs, int xe) => Mouse.Y == y && Mouse.X >= xs && Mouse.X <= xe;

    public void Dispose() { }

    public enum HorAlign
    {
        Left,
        Center,
        Right,
    }
    public enum VerAlign
    {
        Top,
        Center,
        Bottom,
    }
}
public class MenuItem
{
    public MenuItem(string name, string line, Action action, MenuOptions options = MenuOptions.None)
    {
        Name = name;
        Line = line;
        Action = action;
    }
    public MenuItem(string line, Action action, MenuOptions options = MenuOptions.None) : this(line, line, action, options) { }
    public int X = 0, Y = 0;
    public int Index { get; set; }
    public string Name { get; set; }
    public string Line { get; set; }
    public Action Action;
    public MenuOptions Options { get; set; }
    public void Invoke(Menu menu = null)
    {
        Action?.Invoke();
        if (Options == MenuOptions.Close)
            menu?.Close();
        else if (Options == MenuOptions.Hide)
            menu?.Hide();
    }
    public enum MenuOptions 
    {
        None,
        Hide,
        Close,
    }
}