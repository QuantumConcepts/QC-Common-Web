﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuantumConcepts.Common.Web.Mvc.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TemplateVisibilityAttribute : Attribute, IMetadataAware
    {
        public bool ShowForDisplay { get; set; }
        public bool ShowForEdit { get; set; }

        public TemplateVisibilityAttribute()
        {
            this.ShowForDisplay = true;
            this.ShowForEdit = true;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            metadata.ShowForDisplay = this.ShowForDisplay;
            metadata.ShowForEdit = this.ShowForEdit;
        }
    }
}