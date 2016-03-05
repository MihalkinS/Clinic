using Clinic.Api.Models.AppModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinic.Api.Models.ViewModels
{
    public class ClientViewModel
    {
        public string Id { get; set; }
        public string PetName { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string Address { get; set; }

        public string AvatarURL { get; set; }

        public ClientViewModel(Client client)
        {
            if (client != null)
            {
                Id = client.UserId;

                PetName = client.PetName;
                Breed = client.Breed;
                Color = client.Color;

                FirstName = client.FirstName;
                LastName = client.LastName;
                MiddleName = client.MiddleName;

                Address = client.Address;

                AvatarURL = client.AvatarURL;
            }
        }
    }
}