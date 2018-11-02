using GitHubCMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GitHubCMS.Services
{
    public interface IGitHubService
    {
        Task<List<GHFile>> GetFiles();
    }

    public class GitHubService : IGitHubService
    {
        public async Task<List<GHFile>> GetFiles()
        {
            using (var client = new HttpClient())
            {
                //setup HttpClient
                var fullApiUrl = $"http://api.github.com/repos/martinkearn/Content/contents/Blogs";
                client.BaseAddress = new Uri(fullApiUrl);
                client.DefaultRequestHeaders.Add("User-Agent", "GitHub-CMS");

                //setup httpContent object
                var response = await client.GetAsync(fullApiUrl);

                //return null if not sucessfull
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                //convert response to files
                var responseString = await response.Content.ReadAsStringAsync();
                var files = JsonConvert.DeserializeObject<IEnumerable<GHFile>>(responseString);

                //return
                return files.ToList();
            }
        }
    }
}
