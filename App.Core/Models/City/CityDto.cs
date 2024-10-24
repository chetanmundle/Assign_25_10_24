using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Models.City
{
    public class CityDto
    {
        public int CityId { get; set; }

        public string CityName { get; set; }

        public int StateId { get; set; }

    }
}
