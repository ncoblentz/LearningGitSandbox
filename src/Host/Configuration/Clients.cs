﻿using IdentityServer4.Models;
using System.Collections.Generic;

namespace Host.Configuration
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                ///////////////////////////////////////////
                // Console Client Credentials Flow Sample
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "client",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AllowedScopes = new List<string>
                    {
                        "api1", "api2"
                    }
                },

                ///////////////////////////////////////////
                // Custom Grant Sample
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "client.custom",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedGrantTypes = GrantTypes.List("custom"),

                    AllowedScopes = new List<string>
                    {
                        "api1", "api2"
                    }
                },

                ///////////////////////////////////////////
                // Console Resource Owner Flow Sample
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "roclient",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.OfflineAccess.Name,

                        "api1", "api2"
                    }
                },

                ///////////////////////////////////////////
                // Introspection Client Sample
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "roclient.reference",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AllowedScopes = new List<string>
                    {
                        "api1", "api2"
                    },

                    AccessTokenType = AccessTokenType.Reference
                },

                ///////////////////////////////////////////
                // MVC Implicit Flow Samples
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "mvc.implicit",
                    ClientName = "MVC Implicit",
                    ClientUri = "http://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        "http://localhost:44077/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:44077/"
                    },
                    LogoutUri = "http://localhost:44077/signout-oidc",

                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.Roles.Name,

                        "api1", "api2"
                    },
                },

                ///////////////////////////////////////////
                // MVC Hybrid Flow Samples
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "mvc.hybrid",
                    ClientName = "MVC Hybrid",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientUri = "https://loadbalancer/",                    

                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser = false,
                    RedirectUris = new List<string>
                    {
                        "https://loadbalancer:443/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "https://loadbalancer:443/"
                    },
                    LogoutUri = "https://loadbalancer/signout-oidc",

                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name
                    },
                },

                ///////////////////////////////////////////
                // JS OAuth 2.0 Sample
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "js_oauth",
                    ClientName = "JavaScript OAuth 2.0 Client",
                    ClientUri = "http://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        "http://localhost:28895/index.html"
                    },

                    AllowedScopes = new List<string>
                    {
                        "api1", "api2"
                    },
                },
                
                ///////////////////////////////////////////
                // JS OIDC Sample
                //////////////////////////////////////////
                new Client
                {
                    ClientId = "js_oidc",
                    ClientName = "JavaScript OIDC Client",
                    ClientUri = "http://identityserver.io",

                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = new List<string>
                    {
                        "http://localhost:7017/index.html",
                        "http://localhost:7017/silent_renew.html",
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:7017/index.html",
                    },

                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:7017"
                    },

                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.Roles.Name,
                        "api1", "api2"
                    },
                },
            };
        }
    }
}
//using IdentityServer4.Models;
//using System.Collections.Generic;

//namespace Host.Configuration
//{
//    public class Clients
//    {
//        public static IEnumerable<Client> Get()
//        {
//            return new List<Client>
//            {                
//                ///////////////////////////////////////////
//                // MyApp Client Credentials
//                //////////////////////////////////////////
//                //new Client
//                //{
//                //    ClientId = "myapp.client",
//                //    ClientSecrets = new List<Secret>
//                //    {
//                //        new Secret("myapp.clientcredentials".Sha256())
//                //    },
//                //    AllowedGrantTypes = GrantTypes.ClientCredentials,
//                //    AllowedScopes = new List<string>
//                //    {
//                //        "MealsApi1.0",
//                //        "QAndAApi1.0"
//                //    }
//                //}
//                new Client
//                {
//                    ClientId = "aspnetcorekestrelresearch",
//                    ClientName = "ASPNETCoreKestrelResearch",
//                    ClientSecrets = new List<Secret>
//                    {
//                        new Secret("aspnetcorekestrelresearchsecret".Sha256())
//                    },
//                    ClientUri = "http://localhost",
//                    AllowedGrantTypes = GrantTypes.Hybrid,
//                    RedirectUris = new List<string>
//                    {
//                        "http://localhost/signin-oidc",
//                        "http://localhost:5000/signin-oidc"
//                    },
//                    PostLogoutRedirectUris = new List<string>
//                    {
//                        "http://localhost",
//                        "http://localhost:5000"
//                    },
//                    LogoutUri = "http://localhost/logout-oidc",
//                    AllowedScopes = new List<string>
//                    {
//                        StandardScopes.OpenId.Name,
//                        StandardScopes.Profile.Name,
//                        "ASPNETCoreKestrelResearch"
//                    }

//                }
//            };
//        }
//    }
//}