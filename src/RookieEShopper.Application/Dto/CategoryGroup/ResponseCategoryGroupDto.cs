using RookieEShopper.Application.Dto.Category;

namespace RookieEShopper.Application.Dto.CategoryGroup
{
    public class ResponseCategoryGroupDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public IList<ResponseCategoryDto>? categories { get; set; }
    }
}