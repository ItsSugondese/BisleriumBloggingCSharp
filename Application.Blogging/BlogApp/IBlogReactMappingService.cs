﻿using Domain.Blogging.view.BLogView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Blogging.BlogApp
{
    public interface IBlogReactMappingService
    {
        Task SaveBlogReaction(BlogReactMappingViewModel model);
    }
}
