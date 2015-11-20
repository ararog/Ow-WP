using Newtonsoft.Json;
using Ow.Model;
using SQLite;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Service
{
    public class HistoryService
    {
        private SQLiteConnection connection;

        public HistoryService(SQLiteConnection connection)
        {
            this.connection = connection;
        }

        public List<Notification> FindAllNotifications()
        {
            List<Notification> notifications = new List<Notification>();

            var thirtyDaysAgo = DateTime.Now.AddDays(-30);

            var query = connection.Table<Notification>()
                .Where(o => o.Date >= thirtyDaysAgo);

            notifications = query.ToList();

            return notifications;
        }

        public int CountUnreadNotifications()
        {
            var query = connection.Table<Notification>()
                .Where(o => ! o.Read);

            return query.Count();
        }

        public void SaveNotification(Notification notification)
        {
            if(notification.Id == null)
                connection.Insert(notification);
            else
                connection.Update(notification);
        }

        public void DeleteAllNotifications()
        {
            connection.DeleteAll<Notification>();
        }
    }
}
