// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using IdentityModel;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;

namespace CompanyEmployees.IDP;

public class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = 69118,
                country = "Germany"
            };
                
            return new List<TestUser>
            {
                new()
                { 
                    SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4", 
                    Username = "John", 
                    Password = "JohnPassword", 
                    Claims = new List<Claim> 
                    { 
                        new("given_name", "John"), 
                        new("family_name", "Doe"),
                        new("address", "John Doe's Boulevard 323"),
                        new("role", "Administrator")   ,
                    } 
                }, 
                new()
                { 
                    SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340", 
                    Username = "Jane", 
                    Password = "JanePassword", 
                    Claims = new List<Claim> 
                    { 
                        new("given_name", "Jane"), 
                        new("family_name", "Doe"),
                        new("address", "Jane Doe's Avenue 214"),
                        new("role", "Visitor") 
                    }
                }
            };
        }
    }
}
