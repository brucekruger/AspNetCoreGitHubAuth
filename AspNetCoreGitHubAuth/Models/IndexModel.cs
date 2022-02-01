using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Octokit;
using Octokit.Internal;

namespace AspNetCoreGitHubAuth.Models
{
    public class IndexModel : PageModel
    {
        public string GitHubAvatar { get; set; }
        public string GitHubLogin { get; set; }
        public string GitHubName { get; set; }
        public string GitHubUrl { get; set; }

        public IReadOnlyList<Repository> Repositories { get; set; }

        public async Task OnGetAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                GitHubAvatar = User.FindFirst(c => c.Type == "urn:github:avatar")?.Value;
                GitHubLogin = User.FindFirst(c => c.Type == "urn:github:login")?.Value;
                GitHubName = User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
                GitHubUrl = User.FindFirst(c => c.Type == "urn:github:url")?.Value;

                string accessToken = await HttpContext.GetTokenAsync("access_token");

                var github = new GitHubClient(new ProductHeaderValue("AspNetCoreGitHubAuth"),
                    new InMemoryCredentialStore(new Credentials(accessToken)));
                Repositories = await github.Repository.GetAllForCurrent();
            }
        }
    }
}
