using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.DTOs.User.Response
{
    public class ProjectMatchResponseDto
    {
        public Guid ProjectId { get; set; }

        public string ProjectName { get; set; }

        public double Score { get; set; }

        public List<string> Reason { get; set; }
    }
}
