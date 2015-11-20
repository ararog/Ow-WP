using Microsoft.Devices;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Audio;
using Ow.Model;
using Ow.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Ow.Service
{
    public class AlertService
    {

        public void ExecuteAlertForNotification(Notification notification, bool withSound)
        {
            VibrateController vibrateController = VibrateController.Default;
            vibrateController.Start(TimeSpan.FromSeconds(1));

            if (withSound)
            {
                Stream soundStream = Application.GetResourceStream(new Uri("Assets/Audio/ow.wav", UriKind.Relative)).Stream;
                SoundEffectInstance Sound = SoundEffect.FromStream(soundStream).CreateInstance();
                Sound.Play();

                while (Sound.State == SoundState.Playing)
                {

                }

                switch (notification.Type)
                {
                    case 1:
                        soundStream = Application.GetResourceStream(new Uri("Assets/Audio/vamo.wav", UriKind.Relative)).Stream;
                        break;

                    case 2:
                        soundStream = Application.GetResourceStream(new Uri("Assets/Audio/perai.wav", UriKind.Relative)).Stream;
                        break;

                    case 3:
                        soundStream = Application.GetResourceStream(new Uri("Assets/Audio/chegou.wav", UriKind.Relative)).Stream;
                        break;

                    case 4:
                        soundStream = Application.GetResourceStream(new Uri("Assets/Audio/maneiro.wav", UriKind.Relative)).Stream;
                        break;

                    case 5:
                        soundStream = Application.GetResourceStream(new Uri("Assets/Audio/tocomfome.wav", UriKind.Relative)).Stream;
                        break;

                    case 6:
                        soundStream = Application.GetResourceStream(new Uri("Assets/Audio/owkey.wav", UriKind.Relative)).Stream;
                        break;
                }

                Sound = SoundEffect.FromStream(soundStream).CreateInstance();
                Sound.Play();
            }

            ShellToast toast = new ShellToast();

            toast.Title = "Ow";
            toast.Content = String.Format(AppResources.NotificationMessage, 
                notification.Contact.FirstName, notification.TypeToString());
            
            toast.Show();
        }
    }
}
