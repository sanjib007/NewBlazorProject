namespace Cr.UI.Data.ChangeRequirementModel
{
    public class AddChangeReq
    {
        public long Id { get; set; }
        public string? Subject { get; set; }
        public string? ChangeRequestFor { get; set; }

        public string? AddReference { get; set; }
        public IFormFile? AttachFile { get; set; }

        public string? ChangeFromExisting { get; set; }
        public string? ChangeToAfter { get; set; }
        public string? Justification { get; set; }
        public string? ChangeImpactDescription { get; set; }

        public string? LevelOfRisk { get; set; }
        public string? LevelOfRiskDescription { get; set; }
        public string? AlternativeDescription { get; set; }

        
        public string? StepNo { get; set; }
     

    }
}
