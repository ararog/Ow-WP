using Autofac;
using Ow.Service;
using Ow.ViewModel;
using SQLite.Net;
using SQLite.Net.Platform.WindowsPhone8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Ow.Support
{
    public class OwModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<RestService>(c => new RestService(new Uri("http://oww.herokuapp.com"))).SingleInstance();

            builder.Register<SQLiteConnection>(c => new SQLiteConnection(
                new SQLitePlatformWP8(), Path.Combine(ApplicationData.Current.LocalFolder.Path, "ow.db"))).SingleInstance();

            builder.RegisterType<AlertService>().SingleInstance();

            builder.RegisterType<CountryService>().SingleInstance();

            builder.RegisterType<ContactService>().SingleInstance();

            builder.RegisterType<HistoryService>().SingleInstance();

            builder.RegisterType<Session>().SingleInstance();

            builder.RegisterType<MainPageModel>().PropertiesAutowired(); 

            builder.RegisterType<LoginPageModel>().PropertiesAutowired();

            builder.RegisterType<CountryPageModel>().PropertiesAutowired();

            builder.RegisterType<RegistrationPageModel>().PropertiesAutowired();

            builder.RegisterType<ChatManager>()
                .SingleInstance().PropertiesAutowired();
        }
    }
}
