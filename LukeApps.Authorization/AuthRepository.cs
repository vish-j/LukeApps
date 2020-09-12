using LukeApps.Authorization.Enums;
using LukeApps.Common.Helpers;
using LukeApps.EmployeeData;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;

namespace LukeApps.Authorization
{
    public class AuthRepository 
    {
        private static EmployeeProvider empProvider;

        private List<Employee> dbUsers;
        private List<Employee> usersToEdit;
        private List<Employee> usersToAdd;

        public AuthRepository()
        {
            empProvider = EmployeeProvider.GetEmployeeProvider();

        }


        private const string domainName = @"CORP1\";

        private string cleanADEntity(string ADEntity)
        {
            if (ADEntity.Length < domainName.Length || ADEntity.Substring(0, domainName.Length) != domainName)
                return ADEntity;
            else
                return ADEntity.Substring(domainName.Length);
        }

        public static IEnumerable<Employee> GetADUsers(string DomainPath)
        {
            using (DirectorySearcher search = setSearch(new DirectoryEntry(DomainPath)))
            {
                SearchResult result;
                SearchResultCollection resultCol = search.FindAll();
                if (resultCol != null)
                {
                    for (int counter = 0; counter < resultCol.Count; counter++)
                    {
                        result = resultCol[counter];
                        if (result.Properties.Contains("sAMAccountName"))
                        {
                            Employee objUser = new Employee
                            {
                                Username = getValueFromSearch(result, "sAMAccountName"),
                                DisplayName = getValueFromSearch(result, "displayname"),
                                Initials = getValueFromSearch(result, "initials"),
                                CompanyMail2 = getValueFromSearch(result, "mail"),
                                CompanyMail = getValueFromSearch(result, "otherMailbox"),
                                TelephoneNumber = getValueFromSearch(result, "telephoneNumber"),
                                OfficialDepartment = getValueFromSearch(result, "departmentNumber"),
                                Location = getValueFromSearch(result, "adsPath"),
                                UserStatus = !(result.Properties.Contains("userAccountControl")) ? UserStatus.Error : (((UserAccountControl)result.Properties["userAccountControl"][0] & UserAccountControl.ACCOUNTDISABLE) == UserAccountControl.ACCOUNTDISABLE) ? UserStatus.Disabled : UserStatus.Active
                            };

                            yield return objUser;
                        }
                    }
                }
            }
        }

        private static string getValueFromSearch(SearchResult result, string property)
            => !result.Properties.Contains(property) ? null : (string)result.Properties[property][0];

        private static DirectorySearcher setSearch(DirectoryEntry searchRoot)
        {
            DirectorySearcher search = new DirectorySearcher(searchRoot)
            {
                Filter = "(samAccountType=805306368)" // Users
            };
            search.PropertiesToLoad.Add("sAMAccountName");
            search.PropertiesToLoad.Add("mail");
            search.PropertiesToLoad.Add("usergroup");
            search.PropertiesToLoad.Add("displayname");
            search.PropertiesToLoad.Add("departmentNumber");
            search.PropertiesToLoad.Add("userAccountControl");
            search.PropertiesToLoad.Add("telephoneNumber");
            search.PropertiesToLoad.Add("initials");
            search.PropertiesToLoad.Add("otherMailbox");
            search.PageSize = int.MaxValue;
            search.SizeLimit = int.MaxValue;
            return search;
        }

        public void SyncUsers()
        {
            usersToAdd = new List<Employee>();
            usersToEdit = new List<Employee>();

            using (EmployeeDataEntities db = new EmployeeDataEntities())
            {
                dbUsers = db.Employees.ToList();
            }

            var locations = new List<string> {
                "LDAP://Domain/OU=Users,OU=AU_TebOM01,OU=BIT,DC=corp1,DC=ad-is,DC=net",
            };

            empProvider.ClearDictionaries();

            locations.ForEach(l => sync(l));

            //add user values and update both dicts. remove from dbUsers
            empProvider.AddUsers(usersToAdd);

            //edit user values and update both dicts. remove from dbUsers
            empProvider.EditUsers(usersToEdit);

            //change status for users that are still remaining in dbUsers to "Not Available"
            empProvider.ChangeUserStatus(dbUsers, UserStatus.NotAvailable);
        }


        private void cleanAndSortUserInfo(Employee item, Employee dbUser)
        {
            if (dbUser == null)
            {
                usersToAdd.Add(item);
            }
            else
            {
                bool IsUpdateNeeded = false;

                string adName = item.DisplayName.Trim();
                if (dbUser.DisplayName != adName)
                {
                    dbUser.DisplayName = adName;
                    IsUpdateNeeded = true;
                }

                if (dbUser.CompanyMail2 != item.CompanyMail2)
                {
                    dbUser.CompanyMail2 = item.CompanyMail2;
                    IsUpdateNeeded = true;
                }

                if (dbUser.CompanyMail != item.CompanyMail)
                {
                    dbUser.CompanyMail = item.CompanyMail;
                    IsUpdateNeeded = true;
                }

                string adInitial = item.TelephoneNumber?.Trim().Left(3).Trim();

                var IsInitialNumber = int.TryParse(adInitial?.Replace("+", ""), out int x);

                if (IsInitialNumber)
                    adInitial = null;

                if (adInitial == null)
                    adInitial = item.Initials;

                if (dbUser.Initials != adInitial && !string.IsNullOrEmpty(adInitial))
                {
                    dbUser.Initials = adInitial;
                    IsUpdateNeeded = true;
                }

                string adPhone = item.TelephoneNumber?.Trim();

                if (adPhone != null)
                    adPhone = adPhone.Right(8).Trim();

                adPhone = IsInitialNumber ? item.TelephoneNumber.Trim() : adPhone;
                adPhone = adInitial == adPhone ? null : adPhone;

                if (dbUser.TelephoneNumber != adPhone && !string.IsNullOrEmpty(adPhone))
                {
                    dbUser.TelephoneNumber = adPhone;
                    IsUpdateNeeded = true;
                }

                //if (dbUser.departmentNumber != item.departmentNumber)
                //{
                //    dbUser.departmentNumber = item.departmentNumber;
                //    IsUpdateNeeded = true;
                //}

                if (dbUser.Location != item.Location)
                {
                    dbUser.Location = item.Location;
                    IsUpdateNeeded = true;
                }

                if (dbUser.UserStatus != item.UserStatus)
                {
                    dbUser.UserStatus = item.UserStatus;
                    IsUpdateNeeded = true;
                }

                if (IsUpdateNeeded)
                    usersToEdit.Add(dbUser);
                else
                    empProvider.AddToDictionary(dbUser);

                dbUsers.Remove(dbUsers.FirstOrDefault(u => u.Username == item.Username));
            }
        }

        private void sync(string location)
        {
            var adUsers = GetADUsers(location);

            foreach (var item in adUsers)
                cleanAndSortUserInfo(item, dbUsers.FirstOrDefault(u => u.Username == item.Username));
        }

    }
}