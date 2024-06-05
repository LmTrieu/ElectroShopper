namespace RookieEShopper.Application.Dto.CategoryGroup
{
    public class CreateCategoryGroupDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<int>? CategoriesId { get; set; }
    }
}