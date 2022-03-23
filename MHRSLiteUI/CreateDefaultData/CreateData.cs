﻿using ClosedXML.Excel;
using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteEntityLayer.Enums;
using MHRSLiteEntityLayer.IdentityModels;
using MHRSLiteEntityLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MHRSLiteUI.CreateDefaultData
{
    public static class CreateData
    {
        public static void Create(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IUnitOfWork unitOfWork,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            // Eklenmesini istediğim verileri ekleyecek metotları çağıralım...
            CheckRoles(roleManager);
            CreateCities(environment, unitOfWork);
            CreateClinics(environment, unitOfWork);

            //Bu yöntemle sadece Klinik, CheckRoles ve Cities'in  import edilmesi uygundur.
            //İlçeler hastaneler gibi çok datanın olduğu durumlarda SQL scripti ile datalarınızı eklemeniz avantajlıdır.
            //SQL query oluşturmakta zorlanacağınız kadar çok veri varsa
            // ozaman dataları excele yapıştırıp Console application tarzı bir uygulama ile aşağıdaki kodları kullanarak datalarınızı daha kolay ekleyebilirsiniz.

            //Canlıya çıkıldığında bu metot olmayacak
        #if DEBUG
            CreateDistricts(environment, unitOfWork);
            CreateHospitals(environment, unitOfWork);
        #endif
        }

        private static void CreateHospitals(IWebHostEnvironment environment, IUnitOfWork unitOfWork)
        {
            try
            {
                var hospitalList = unitOfWork.HospitalRepository
                    .GetAll().ToList();

                //Provide a path for excel file
                // Excel dosyasının bulunduğu yolu aldık
                string path = Path.Combine(environment.WebRootPath, "Excels");
                string fileName = Path.GetFileName("Hospitals.xlsx");
                string filePath = Path.Combine(path, fileName);
                using (var excelBook = new XLWorkbook(filePath))
                {
                    var rows = excelBook.Worksheet(1).RowsUsed();
                    foreach (var item in rows)
                    {
                        if (item.RowNumber() > 1
                            && item.RowNumber() <= rows.Count())
                        {
                            var cell = item.Cell(1).Value; //hastane adı
                            //ilçe id
                            var districtId = Convert.ToInt32(item.Cell(2).Value);
                            //adress
                            var address = item.Cell(3).Value; //hastane adresi
                          //email
                            var email = item.Cell(4).Value; //hastane emaili
                           //Latitude
                            var latitude = item.Cell(5).Value; //hastane Latitude  
                            //Longitude
                            var longitude = item.Cell(6).Value; //hastane Longitude 
                            //PhoneNumer
                            var phoneNumber = item.Cell(7).Value; //hastane teli

                            var district = unitOfWork.DistrictRepository
                              .GetFirstOrDefault(x => x.Id == districtId);

                            Hospital hospital = new Hospital()
                            {
                                HospitalName=cell.ToString().Trim(),
                                DistrictId=districtId,
                                CreatedDate=DateTime.Now,
                                Address=address.ToString(),
                                Email=email.ToString(),
                                Latitude=latitude.ToString(),
                                Longitude=longitude.ToString(),
                                PhoneNumber=phoneNumber.ToString()
                                };

                            if (hospitalList
                                .Count(x => x.HospitalName.ToLower()
                                == cell.ToString().ToLower()
                                && x.DistrictId == districtId) == 0)
                            {
                                unitOfWork.HospitalRepository.Add(hospital);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private static void CreateDistricts(IWebHostEnvironment environment, IUnitOfWork unitOfWork)
        {
            try
            {
                var districtList = unitOfWork.DistrictRepository
                    .GetAll().ToList();

                //Provide a path for excel file
                // Excel dosyasının bulunduğu yolu aldık
                string path = Path.Combine(environment.WebRootPath, "Excels");
                string fileName = Path.GetFileName("Districts.xlsx");
                string filePath = Path.Combine(path, fileName);
                using (var excelBook = new XLWorkbook(filePath))
                {
                    var rows = excelBook.Worksheet(1).RowsUsed();
                    foreach (var item in rows)
                    {
                        if (item.RowNumber() > 1
                            && item.RowNumber() <= rows.Count())
                        {
                            var cell = item.Cell(1).Value; //ilçe adı
                            var cityId = Convert.ToByte(item.Cell(2).Value);//1
                            var city = unitOfWork.CityRepository
                              .GetFirstOrDefault(x => x.Id == cityId);
                            District district = new District()
                            {
                                DistrictName = cell.ToString(),
                                CityId = cityId,
                                CreatedDate = DateTime.Now
                            };

                            if (districtList
                                .Count(x => x.DistrictName.ToLower()
                                == cell.ToString().ToLower()
                                && x.CityId == cityId) == 0)
                            {
                                unitOfWork.DistrictRepository.Add(district);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        private static void CreateClinics(IWebHostEnvironment environment, IUnitOfWork unitOfWork)
        {
            try
            {
                var clinicList = unitOfWork.ClinicRepository
                    .GetAll().ToList();

                //Provide a path for excel file
                // Excel dosyasının bulunduğu yolu aldık
                string path = Path.Combine(environment.WebRootPath, "Excels");
                string fileName = Path.GetFileName("Clinics.xlsx");
                string filePath = Path.Combine(path, fileName);
                using (var excelBook = new XLWorkbook(filePath))
                {
                    var rows = excelBook.Worksheet(1).RowsUsed();
                    foreach (var item in rows)
                    {
                        if (item.RowNumber() > 1
                            && item.RowNumber() <= rows.Count())
                        {
                            var cell = item.Cell(1).Value;
                            Clinic clinic = new Clinic()
                            {
                                ClinicName = cell.ToString(),
                                CreatedDate = DateTime.Now
                            };
                            if (clinicList
                                .Count(x => x.ClinicName.ToLower()
                                == cell.ToString().ToLower()) == 0)
                            {
                                unitOfWork.ClinicRepository.Add(clinic);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private static void CheckRoles(RoleManager<AppRole> roleManager)
        {
            var allRoles = Enum.GetNames(typeof(RoleNames));
            foreach (var item in allRoles)
            {
                if (!roleManager.RoleExistsAsync(item).Result)
                {
                    var result = roleManager.CreateAsync(new AppRole()
                    {
                        Name = item,
                        Description = item
                    }).Result;
                }
            }
        }

        private static void CreateCities(IWebHostEnvironment environment,
            IUnitOfWork unitOfWork)
        {
            try
            {
                var cityList = unitOfWork.CityRepository
                   .GetAll().ToList();

                //Provide a path for excel file
                // Excel dosyasının bulunduğu yolu aldık
                string path = Path.Combine(environment.WebRootPath, "Excels");
                string fileName = Path.GetFileName("Cities.xlsx");
                string filePath = Path.Combine(path, fileName);
                using (var excelBook = new XLWorkbook(filePath))
                {
                    var rows = excelBook.Worksheet(1).RowsUsed();
                    foreach (var item in rows)
                    {
                        if (item.RowNumber() > 1 && item.RowNumber() <= rows.Count())
                        {
                            var cell = item.Cell(1).Value;//İstanbul
                            var plateCode = item.Cell(2).Value;
                            City city = new City()
                            {
                                CreatedDate = DateTime.Now,
                                CityName = cell.ToString(),
                                PlateCode = Convert.ToByte(plateCode)
                            };
                            if (cityList
                              .Count(x => x.CityName.ToLower()
                              == cell.ToString().ToLower()) == 0)
                            {
                                unitOfWork.CityRepository.Add(city);
                            }
                            //buraya geri döneceğiz.
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
