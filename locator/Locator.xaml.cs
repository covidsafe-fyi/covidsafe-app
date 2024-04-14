using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json;
using System.Web;

namespace locator
{
    [QueryProperty(nameof(SearchName), "name")]
    [QueryProperty(nameof(FullName), "fullName")]
    public partial class Locator : ContentPage
    {
        public Locator()
        {
            InitializeComponent();
            this.Loaded += Locator_Loaded;
            BindingContext = this;
            Providers = EmptyProviders;
        }

        public string SearchName { get; set; }
        public string FullName { get; set; }

        private async void Locator_Loaded(object? sender, EventArgs e)
        {
            if (SearchName != null)
            {
                Title = FullName + " Locator";
            }

            double zoomLevel = 8;
            double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
            Location? location = await Geolocation.Default.GetLocationAsync();
            if (location == null)
            {
                location = new Location(47.60621000, -122.33207000); // seattle
            }

            map.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == "VisibleRegion")
                {
                    if (SearchAlive)
                    {
                        map.Pins.Clear();
                        centerCircle.IsVisible = false;
                        var lastSelectedProvider = list.SelectedItem as Provider;
                        CircleSelectedProvider = lastSelectedProvider == null;
                        Provider? newSelectedProvider = null;
                        if (map.VisibleRegion != null)
                        {
                            var location = map.VisibleRegion.Center;
                            var meters = map.VisibleRegion.Radius.Meters;
                            var url = "https://healthdata.gov/resource/xkzp-zhs7.json?"
                                + $"$where=within_circle(geopoint,{location.Latitude},{location.Longitude},{meters})"
                                + $"&has_{SearchName}=true";
                            var stream = await client.GetStreamAsync(url);
                            var fetchedProviders = await JsonSerializer.DeserializeAsync<IList<Provider>>(stream);
                            if (fetchedProviders != null)
                            {
                                emptyLabel.Text = "";
                                if (fetchedProviders.Count == 0)
                                {
                                    list.ItemsSource = fetchedProviders;
                                    emptyLabel.Text = "zoom out or move map location to find providers.";
                                }
                                else if (fetchedProviders.Count < 101)
                                {
                                    fetchedProviders = fetchedProviders.OrderBy(p => p.city).ThenBy(p => p.provider_name).ThenBy(p=>p.address1).ToList();
                                    newSelectedProvider = null;
                                    int id = 1;
                                    foreach (var provider in fetchedProviders)
                                    {
                                        provider.provider_id = id++;
                                        var pin = new ProviderPin()
                                        {
                                            Provider = provider,
                                            Location = new Location(provider.geopoint.coordinates[1], provider.geopoint.coordinates[0]),
                                            Label = provider.provider_id.ToString(),
                                            Address = provider.provider_name + ", " + provider.address1 + ", " + provider.city + ", " + provider.public_phone,
                                        };

                                        if (newSelectedProvider == null && provider.provider_name == lastSelectedProvider?.provider_name
                                            && provider.address1 == lastSelectedProvider.address1
                                            && provider.city == lastSelectedProvider.city)
                                        {
                                            newSelectedProvider = provider;
                                        }

                                        pin.MarkerClicked += Pin_MarkerClicked;
                                        map.Pins.Add(pin);
                                    }

                                    list.ItemsSource = fetchedProviders;
                                    list.SelectedItem = newSelectedProvider;
                                }
                                else
                                {
                                    list.ItemsSource = EmptyProviders;
                                    emptyLabel.Text = $"this map region has too many providers to display. zoom in or move map location until there are 100 or less providers.";
                                }

                                Providers = fetchedProviders;
                            }
                        }
                    }
                    else
                    {
                        SearchAlive = true;
                    }
                }
            };

            if (map.VisibleRegion != null)
            {
                map.MoveToRegion(new MapSpan(location, latlongDegrees, latlongDegrees));
            }
        }

        private void Pin_MarkerClicked(object? sender, Microsoft.Maui.Controls.Maps.PinClickedEventArgs e)
        {
            var pin = sender as ProviderPin;
            CircleSelectedProvider = false;
            list.SelectedItem = pin?.Provider;
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.CurrentSelection)
            {
                var provider = item as Provider;
                if (provider != null && map?.VisibleRegion != null)
                {
                    list.ScrollTo(provider);
                    var visibleLatitudeDegrees = map.VisibleRegion.LatitudeDegrees;
                    var visibleLongitudeDegrees = map.VisibleRegion.LongitudeDegrees;
                    SearchAlive = false;
                    centerCircle.IsVisible = CircleSelectedProvider;
                    if (CircleSelectedProvider)
                    {
                        map.MoveToRegion(new MapSpan(new Location(provider.geopoint.coordinates[1], provider.geopoint.coordinates[0]), visibleLatitudeDegrees, visibleLongitudeDegrees));
                    }
                }
                else
                {
                    centerCircle.IsVisible = false;
                }
            }
        }

        public IList<Provider>? Providers
        {
            get { return providers; }
            set
            {
                providers = value;
                OnPropertyChanged();
            }
        }
        static IList<Provider> EmptyProviders = new List<Provider>();
        HttpClient client = new HttpClient();
        private bool SearchAlive = true;
        private bool CircleNext = false;
        private bool CircleSelectedProvider = true;
        private IList<Provider>? providers;

        private async void Back_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///locators");
        }
    }
}
