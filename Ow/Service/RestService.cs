using Newtonsoft.Json;
using Ow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace Ow.Service
{
    public class RestService
    {
        private Uri serviceUrl;
        private HttpClient client;

        public RestService(Uri serviceUrl) 
        {
            this.serviceUrl = serviceUrl;

            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<User> Signin(Login login) 
        {
            HttpStringContent postContent = new HttpStringContent(JsonConvert.SerializeObject(login));
            postContent.Headers["Content-Type"] = "application/json";

            try 
            { 
                HttpResponseMessage response = await client.PostAsync(new Uri(serviceUrl, "/api/v1/users/login"), postContent);

                if (response.IsSuccessStatusCode) 
                { 
                    string content = await response.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(content);

                    return user;
                }
            }
            catch
            {
                
            }

            return null;
        }

        public async Task<User> Verify(Registration registration)
        {
            HttpStringContent postContent = new HttpStringContent(JsonConvert.SerializeObject(registration));
            postContent.Headers["Content-Type"] = "application/json";

            try 
            { 
                HttpResponseMessage response = await client.PostAsync(new Uri(serviceUrl, "/api/v1/users/verify"), postContent);

                if (response.IsSuccessStatusCode) 
                {
                    string content = await response.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(content);

                    return user;
                }
            }
            catch
            {

            }

            return null;
        }

        public async Task<List<Contact>> SyncContacts(User user, List<Contact> contacts)
        {
            HttpStringContent postContent = new HttpStringContent(JsonConvert.SerializeObject(contacts));
            postContent.Headers["Content-Type"] = "application/json";

            try 
            {
                string path = string.Format("/api/v1/users/{0}/contacts/sync", user.Id);

                HttpResponseMessage response = await client.PostAsync(
                    new Uri(serviceUrl, path), postContent);

                if (response.IsSuccessStatusCode) 
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<Contact> syncedContacts = JsonConvert.DeserializeObject<List<Contact>>(content);

                    return syncedContacts;
                }
            }
            catch
            {

            }

            return null;
        }

        public async Task<List<Contact>> LoadContacts(User user)
        {
            string path = string.Format("/api/v1/users/{0}/contacts", user.Id);

            HttpResponseMessage response = await client.GetAsync(
                new Uri(serviceUrl, path));

            try 
            { 
                if (response.IsSuccessStatusCode) 
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<Contact> contacts = JsonConvert.DeserializeObject<List<Contact>>(content);

                    return contacts;
                }
            }
            catch
            {

            }

            return null;
        }
    }
}
