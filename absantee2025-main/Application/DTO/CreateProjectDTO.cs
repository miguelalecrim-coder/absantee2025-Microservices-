using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.DTO
{
    public record CreateProjectDTO
    {
        public string Title { get; set; }
        public string Acronym { get; set; }
        public PeriodDate PeriodDate { get; set; }


        public CreateProjectDTO(string title, string acronym, PeriodDate periodDate)
        {
            Title = title;
            Acronym = acronym;
            PeriodDate = periodDate;
        }
    }



}
