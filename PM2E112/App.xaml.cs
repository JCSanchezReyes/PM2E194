using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PM2E112.Controller;
using System.IO;
using Xamarin.Essentials;

namespace PM2E112
{
    public partial class App : Application
    {
        static DataBase db;

        public static DataBase DBase
        {
            get
            {
                //se declara la ruta donde estan los archivos de la BD
                if (db == null)
                {
                    String FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EmpleDB.db3");//db3 es extencion de sqlite
                    db = new DataBase(FolderPath);
                }

                return db;
            }
        }
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new MainPage());

            //ExperimentalFeatures.Enable(ExperimentalFeatures.ShareFileRequest);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
