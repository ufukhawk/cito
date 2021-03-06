using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Cito.ViewModels;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Forms;

namespace Cito.Droid
{
    internal static class FacebookLogin
    {
        internal static ICallbackManager CallbackManager;
        internal static Profile FacebookProfile = Profile.CurrentProfile;
        internal static FacebookCallback<LoginResult> LoginCallback;
        internal static Xamarin.Facebook.ProfileTracker FacebookProfileTracker = new ProfileTracker();
        internal static bool IsFacebookLogin;
        internal static bool FacebookLoggedIn;

        public static void Handle()
        {
            CallbackManager = CallbackManagerFactory.Create();
            RegisterFacebookCallbacks();
            HandleToken();
        }

        public static void Connect(int requestCode, Result resultCode, Intent data)
        {
            CallbackManager.OnActivityResult(requestCode, (int)resultCode, data);

            if(FacebookLoggedIn)
                App.Locator.Prelogin.ExternalLoginCommand.Execute(null);
        }

        public static void RegisterFacebookCallbacks()
        {
            CallbackManager = CallbackManagerFactory.Create();

            LoginCallback = new FacebookCallback<LoginResult>
            {
                HandleSuccess = loginResult =>
                {
                    FacebookSdk.ClientToken = AccessToken.CurrentAccessToken.Token;
                    FacebookProfileTracker.StartTracking();
                    FacebookLoggedIn = true;
                },

                HandleCancel = () =>
                {
                },

                HandleError = loginError =>
                {
					Console.WriteLine("fb login error");
                },

                HandleLogout = () =>
                {
                }
            };

            LoginManager.Instance.RegisterCallback(CallbackManager, LoginCallback);
        }

        public static void HandleToken()
        {
            if (AccessToken.CurrentAccessToken != null)
            {
                var token = AccessToken.CurrentAccessToken;
                FacebookSdk.ClientToken = token.Token;
                var permissions = AccessToken.CurrentAccessToken.Permissions;
                var deniedPermissions = AccessToken.CurrentAccessToken.DeclinedPermissions;
                LoginCallback.HandleSuccess.Invoke(new LoginResult(token, permissions, deniedPermissions));
            }
            else
            {
                FacebookSdk.ClientToken = null;
                LoginCallback.HandleCancel.Invoke();
            }
        }
    }


    internal class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
    {
        public Action HandleCancel { get; set; }
        public Action<FacebookException> HandleError { private get; set; }
        public Action<TResult> HandleSuccess { get; set; }
        public Action HandleLogout { get; set; }

        public void OnCancel()
        {
            try
            {
                App.UpdateLoading(true);
                HandleCancel?.Invoke();
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                App.UpdateLoading(false);
            }
        }

        public void OnError(FacebookException error)
        {
            try
            {
                App.UpdateLoading(true);
                HandleError?.Invoke(error);
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                App.UpdateLoading(false);
            }
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            try
            {
                App.UpdateLoading(true);
                HandleSuccess?.Invoke(result.JavaCast<TResult>());
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                App.UpdateLoading(false);
            }
        }

        public void OnLogout()
        {
            try
            {
                App.UpdateLoading(true);
                HandleLogout?.Invoke();
            }
            catch (Exception e)
            {
                // ignored
            }
            finally
            {
                App.UpdateLoading(false);
            }
        }
    }

    class ProfileTracker : Xamarin.Facebook.ProfileTracker
    {
        protected override void OnCurrentProfileChanged(Profile oldProfile, Profile currentProfile)
        {
            FacebookLogin.FacebookProfile = currentProfile;
            var pic = FacebookLogin.FacebookProfile.GetProfilePictureUri(300, 300).ToString();

            var profilePicture = ImageSource.FromUri(new Uri(pic));
            ProfileData.ProfilePicture = profilePicture;

            App.Locator.CreateAccount.FullName = FacebookLogin.FacebookProfile.Name;
            if (oldProfile == null) { }
        }
    }
}