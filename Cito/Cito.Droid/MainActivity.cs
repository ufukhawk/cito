﻿using System;
using System.Linq;
using Acr.UserDialogs;
using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using ImageCircle.Forms.Plugin.Droid;
using Xamarin.Facebook;
using Xamarin.Forms;
using Context = System.Runtime.Remoting.Contexts.Context;


namespace Cito.Droid
{
    [Activity(Label = "Cito", Name = "cito.MainActivity", Theme = "@style/SplashScreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {        
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            SetTheme(Resource.Style.MainTheme);
            System.Net.ServicePointManager.DnsRefreshTimeout = 0;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            ImageCircleRenderer.Init();
            UserDialogs.Init(this);                               

            LoadApplication(new App());
            SetColors();
            
            if (FacebookSdk.IsInitialized)
                FacebookLogin.Handle();

            GoogleLogin.Handle();
           

        }

        private void SetColors()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                var themeColor = (Xamarin.Forms.Color)Xamarin.Forms.Application.Current.Resources.FirstOrDefault(res => res.Key.Equals("CitoMain")).Value;

                Window.SetStatusBarColor(new Android.Graphics.Color(Convert.ToInt32(themeColor.R * 255), Convert.ToInt32(themeColor.G * 255), Convert.ToInt32(themeColor.B * 255)));
                Window.SetNavigationBarColor(new Android.Graphics.Color(Convert.ToInt32(themeColor.R * 255), Convert.ToInt32(themeColor.G * 255), Convert.ToInt32(themeColor.B * 255)));
            }
            
        }

        

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (GoogleLogin.IsGoogleLogin && !GoogleLogin.MyGoogleApiClient.IsConnecting)
                GoogleLogin.MyGoogleApiClient.Connect();
            else if(FacebookLogin.IsFacebookLogin)
                FacebookLogin.CallbackManager.OnActivityResult(requestCode, (int) resultCode, data);

        }      

    }

 
}

