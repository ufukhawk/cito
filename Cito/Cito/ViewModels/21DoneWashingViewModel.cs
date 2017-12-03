﻿using System.Threading.Tasks;
using Cito.Views;

namespace Cito.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;

    using Plugin.Media;

    using Xamarin.Forms;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    public class DoneWashingViewModel : CitoViewModelBase
    {

        public DoneWashingViewModel()
        {
            this.DoneWashingIndicatorColor = Color.Transparent;
        }

        private ImageSource photo;

        public ImageSource Photo
        {
            get
            {
                try
                {
                    return photo;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            set { Set(ref photo, value); }
        }

        private bool _photoTaken;

        public bool PhotoTaken
        {
            get { return _photoTaken; }
            set { Set(ref _photoTaken, value); }
        }

        private bool doneWashing;

        public bool DoneWashing
        {
            get => this.doneWashing;
            set => Set(ref this.doneWashing, value);
        }

        private Color doneWashingIndicatorColor;

        public Color DoneWashingIndicatorColor
        {
            get => this.doneWashingIndicatorColor;
            set => Set(ref this.doneWashingIndicatorColor, value);
        }

        public ICommand DoneWashingCommand => new Command(
            async () =>
                {
                    this.DoneWashing = true;
                    this.DoneWashingIndicatorColor = Color.FromHex("26a4ad");
                });


        public ICommand DoneCommand => new Command(async () =>
        {
            await GoToRootPage();
            PhotoTaken = false;
            DoneWashing = false;
        });

        public ICommand GoToPhotoPageCommand => new Command(async () =>
            {
                await this.GoToPage(new DoneWashingPhotoPage());
                this.TakePhoto();
            });

        public ICommand TakePhotoCommand => new Command(async () => await TakePhoto());
        public async Task TakePhoto() // takePhoto.Clicked += async(sender, args) =>

        {
            if (!DoneWashing)
            {
                return;
            }

            await CrossMedia.Current.Initialize();
    
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                //DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var x = new Plugin.Media.Abstractions.StoreCameraMediaOptions();

            var file = await CrossMedia.Current.TakePhotoAsync(x);

            if (file == null)
                return;

            //await DisplayAlert("File Location", file.Path, "OK");

            //var src = ImageSource.FromStream(() =>
            //    {
            //        var stream = file.GetStream();
            //        file.Dispose();
            //        return stream;
            //    });

            //or:
            var src = ImageSource.FromFile(file.Path);
            //image.Dispose();

            if (src != null)
            {
                Photo = src;
                PhotoTaken = true;
            }

          
            
        }
    }
}
