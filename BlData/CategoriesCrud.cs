﻿using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BaseballUa.BlData
{
	public class CategoriesCrud : ICrud<Category>
	{
		private readonly BaseballUaDbContext _dbContext;

        public CategoriesCrud(BaseballUaDbContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        public void Add(Category item)
		{
            _dbContext.Categories.Add(item);
            _dbContext.SaveChanges();
		}

		public void Delete(Category item)
		{
			throw new NotImplementedException();
		}

		public Category Get(int itemId)
		{
			if (itemId == null) return null;
			var category = _dbContext.Categories.First(c => c.Id == itemId);

			return category;
		}

		public IEnumerable<Category> GetAll()
		{
			return _dbContext.Categories;
		}

		public void Update(Category item)
		{
			_dbContext.Categories.Update(item);
			_dbContext.SaveChanges();
		}

        public List<SelectListItem> GetSelectItemList()
        {
            var categoriesSL = _dbContext.Categories.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();

            return categoriesSL;
        }

		public IEnumerable<int> GetIds(List<string> shortnames)
		{
			//var result = _dbContext.Categories.Where(c => shortnames.Exists(s => s == c.ShortName)).Select(c => c.Id);
			var result = _dbContext.Categories.Where(c => shortnames.Any(s => s == c.ShortName)).Select(c => c.Id);
			return result;
		}

    }
}
