namespace CommunityToolkit.Maui.Markup.Sample.ViewModels;

sealed partial class SettingsViewModel : BaseViewModel
{
	readonly SettingsService settingsService;

	[ObservableProperty]
	int numberOfTopStoriesToFetch;

	public SettingsViewModel(SettingsService settingsService)
	{
		this.settingsService = settingsService;
		NumberOfTopStoriesToFetch = settingsService.NumberOfTopStoriesToFetch;
	}

	partial void OnNumberOfTopStoriesToFetchChanged(int value)
	{
		settingsService.NumberOfTopStoriesToFetch = value;
	}

	[RelayCommand]
	static async Task MainButton()
	{
		await Shell.Current.GoToAsync($"//" + MauiProgram.ROOT);
	}

}