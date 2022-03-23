using MHRSLiteBusinessLayer.Contracts;
using MHRSLiteDataLayer;
using MHRSLiteEntityLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteBusinessLayer.Implementations
{
    public class DenemeRepository:Repository<Deneme>, IDenemeRepository
    {
        public DenemeRepository(MyContext myContext): base(myContext)
        {

        }
    }
}
