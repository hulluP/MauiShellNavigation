namespace CommunityToolkit.Maui.Markup.Sample.Pages;

sealed class ModalPage : BaseContentPage<ModalViewModel>
{


    public ModalPage(ModalViewModel modalViewModel) : base(modalViewModel, "Modal Page")
    {
        Shell.SetPresentationMode(this, PresentationMode.Modal);
        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Spacing = 25,
                Padding = 30,

                Children =
                {
                    new Label()
                        .Text("Hello World")
                        .Font(size: 32)
                        .CenterHorizontal(),

                    new Label()
                        .Text("Welcome to .NET MAUI Markup Community Toolkit Sample")
                        .Font(size: 18)
                        .CenterHorizontal(),

                    new Label()
                        .Font(size: 18, bold: true)
                        .CenterHorizontal()
                        .Bind(Label.TextProperty,
                                static (ModalViewModel vm) => vm.ClickCount,
                                convert: count => $"Current Count: {count}"),

                    new Button()
                        .Text("Click Me")
                        .Font(bold: true)
                        .CenterHorizontal()
                        .Bind(Button.CommandProperty,
                                static (ModalViewModel vm) => vm.IncrementClickMeButtonCommand,
                                mode: BindingMode.OneTime),

                    new Image()
                        .Source("dotnet_bot")
                        .Size(250, 310)
                        .CenterHorizontal(),

                    new Button()
                        .Text("close Me")
                        .Font(bold: true)
                        .CenterHorizontal()
                        .Bind(Button.CommandProperty,
                                static (ModalViewModel vm) => vm.CloseButtonCommand,
                                mode: BindingMode.OneTime),
                }
            }
        };
    }
}