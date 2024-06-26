﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using RookieEShopper.Application.Dto.CategoryGroup;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Application.Service;
using RookieEShopper.Domain.Data.Entities;
using RookieEShopper.SharedLibrary.HelperClasses;

namespace RookieEShopper.Infrastructure.Persistent.Repositories
{
    public class CategoryGroupRepository : ICategoryGroupRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public CategoryGroupRepository(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CategoryGroup> CreateCategoryGroupAsync(CreateCategoryGroupDto categoryGroupDto)
        {
            var categoryGroup = new CategoryGroup();
            _mapper.Map(categoryGroupDto, categoryGroup);

            categoryGroup.Categories = new List<Category>();

            ((List<Category>)categoryGroup.Categories)
                    .AddRange(_context.Categories
                        .Where(c => categoryGroupDto.CategoriesId
                            .Contains(c.Id))
                        .ToList());

            await _context.CategoryGroups.AddAsync(categoryGroup);

            await _context.SaveChangesAsync();
            return categoryGroup;
        }

        public Task<CategoryGroup> DeleteCategoryGroupAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<ResponseCategoryGroupDto>> GetCategoryGroupsAsync(QueryParameters query)
        {
            return await PagedList<ResponseCategoryGroupDto>.ToPagedList(_context.CategoryGroups
                .Include(cg => cg.Categories)
                .ProjectTo<ResponseCategoryGroupDto>(_mapper.ConfigurationProvider),
                    query.PageNumber,
                    query.PageSize);
        }

        public async Task<CategoryGroup?> UpdateCategoryListAsync(int id, UpdateCategoryListDto updateCategoryList)
        {
            var categoryGroup = await _context.CategoryGroups
                .Include(cg => cg.Categories)
                .Where(cg => cg.Id == id)
                .FirstOrDefaultAsync();

            if (categoryGroup is not null)
            {
                categoryGroup.Categories
                    .ToHashSet()
                    .RemoveWhere(c => updateCategoryList.CategoriesToRemoveId.Contains(c.Id));
                categoryGroup.Categories
                    .ToList()
                    .AddRange(await _context.Categories
                        .Where(c => updateCategoryList.CategoriesToAddId
                            .Contains(c.Id))
                            .ToListAsync());
            }

            await _context.SaveChangesAsync();
            return categoryGroup;
        }
    }
}