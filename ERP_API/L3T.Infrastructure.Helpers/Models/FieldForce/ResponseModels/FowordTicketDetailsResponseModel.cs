using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels
{
    public class FowardTicketDetailsResponseModel
    {
        [Key]
        public string project_Id { get; set; }
        public string project_Title { get; set; }        
        public string client_CompanyName { get; set; }        
        public string project_Description { get; set; }
        public string project_category { get; set; }
        public string Team_id { get; set; }
    }
}
