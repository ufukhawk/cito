﻿using System;
using Acr.UserDialogs;
using Cito.Helpers;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace Cito.Framework.Utilities
{
    public static class Location
    {
        public static Position CurrentPosition { get; set; }

        public static async void GetUserLocation()
        {
            if (!CrossGeolocator.Current.IsGeolocationEnabled && CitoSettings.Current.IsUserLoggedIn)
            {
                UserDialogs.Instance.Toast("Please enable your GPS for better experience", TimeSpan.FromSeconds(4));
                return;
            }

            await CrossGeolocator.Current.GetPositionAsync().ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    CurrentPosition = t.Result;
                    App.Locator.Map.CurrentUserPosition =
                        new Xamarin.Forms.Maps.Position(Location.CurrentPosition.Latitude,
                            Location.CurrentPosition.Longitude);

                    CitoSettings.Current.LastLatitude = Location.CurrentPosition.Latitude;
                    CitoSettings.Current.LastLongitude = Location.CurrentPosition.Longitude;
                    
                }
            });
        }

        public static void StopGps()
        {
            //handle this
        }
    }
}
