using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuantumConcepts.Common.DataObjects;
using QuantumConcepts.Common.Extensions;
using QuantumConcepts.Common.Web.Utils;

namespace QuantumConcepts.Common.Web.WebControls
{
    public class PageParameter<PageType, UserType>
        where PageType : BasePage<UserType>
        where UserType : IUser
    {
        public string Name { get; private set; }
        public bool Required { get; private set; }
        public Func<string, bool> Validator { get; private set; }
        public Action<string, PageType> Extractor { get; private set; }

        private PageParameter(string name, bool required, Func<string, bool> validator, Action<string, PageType> extractor)
        {
            this.Name = name;
            this.Required = required;
            this.Validator = validator;
            this.Extractor = extractor;
        }

        public static void Process(string name, bool required, Func<string, bool> validator, Action<string, PageType> extractor, HttpRequest request, PageType page)
        {
            (new PageParameter<PageType, UserType>(name, required, validator, extractor)).Extract(request, page);
        }

        public void Extract(HttpRequest request, PageType page)
        {
            string valueString = RequestUtil.GetParameter(HttpContext.Current, this.Name);

            if (string.IsNullOrEmpty(valueString))
            {
                if (this.Required)
                    throw new ApplicationException("The parameter \"{0}\" is required but was not supplied.".FormatString(this.Name));
            }
            else
            {
                if (this.Validator != null && !this.Validator(valueString))
                    throw new ApplicationException("The parameter \"{0}\" is not valid.".FormatString(this.Name));

                this.Extractor(valueString, page);
            }
        }
    }
}
