namespace CommunityToolkit.Maui.Markup.Sample.ViewModels;

sealed partial class ModalViewModel : BaseViewModel
{
	[ObservableProperty]
	int clickCount = 0;

	[RelayCommand]
	public void IncrementClickMeButton() => ClickCount++;

	[RelayCommand]
	static Task CloseButton()
	{
		var route = $"..";
		return Shell.Current.GoToAsync(route);
	}


}