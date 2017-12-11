﻿using System;
using System.Collections.Generic;
using System.Text;
using Foundation;
using Google.SignIn;
using UIKit;

namespace Cito.iOS
{
    public class GoogleLogin : UIViewController, ISignInDelegate, ISignInUIDelegate
    {
        internal static bool IsGoogleLogin;

        public GoogleLogin()
        {
            SignIn.SharedInstance.UIDelegate = this;
            SignIn.SharedInstance.Delegate = this;
        }
        public void HandleGoogleLoginClicked()
        {
            SignIn.SharedInstance.SignInUserSilently();
        }


        public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
                  
            if (user != null && error == null)
            {
                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.ModalViewController;
                }

             
            }
            
            //App.Locator.Prelogin.ExternalLoginCommand.Execute(null);
        }


        [Export("signInWillDispatch:error:")]
        public void WillDispatch(SignIn signIn, NSError error)
        {
        }

        [Export("signIn:presentViewController:")]
        public void PresentViewController(SignIn signIn, UIViewController viewController)
        {
            PresentViewController(viewController, true, null);
        }

        [Export("signIn:dismissViewController:")]
        public void DismissViewController(SignIn signIn, UIViewController viewController)
        {
            DismissViewController(true, null);
        }
    }
}
