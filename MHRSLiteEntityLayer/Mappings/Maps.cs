﻿using AutoMapper;
using MHRSLiteEntityLayer.Models;
using MHRSLiteEntityLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.Mappings
{
    public class Maps: Profile
    {
        public Maps()
        {
            ////Appointment'ı AppointmetnVM'ye dönüştür
            //CreateMap<Appointment, AppointmentVM>();
            ////AppointmentVM'yi Appointment'a dönüştür
            //CreateMap<AppointmentVM, Appointment>();
            
            //ReverseMap sayesinde yukarıdaki 2 ayrı işlemi tek satırda yapmış oluyoruz.
            //Appointment AppointmentVM'ye dönüşebilir
            //AppointmentVM Appointmenta dönüşebilir
            CreateMap<AppointmentVM, Appointment>().ReverseMap();


        }
    }
}
