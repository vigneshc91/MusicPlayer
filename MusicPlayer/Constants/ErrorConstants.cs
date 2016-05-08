using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicPlayer.Constants
{
    public class ErrorConstants
    {
        public const string INDEX_CREATE_FAILED = "Index cannot be created";
        public const string LOGIN_FAILED = "Login failed. Please try again";
        public const string USER_NOT_LOGGED_IN = "User not logged in";
        public const string ADMIN_USERNAME_ALREADY_TAKEN = "Admin user name already taken. Please choose different user name";

        public const string INVALID_ID = "Invalid id";
        public const string INVALID_USERNAME_OR_PASSWORD = "Invalid user name or password";
        public const string NO_PREVILAGE = "You don't have previlage to perform this operation";
        public const string PASSWORD_UPDATE_FAILED = "password change failed. Please try again";
        public const string PROBLEM_CREATING_ADMIN = "Problem in creating admin. Please try again";
        public const string PROBLEM_LOGOUT = "Problem Logout";
        public const string PROBLEM_OCCURED_WHILE_GENERATING_PASSWORD = "Problem occured while generating password. Please try again";
        public const string REQUIRED_FIELD_EMPTY = "Required fields empty";
        public const string SOMEBODY_LOGGEDIN = "Somebody already logged in";
        public const string WRONG_PASSWORD = "Invalid password. Please try again ";
        public const string ADMIN_NOT_FOUND = "Admin not found";
        public const string PROBLEM_DELETING_AMDIN = "Problem in deleting admin. Please try again";
        public const string NO_CHANGES = "No changes to made";
        public const string PROBLEM_OCCURES_ON_RETRIVING_ADMIN_LIST = "Problem occured on retriving admin list. Please try again";
        public const string INVALID_DATA = "Invalid data. Please check the value you entered";

        public const string PROBLEM_ADDING_PRODUCT = "Problem in adding product. Please try again";
        public const string PRODUCT_WITH_GIVEN_ID_ALREADY_EXIST = "Product with given id already exist. Please use different product id";
        public const string PRODUCT_NOT_FOUND = "Product with the given id not found. Please check the product id";
        public const string PROBLEM_UPDATING_PRODUCT = "Problem in updating product. Please try again";
        public const string PROBLEM_DELETING_PRODUCT = "Problem in deleting product. Please try again";

        public const string PROBLEM_ADDING_USER = "Problem in adding user. Please try again";
        public const string USER_WITH_GIVEN_ID_ALREADY_EXIST = "user with given id already exist. Please use different product id";
        public const string USER_NOT_FOUND = "user with the given id not found. Please check the user id";
        public const string PROBLEM_UPDATING_USER = "Problem in updating user. Please try again";
        public const string PROBLEM_DELETING_USER = "Problem in deleting user. Please try again";

        public const string PROBLEM_ADDING_EMPLOYEE = "Problem in adding employee. Please try again";
        public const string EMPLOYEE_WITH_GIVEN_ID_ALREADY_EXIST = "employee with given id already exist. Please use different product id";
        public const string EMPLOYEE_NOT_FOUND = "employee with the given id not found. Please check the product id";
        public const string PROBLEM_UPDATING_EMPLOYEE = "Problem in updating employee. Please try again";
        public const string PROBLEM_DELETING_EMPLOYEE = "Problem in deleting employee. Please try again";

        public const string ALBUM_ALREADY_EXIST = "Album already exist";
        public const string PROBLEM_CREATING_ALBUM = "Problem in creating album. Please try again";
        public const string PROBLEM_UPDATING_ALBUM = "Problem in updating album. Please try again";
        public const string PROBLEM_DELETING_ALBUM = "Problem in deleting album. Please try again";
        public const string ALBUM_NOT_FOUND = "Album not found";

        public const string PROBLEM_ADDING_SONG = "Problem in adding song. Please try again";
        public const string PROBLEM_UPDATING_SONG = "Problem in updating song. Please try again";
        public const string PROBLEM_DELETING_SONG = "Problem in deleting song. Please try again";
        public const string SONG_NOT_FOUND = "Song not found";

    }
}