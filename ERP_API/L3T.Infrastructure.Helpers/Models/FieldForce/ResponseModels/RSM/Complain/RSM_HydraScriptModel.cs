using System.ComponentModel.DataAnnotations;


namespace L3T.Infrastructure.Helpers.Models.FieldForce.ResponseModels.RSM.Complain
{
    public class RSM_HydraScriptModel
    {
        [Key]
        public int ID { get; set; }
        public string SQLText { get; set; }
        public string SqlType { get; set; }
    }
}
