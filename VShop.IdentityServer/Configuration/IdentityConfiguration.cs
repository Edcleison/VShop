﻿using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using static System.Net.WebRequestMethods;

namespace VShop.IdentityServer.Configuration;

public class IdentityConfiguration
{
    public const string Admin = "Admin";
    public const string Client = "Client";

    public static IEnumerable<IdentityResource> IdendityResources =>
        new List<IdentityResource>
    {

            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
    };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            //vshop é a aplicação que vai acessar o IdentityServer para obter o token
            new ApiScope("vshop","Vshop Server"),
            new ApiScope(name:"read","Read data."),
            new ApiScope(name:"write","Write data."),
            new ApiScope(name:"delete","Delete data."),
        };

    public static IEnumerable<Client> Clients =>
        new List<Client> {
        //cliente genérico
        new Client{
            ClientId = "client",
            ClientSecrets = {new Secret("abracadabra#simsalabim".Sha256())},
            AllowedGrantTypes = GrantTypes.ClientCredentials, // precisa das credenciais do usuário
            AllowedScopes = {"read","write","profile"}
        },
        new Client
        {
            ClientId = "vshop",
            ClientSecrets = {new Secret("abracadabra#simsalabim".Sha256())},
            AllowedGrantTypes= GrantTypes.Code, //via codigo
            RedirectUris = { "https://localhost:7224/signin-iodc" }, //login
            PostLogoutRedirectUris = { "https://localhost:7224/signout-callback-oidc"},//
            AllowedScopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                "vshop"
            }
        }
        };
}

