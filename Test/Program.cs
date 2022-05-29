using static FuckSharp;
Menu login = new Menu()
{
    startPoint = (2, 1),
    HAlign = Menu.HorAlign.Left,
    VAlign = Menu.VerAlign.Top,
};
login.Add(new List<BaseMenuItem>()
{
    new LabelItem("Discord File Hosting Panel."),
    new LabelItem(""),
    new LabelItem("Login User."),
    new LabelItem(""),    
    new InputItem("Login:"),
    new InputItem("Password:")
}.ToArray());

Menu menu = new Menu()
{
    startPoint = (2, 1),
    HAlign = Menu.HorAlign.Left,
    VAlign = Menu.VerAlign.Top,
};
menu.Add(new List<BaseMenuItem>()
{
    new LabelItem("Discord File Hosting Panel."),
    new LabelItem(""),
    new LabelItem(""),
    new ButtonItem("Login", new(() => { login.Show(); })),
    new ButtonItem("Register", new(() => { })),

    new ButtonItem("User Exist", new(() => { })),
    new ButtonItem("User Is Valid", new(() => { })),

    new ButtonListItem("Find Files",
        ("Private", () => { }),
        ("Public", () => { }),
        ("Anon", () => { })),

    new ButtonListItem("Upload Document",
        ("Private", () => { }),
        ("Public", () => { }),
        ("Anon", () => { })),

    new ButtonListItem("Upload File",
        ("Private", () => { }),
        ("Public", () => { }),
        ("Anon", () => { })),

    new ButtonItem("Download File", new(() => { })),
    new ButtonItem("Get New Anon Files", new(() => { })),
    
}.ToArray());
menu.Show();