using System.ComponentModel.DataAnnotations;

namespace Cr.UI.Data.Permission
{
	public class MenuUpdateRequestModel
	{
		[Required(ErrorMessage = "Id is required")]
		public long Id { get; set; }

		[Required(ErrorMessage = "Menu name is required")]
		public string? MenuName { get; set; }

		[Required(ErrorMessage = "Icon is required")]
		public string? MenuIcon { get; set; }

		[Required(ErrorMessage = "Parent Id is required")]
		public int ParentId { get; set; }

		[Required(ErrorMessage = "Sequence Id is required")]
		public int MenuSequence { get; set; }

		[Required(ErrorMessage = "IsVisible Id is required")]
		public bool IsVisible { get; set; }

		[Required(ErrorMessage = "Show In MenuItem Id is required")]
		public bool ShowInMenuItem { get; set; }
	}
}
