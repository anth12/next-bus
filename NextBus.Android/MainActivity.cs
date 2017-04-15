using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using NextBus.Logging;
using Plugin.Geolocator;
using Plugin.Permissions;
using Xamarin.Forms;

namespace NextBus.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            DependencyService.Register<UnifiedStorage.Android.FileSystem>();
            
            LoadApplication(new App());

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
        }

        private void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogHelper.Error<MainActivity>("Unhandled Exception", e?.ExceptionObject?.ToString() ?? e?.ToString());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}