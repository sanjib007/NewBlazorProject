using System.ComponentModel.DataAnnotations;

namespace Cr.UI.Data.Permission
{
	public class PermissionUpdateRequestModel
	{
		[Required(ErrorMessage = "Id is required")]
		public long Id { get; set; }

		[Required(ErrorMessage = "Feature Name is required")]
		public string? FeatureName { get; set; }

		[Required(ErrorMessage = "IsVisible is required")]
		public bool IsVisible { get; set; }

		[Required(ErrorMessage = "Allow Anonymous is required")]
		public bool AllowAnonymous { get; set; }
	}
}
