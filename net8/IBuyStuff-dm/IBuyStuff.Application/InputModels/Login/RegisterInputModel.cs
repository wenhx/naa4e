﻿using IBuyStuff.Application.Utils;
using IBuyStuff.Application.ViewModels;
using IBuyStuff.Domain.Shared;

namespace IBuyStuff.Application.InputModels.Login
{
    public class RegisterInputModel : ViewModelBase
    {
        private string _avatar = String.Empty;

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string Avatar 
        {
            get
            { 
                return _avatar;
            }
            set 
            { 
                _avatar = value ?? String.Empty;
            }
        }

        public bool IsValid()
        {
            return Globals.IsAnyNullOrEmpty(UserName, FirstName, LastName, Email);
        }
    }
}