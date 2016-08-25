using IdentityServer4.Models;
using System.Collections.Generic;

namespace Host.Configuration
{
    public class Scopes
    {
        public static IEnumerable<Scope> Get()
        {
            return new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.ProfileAlwaysInclude,
                StandardScopes.EmailAlwaysInclude,
                StandardScopes.OfflineAccess,
                StandardScopes.RolesAlwaysInclude,

                new Scope
                {
                    Name = "mvcaccess",
                    DisplayName = "Access to the MVC Application",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("mvclogin")
                    }
                },

                new Scope
                {
                    Name = "api1",
                    DisplayName = "API 1",
                    Description = "API 1 features and data",
                    Type = ScopeType.Resource,

                    ScopeSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim("role")
                    }
                },
                new Scope
                {
                    Name = "api2",
                    DisplayName = "API 2",
                    Description = "API 2 features and data, which are better than API 1",
                    Type = ScopeType.Resource
                }
            };
        }
    }
}
//using IdentityServer4.Models;
//using System.Collections.Generic;

//namespace Host.Configuration
//{
//    public class Scopes
//    {
//        public static IEnumerable<Scope> Get()
//        {
//            return new List<Scope>
//            {
//                StandardScopes.OpenId,
//                StandardScopes.ProfileAlwaysInclude,
//                StandardScopes.EmailAlwaysInclude,
//                StandardScopes.OfflineAccess,
//                StandardScopes.RolesAlwaysInclude,

//                new Scope
//                {
//                    Name = "ASPNETCoreKestrelResearch",
//                    DisplayName = "ASPNETCoreKestrelResearch Login",
//                    Description = "Used to login to ASPNETCoreKestrelResearch",
//                    Type = ScopeType.Resource,

//                    ScopeSecrets = new List<Secret>
//                    {
//                        new Secret("aspnetcorekestrelresearchsecret".Sha256())
//                    },
//                    Claims = new List<ScopeClaim>
//                    {
//                    }
//                },new Scope
//                {
//                    Name = "MealsApi1.0",
//                    DisplayName = "Meals API v1.0",
//                    Description = "Meals API 1.0 features and data",
//                    Type = ScopeType.Resource,

//                    ScopeSecrets = new List<Secret>
//                    {
//                        new Secret("mealsecret".Sha256())
//                    },
//                    Claims = new List<ScopeClaim>
//                    {
//                        new ScopeClaim("read"),
//                        new ScopeClaim("write")
//                    }
//                },
//                new Scope
//                {
//                    Name = "QAndAApi1.0",
//                    DisplayName = "Questions and Answers API 1.0",
//                    Description = "Question and Answers 1.0 features and data",
//                    Type = ScopeType.Resource,                    
//                    ScopeSecrets = new List<Secret>
//                    {
//                        new Secret("qandasecret".Sha256())
//                    },
//                    Claims = new List<ScopeClaim>
//                    {
//                        new ScopeClaim("Ask"),
//                        new ScopeClaim("Answer")
//                    }                    
//                }                
//            };
//        }
//    }
//}