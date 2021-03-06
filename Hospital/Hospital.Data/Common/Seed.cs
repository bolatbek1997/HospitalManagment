﻿namespace Hospital.Data.Common
{
    using Constants;
    using DatabaseModels;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Seed
    {
        private GenerateRandomData random = new GenerateRandomData();
        
        internal void SeedAdminAndPatients(ApplicationDbContext context)
        {

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<User>(new UserStore<User>(context));

            if (!roleManager.RoleExists(GlobalConstants.adminRoleName))
            {
                var role = new IdentityRole()
                {
                    Name = GlobalConstants.adminRoleName
                };
                var role2 = new IdentityRole()
                {
                    Name = "doctor"
                };
                var role3 = new IdentityRole()
                {
                    Name = "user"
                };
                roleManager.Create(role);
                roleManager.Create(role2);
                roleManager.Create(role3);

                var user = new User()
                {
                    UserName = GlobalConstants.adminUserName,
                    Email = GlobalConstants.adminUserName
                };

                for (int i = 0; i < 10; i++)
                {
                    var patient = new User()
                    {
                        UserName = "patient" + i + "@patient.com",
                        Email = "patient" + i + "@patient.com",
                        ClinicalResults = new List<ClinicalResult>()
                        {
                            new ClinicalResult()
                            {
                                StatusResult = StatusResult.Ready
                            },
                            new ClinicalResult()
                            {
                                StatusResult = StatusResult.inProcess
                            }
                        }
                    };

                    context.Users.Add(patient);
                }
                context.SaveChanges();


                IdentityResult createUserResult = userManager.Create(user, GlobalConstants.adminPassword);
                if (createUserResult.Succeeded)
                {
                    userManager.AddToRole(user.Id, GlobalConstants.adminRoleName);
                }
            }
        }


        private Image CreateImage()
        {
            var img = new Image()
            {
                FileName = "doctor1.jpg",
                Path = PathConstants.PathImage
            };

            return img;
        }
        

        internal void SeedDoctorsClinicalTrialsAndSpecialties(ApplicationDbContext context)
        {
            if (!context.Doctors.Any() || !context.ClinicalTrials.Any() || !context.Specialities.Any())
            {
                for (int i = 1; i < 5; i++)
                {
                    var clinicalTrial = new ClinicalTrial
                    {
                        Title = "Емдеу түрі - " + i + ".1",
                        Price = random.RandomDataInt(25, 4587),
                        IsAvailable = IsAvailable.Yes,
                    };

                    var clinicalTrial2 = new ClinicalTrial
                    {
                        Title = "Емдеу түрі  - " + i + ".2",
                        Price = random.RandomDataInt(48, 565),
                        IsAvailable = IsAvailable.No,
                    };

                    var listDoctors = new List<UserInfo>();
                    for (int j = 0; j < random.RandomDataInt(2, 6); j++)
                    {
                        var doctor = new UserInfo
                        {
                            Name = "Doctor - " + random.RandomDataString(6, 10) + " " + random.RandomDataString(10, 13),
                            Image = CreateImage(),
                            Description = random.RandomDataString(400, 1000)
                        };
                        listDoctors.Add(doctor);
                    }

                    var specialty = new Speciality
                    {
                        Title = "Бөлім - " + i,
                        Description = random.RandomDataString(200, 500),
                        ClinicalTrials = new HashSet<ClinicalTrial>()
                        {
                            clinicalTrial,
                            clinicalTrial2
                        },
                        Doctors = listDoctors
                    };
                    context.Specialities.Add(specialty);
                    context.SaveChanges();
                }
            }
        }
    }
}
