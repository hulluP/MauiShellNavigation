namespace CommunityToolkit.Maui.Markup.Sample.Pages;

sealed class X2ndLevelPage : BaseContentPage<X2ndLevelViewModel>
{


	public X2ndLevelPage(X2ndLevelViewModel x2ndLevelViewModel) : base(x2ndLevelViewModel, "2nd Level")
	{

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


					new Image()
						.Source("dotnet_bot")
						.Size(250, 310)
						.CenterHorizontal(),

					new Button()
						.Text("close Me")
						.Font(bold: true)
						.CenterHorizontal()
						.Bind(Button.CommandProperty,
								static (X2ndLevelViewModel vm) => vm.CloseButtonCommand,
								mode: BindingMode.OneTime),
				}
			}
		};
	}
}