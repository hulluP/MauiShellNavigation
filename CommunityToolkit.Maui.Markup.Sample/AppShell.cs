namespace CommunityToolkit.Maui.Markup.Sample;

class AppShell : Shell
{


	public AppShell(SettingsPage settingsPage, NewsPage newsPage)
	{
        Items.Add(new ShellContent() { Content = newsPage });
        Items.Add(new ShellContent() { Content = settingsPage });
		// Items.Add(settingsPage);
		// Items.Add(newsPage);
	}

}