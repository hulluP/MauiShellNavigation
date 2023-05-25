namespace CommunityToolkit.Maui.Markup.Sample.ViewModels;

sealed partial class NewsViewModel : BaseViewModel, IDisposable
{
    readonly IDispatcher dispatcher;
    readonly SettingsService settingsService;
    readonly HackerNewsAPIService hackerNewsAPIService;
    readonly WeakEventManager pullToRefreshEventManager = new();
    readonly SemaphoreSlim insertIntoSortedCollectionSemaphore = new(1, 1);

    [ObservableProperty]
    bool isListRefreshing;

    public NewsViewModel(IDispatcher dispatcher,
                            SettingsService settingsService,
                            HackerNewsAPIService hackerNewsAPIService)
    {
        this.dispatcher = dispatcher;
        this.settingsService = settingsService;
        this.hackerNewsAPIService = hackerNewsAPIService;
    }

    public event EventHandler<string> PullToRefreshFailed
    {
        add => pullToRefreshEventManager.AddEventHandler(value);
        remove => pullToRefreshEventManager.RemoveEventHandler(value);
    }

    public ObservableCollection<StoryModel> TopStoryCollection { get; } = new();

    public void Dispose()
    {
        insertIntoSortedCollectionSemaphore.Dispose();
    }

    public StoryModel SelectedItem
    {
        get
        {
            return null;
        }
        set
        {
            if (value != null)
            {
                var storyModel = value;
                if (!string.IsNullOrEmpty(storyModel.Url))
                {
                    NavigateToNewsDetailPage(storyModel);
                }
                else
                {
                    //DisplayAlert("Invalid Article", "ASK HN articles have no url", "OK");
                }
            }
        }
    }
    [RelayCommand]
    async Task PullToRefresh()
    {
        TopStoryCollection.Clear();

        try
        {
            await foreach (var story in GetTopStories(settingsService.NumberOfTopStoriesToFetch).ConfigureAwait(false))
            {
                await InsertIntoSortedCollection((a, b) => b.Score.CompareTo(a.Score), story).ConfigureAwait(false);
            }
        }
        catch (Exception e)
        {
            OnPullToRefreshFailed(e.ToString());
        }
        finally
        {
            IsListRefreshing = false;
        }
    }

    Task NavigateToNewsDetailPage(StoryModel storyModel) => dispatcher.DispatchAsync(() =>
    {
        var route = $"//{nameof(NewsPage)}/{nameof(NewsDetailViewModel)}";

        // Shell passes these parameters to NewsDetailViewModel.ApplyQueryAttributes
        var parameters = new Dictionary<string, object>
        {
            { nameof(NewsDetailViewModel.Uri), storyModel.Url },
            { nameof(NewsDetailViewModel.Title), storyModel.Title },
            { nameof(NewsDetailViewModel.ScoreDescription), storyModel.Description}
        };

        return Shell.Current.GoToAsync(route, parameters);
    });

    async IAsyncEnumerable<StoryModel> GetTopStories(int storyCount)
    {
        var topStoryIds = await hackerNewsAPIService.GetTopStoryIDs().ConfigureAwait(false);
        var getTopStoryTaskList = topStoryIds.Select(hackerNewsAPIService.GetStory).ToList();

        while (getTopStoryTaskList.Any() && storyCount-- > 0)
        {
            var completedGetStoryTask = await Task.WhenAny(getTopStoryTaskList).ConfigureAwait(false);
            getTopStoryTaskList.Remove(completedGetStoryTask);

            var story = await completedGetStoryTask.ConfigureAwait(false);
            yield return story;
        }
    }

    async Task InsertIntoSortedCollection(Comparison<StoryModel> comparison, StoryModel modelToInsert)
    {
        await insertIntoSortedCollectionSemaphore.WaitAsync().ConfigureAwait(false);

        try
        {
            if (TopStoryCollection.Count is 0)
            {
                await dispatcher.DispatchAsync(() => TopStoryCollection.Add(modelToInsert)).ConfigureAwait(false);
            }
            else if (!TopStoryCollection.Any(x => x.Title == modelToInsert.Title))
            {
                int index = 0;
                foreach (var model in TopStoryCollection)
                {
                    if (comparison(model, modelToInsert) >= 0)
                    {
                        await dispatcher.DispatchAsync(() => TopStoryCollection.Insert(index, modelToInsert)).ConfigureAwait(false);
                        return;
                    }

                    index++;
                }

                await dispatcher.DispatchAsync(() => TopStoryCollection.Insert(index, modelToInsert)).ConfigureAwait(false);
            }
        }
        finally
        {
            insertIntoSortedCollectionSemaphore.Release();
        }
    }

    void OnPullToRefreshFailed(string message) => pullToRefreshEventManager.HandleEvent(this, message, nameof(PullToRefreshFailed));
}