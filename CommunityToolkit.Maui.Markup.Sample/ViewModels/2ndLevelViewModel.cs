namespace CommunityToolkit.Maui.Markup.Sample.ViewModels;

sealed partial class X2ndLevelViewModel : BaseViewModel
{


	[RelayCommand]
	static Task CloseButton()
	{
		var route = $"..";
		return Shell.Current.GoToAsync(route);
	}


}