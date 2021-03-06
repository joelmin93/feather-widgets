﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Frontend.Media.Mvc.Controllers;
using Telerik.Sitefinity.Frontend.Media.Mvc.Models.ImageGallery;

namespace FeatherWidgets.TestUnit.DummyClasses.Media
{
    public class DummyImageGalleryController : ImageGalleryController
    {
        private readonly IImageGalleryModel model;

        public DummyImageGalleryController(IImageGalleryModel model)
        {
            this.model = model;
        }

        public override IImageGalleryModel Model
        {
            get
            {
                return this.model ?? base.Model;
            }
        }

        protected override string GetQueryString(string key)
        {
            return string.Empty;
        }

        protected override void AddCanonicalUrlTag(Telerik.Sitefinity.Model.IDataItem item)
        {
        }
    }
}
