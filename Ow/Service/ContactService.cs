using Microsoft.Phone.UserData;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ow.Service
{

    public class ContactService 
    {
        SQLiteConnection connection;

        Contacts phoneContacts;

        public ContactService(SQLiteConnection connection)
        {
            this.connection = connection;
        }

        public Task<List<Ow.Model.Contact>> FindDeviceContacts()
        {
            phoneContacts = new Contacts();

            var taskCompletionSource =
                new TaskCompletionSource<List<Ow.Model.Contact>>();

            EventHandler<ContactsSearchEventArgs> handler = null;
            handler = (s, e) =>
            {
                phoneContacts.SearchCompleted -= handler;

                List<Ow.Model.Contact> contacts = new List<Model.Contact>();

                foreach (Contact deviceContact in e.Results)
                {
                    ContactPhoneNumber contactPhoneNumber = deviceContact.PhoneNumbers
                        .FirstOrDefault(o => o.Kind == PhoneNumberKind.Mobile);

                    if (deviceContact.CompleteName != null && contactPhoneNumber != null)
                    {
                        Ow.Model.Contact contact = new Ow.Model.Contact();
                        contact.FirstName = deviceContact.CompleteName.FirstName;
                        contact.LastName = deviceContact.CompleteName.LastName;
                        contact.PhoneNumber = contactPhoneNumber.PhoneNumber;
                        contacts.Add(contact);
                    }
                }

                taskCompletionSource.TrySetResult(contacts);
            };
            
            phoneContacts.SearchCompleted += handler;

            phoneContacts.SearchAsync(String.Empty,
                FilterKind.None, null);

            return taskCompletionSource.Task;
        }

        public List<Ow.Model.Contact> FindAllContacts()
        {
            var query = connection
                .Table<Ow.Model.Contact>();

            return query.ToList();
        }

        public void SaveAll(List<Ow.Model.Contact> contacts)
        {
            foreach (Ow.Model.Contact contact in contacts)
            { 

                bool exists = connection.ExecuteScalar<bool>("SELECT EXISTS(SELECT Id FROM Contacts WHERE Id = ?)", contact.Id);

                if(! exists)
                {
                    connection.Insert(contact);
                }
            }
        }
    }
}
