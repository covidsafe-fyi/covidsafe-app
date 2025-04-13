private static Marker AddMarker(GoogleMap map, IMapPin pin, List<Marker> markers, MarkerOptions markerOption)
    {
        Marker marker = map.AddMarker(markerOption);
        pin.MarkerId = marker.Id;
        markers.Add(marker);

        return marker;
    }

private void addPins(IEnumerable<IMapPin> mapPins)
    {
        if (Map is null || MauiContext is null)
        {
            return;
        }

        foreach (IMapPin pin in mapPins)
        {
            IElementHandler pinHandler = pin.ToHandler(MauiContext);
            if (pinHandler is IMapPinHandler mapPinHandler)
            {
                if (pin is CustomStorePin customStorePin)
                {
#if ANDROID
                MarkerOptions markerOption = mapPinHandler.PlatformView;

                    customStorePin.Marker = AddMarker(Map, pin, Markers, markerOption);

#elif IOS

                    customStorePin.DisplayAnnotation = () =>
                    {
                        if (mapPinHandler.PlatformView as MKMapView is MKMapView mKMapView && mKMapView.Annotations.SingleOrDefault(annotation => annotation.Coordinate.Latitude == pin?.Location.Latitude && annotation.Coordinate.Longitude == pin.Location.Longitude) is IMKAnnotation mKAnnotation)
                        {
                            mKMapView.SelectAnnotation(mKAnnotation, true);
                        }
                    };
#endif
                }
            }
        }
    }