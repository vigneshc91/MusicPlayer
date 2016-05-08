using MusicPlayer.Constants;
using MusicPlayer.Helper;
using MusicPlayer.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPlayer.Managers
{
    public class UserManager:ElasticSearchManager
    {
        public bool AddUser( User user)
        {
            if (user == null) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            try
            {
                var elasticClient = GetElasticClient();

                var userExist = GetUserByEmail(user.email);
                if (userExist != null) throw new Exception(ErrorConstants.USER_WITH_GIVEN_ID_ALREADY_EXIST);

                var response = elasticClient.Index<User>(user, i => i
                 .Index(ElasticMappingConstants.INDEX_NAME)
                 .Type(ElasticMappingConstants.TYPE_USER)
                );

                return response.RequestInformation.Success;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public User GetUserByEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email)) throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            try
            {
                var elasticClient = GetElasticClient();

                var response = elasticClient.Search<User>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_USER)
                .Filter(f => f.Term(ConstUser.EMAIL, email))
                .Size(1));

                User retUser = null;

                if(response.Total > 0)
                    foreach (var item in response.Hits)
                        retUser = item.Source;

                return retUser;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public User LoginUser(string email, string password)
        {
            if (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(password))
                throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            try
            {
                var elasticClient = GetElasticClient();

                var emailFilter = new TermFilter()
                {
                    Field = ConstUser.EMAIL,
                    Value = email
                };

                var passwordFilter = new TermFilter()
                {
                    Field = ConstUser.PASSWORD,
                    Value = password
                };

                var loginFilters = new List<FilterContainer>();
                loginFilters.Add(emailFilter);
                loginFilters.Add(passwordFilter);

                var loginFilter = new AndFilter();
                loginFilter.Filters = loginFilters;

                var loginResponse = elasticClient.Search<User>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_USER)
                .Filter(loginFilter)
                .Size(1));

                var user = new User();

                if (loginResponse.Total > 0)
                {
                    foreach (IHit<User> hit in loginResponse.Hits)
                    {
                        user = hit.Source;
                    }
                    
                    return user;
                }
                else
                    return null;


            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.GetBaseException().Message);
                return null;
            }
        }

        public User LoginWithFacebook(string name, string email)
        {
            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(email))
                throw new Exception(ErrorConstants.REQUIRED_FIELD_EMPTY);

            try
            {
                var elasticClient = GetElasticClient();

                var emailFilter = new TermFilter()
                {
                    Field = ConstUser.EMAIL,
                    Value = email
                };

               

                var loginFilters = new List<FilterContainer>();
                loginFilters.Add(emailFilter);
                

                var loginFilter = new AndFilter();
                loginFilter.Filters = loginFilters;

                var loginResponse = elasticClient.Search<User>(s => s
                .Index(ElasticMappingConstants.INDEX_NAME)
                .Type(ElasticMappingConstants.TYPE_USER)
                .Filter(loginFilter)
                .Size(1));

                var user = new User();

                if (loginResponse.Total > 0)
                {
                    foreach (IHit<User> hit in loginResponse.Hits)
                    {
                        user = hit.Source;
                    }

                    return user;
                }
                else
                {
                    var registerUser = new User() {
                        userid = Guid.NewGuid(),
                        name = name,
                        email = email,
                        created_at = DateTime.Now
                    };

                    var registerResponse = elasticClient.Index<User>(registerUser, i => i
                   .Index(ElasticMappingConstants.INDEX_NAME)
                   .Type(ElasticMappingConstants.TYPE_USER)
                    );

                    if (registerResponse.RequestInformation.Success)
                        return registerUser;
                    else
                        throw new Exception(ErrorConstants.PROBLEM_ADDING_USER);
                }
                    


            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.GetBaseException().Message);
                return null;
            }
        }

        public bool LogoutUser()
        {
            try
            {
                SessionHelper.RemoveSession();
                return true;
            }
            catch (Exception e)
            {

                Console.Error.WriteLine(e.GetBaseException().Message);
                throw new Exception(ErrorConstants.PROBLEM_LOGOUT);
            }
        }

       
    }
}