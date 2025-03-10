using DataAccessObject.Models;
using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Models.ViewModel
{
    public class CreateNewsArticleVM
    {
        [Required(ErrorMessage = "News Title is required")]
        [StringLength(400, ErrorMessage = "Max 400 characters")]
        public string NewsTitle { get; set; }

        [Required(ErrorMessage = "Headline is required")]
        [StringLength(150, ErrorMessage = "Max 150 characters")]
        public string Headline { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [StringLength(4000, ErrorMessage = "Max 4000 characters")]
        public string NewsContent { get; set; }

        [StringLength(400, ErrorMessage = "Max 400 characters")]
        public string? NewsSource { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public short? CategoryId { get; set; }

        public List<int> SelectedTagIds { get; set; } = new();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();

    }
}
