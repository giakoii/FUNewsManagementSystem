using DataAccessObject.Models;
using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Models.ViewModel
{
    public class EditNewsArticleVM
    {
        public string NewsArticleId { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(400, ErrorMessage = "Tiêu đề tối đa 400 ký tự")]
        public string NewsTitle { get; set; }

        [Required(ErrorMessage = "Headline không được để trống")]
        [StringLength(150, ErrorMessage = "Headline tối đa 150 ký tự")]
        public string Headline { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống")]
        [StringLength(4000, ErrorMessage = "Nội dung tối đa 4000 ký tự")]
        public string NewsContent { get; set; }

        [StringLength(400, ErrorMessage = "Nguồn tin tối đa 400 ký tự")]
        public string? NewsSource { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public short? CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ít nhất 1 tag")]
        public List<int> SelectedTagIds { get; set; } = new();

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
    }
}
