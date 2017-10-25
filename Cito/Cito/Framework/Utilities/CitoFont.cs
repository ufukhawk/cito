﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Cito.Framework.Utilities
{
    public static class CitoFont
    {
        public static string SetFont()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    return "Lato";
                case Device.Android:
                    return "/Assets/Lato-Regular.ttf";
                default:
                    return "";
            }
        }
    }
}
