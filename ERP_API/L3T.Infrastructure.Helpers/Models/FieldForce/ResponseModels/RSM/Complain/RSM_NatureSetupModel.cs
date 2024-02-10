using System.ComponentModel.DataAnnotations;


namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class RSM_NatureSetupModel
    {
        [Key]
        public int NatureID { get; set; }
        public string NatureName { get; set; }
    }
}
