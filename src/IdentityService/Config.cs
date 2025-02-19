using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("auctionApp", "Auction app full access"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman",
                AllowedScopes = {"openid", "profile", "auctionApp"},
                RedirectUris = {"https://www.getpostman.com/oauth2/callback"},
                ClientSecrets = new[] {new Secret("NotASecret".Sha256())},
                AllowedGrantTypes = {GrantType.ResourceOwnerPassword}
            },
            new Client
            {
                ClientId = "nextApp", 
                ClientName = "nextApp",
                ClientSecrets = {new Secret("secret" .Sha256()) },
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                RequirePkce = false, // For mobile application like React Native 
                // mobile app but we used React web based app
                // We would to use a different authentication flow called PKI because 
                // we not be able to store a secret in a React native app
                RedirectUris = {"http://localhost:3000/api/auth/callback/id-server"},
                AllowOfflineAccess = true, // This is so that we can enable refresh token functionality
                AccessTokenLifetime = 3600*24*30 //Equal to 1 Hour  and exted to month (3600*24*30)

            }
        };
}
