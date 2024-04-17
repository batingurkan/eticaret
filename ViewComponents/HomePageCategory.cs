using AutoMapper;
using eTicaretDAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class HomePageCategory:ViewComponent
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public HomePageCategory(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list =await _dataContext.Category.ToListAsync();
       
            return View(list);
        }
    
    }
