using static ConsoleHandler;
using static ConsoleApp;
using static FuckSharp;

public class Menu : IDisposable
{
    public Menu() {}
    private List<BaseMenuItem> items = new List<BaseMenuItem>();
    public void Add(params BaseMenuItem[] args)
    {
        items.AddRange(args);
        int index = 0;
        items.ForEach(z => z.Index = index++);
    }
    public (int X, int Y) startPoint = (0, 0);
    private Color textColor = Color.Gray;
    public Color TextColor
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
                        items[i].Y = cy - (items.Count / 2 - items[i].Index);
                    else items[i].Y = cy + items[i].Index - items.Count / 2;      
                    break;
                case VerAlign.Bottom:
                    items[i].Y = h - items[i].Index - startPoint.Y;
                    break;
            }
        }
    }
    public void Render() => items.ForEach(z => z.Render(TextColor));
    public void Close()
    {
        MouseUpdate -= Mouse_Update;
        PressKey -= KeyPress_Update;
        Console.Clear();
        Dispose();
    }
    public void Hide()
    {
        MouseUpdate -= Mouse_Update;
        PressKey -= KeyPress_Update;
        Console.Clear();
    }
    public void Show()
    {
        Reload();
        Render();
        MouseUpdate += Mouse_Update;
        PressKey += KeyPress_Update;
        items.Where(z => z is InputItem).ToList().ForEach(z =>
        {
            CCP = (startPoint.X + z.Line.Length, z.Index + 1);
            ((InputItem)items[z.Index]).Value = Console.ReadLine();
        });
    }
    private int lastEntered = -1, inputIndex = -1;
    public void KeyPress_Update(bool down, char @char, ushort key, int state)
    {
        return;
        if (inputIndex != -1)
        {
            CCP = (0, 20);
            W = down + "|" + @char + "|" + key + "|" + state + "             ";
            if (!new int[]{ 60, 70, 112, 96 }.Contains(state))
                return;
            InputItem item = (InputItem)items[inputIndex];
            if (!down && key == 8)
            {
                if(item.Value.Length > 0)
                {
                    ((InputItem)items[inputIndex]).Value = item.Value.Remove(item.Value.Length - 1);
                    CCP = (startPoint.X + item.Value.Length + items[inputIndex].Line.Length, inputIndex + 1);
                    W = default(char);
                    CCP = (startPoint.X + item.Value.Length + items[inputIndex].Line.Length, inputIndex + 1);
                }
            }
            else if(((down && state == 112) || !down) && (char.IsLetter(@char) || char.IsDigit(@char) || @char == 32))
            {
                CCP = (startPoint.X + item.Value.Length + items[inputIndex].Line.Length, inputIndex + 1);
                W = @char;
                ((InputItem)items[inputIndex]).Value += @char;
            }          
        }
    }
    public void Mouse_Update()
    {
        for (int i = 0; i < items.Count; i++)
        {
            if(Intersection(items[i].Y, items[i].X, items[i].X + items[i].Length))
            {
                if (lastEntered != -1 && !MenuItem.UnUpdaterTypes.Contains(items[i].GetType())) items[lastEntered].Render(textColor);
                lastEntered = i;
                if (items[i] is ButtonItem)
                {
                    if (Mouse.State == 0)
                    {
                        items[i].Render(MenuItem.EnterColor);
                    }
                    else if (Mouse.State == 1)
                    {
                        items[i].Render(MenuItem.PressColor);
                        ((ButtonItem)items[i]).Invoke(this);
                    }
                    break;
                }
                else if(items[i] is ButtonListItem)
                {
                    int index = -1;
                    ButtonListItem item = (ButtonListItem)items[i];
                    for (int o = 0; o < item.Units.Count; o++)
                    {
                        int start = item.Line.Length + 2 + Enumerable.Range(0, o).Select(z => item.Units[z].Name.Length + 1).Sum();
                        if (Intersection(start, start + item.Units[o].Name.Length))
                        {
                            index = o;
                            break;
                        }
                    }
                    if(index != -1)
                    {
                        if (Mouse.State == 0)
                        {
                            item.Render(MenuItem.EnterColor, index);
                        }
                        else if (Mouse.State == 1)
                        {
                            item.Render(MenuItem.PressColor, index);
                            item.Invoke(index, this);
                        }
                    }
                    break;
                }
                else if(items[i] is InputItem)
                {
                    if (Mouse.State == 1)
                    {
                        inputIndex = i;
                        CCP = (startPoint.X + ((InputItem)items[i]).Value.Length + items[inputIndex].Line.Length, inputIndex + 1);
                    }
                    break;
                }
                if (lastEntered != -1)
                {
                    if (!MenuItem.UnUpdaterTypes.Contains(items[lastEntered].GetType()))
                        items[lastEntered].Render(textColor);
                    lastEntered = -1;
                }
            }
        }
    }
    private bool Intersection(int y, int xs, int xe) => Mouse.Y == y && Mouse.X >= xs && Mouse.X <= xe;
    private bool Intersection(int xs, int xe) => Mouse.X >= xs && Mouse.X <= xe;

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
    public static Color PressColor = Color.White;
    public static Color EnterColor = Color.DarkGray;
    public static List<Type> UnUpdaterTypes = new List<Type>() {
        typeof(LabelItem), typeof(InputItem)
    };
    public enum MenuOptions 
    {
        None,
        Hide,
        Close,
    }
}