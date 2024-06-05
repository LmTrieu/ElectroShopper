namespace RookieEShopper.Application.Dto.CategoryGroup
{
    public class UpdateCategoryListDto
    {
        public IList<int> CategoriesToAddId { get; set; }
        public IList<int> CategoriesToRemoveId { get; set; }
    }
}