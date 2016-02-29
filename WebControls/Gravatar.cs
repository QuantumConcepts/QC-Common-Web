using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QuantumConcepts.Common.Extensions;

namespace QuantumConcepts.Common.Web.WebControls
{
    [DefaultProperty("Email")]
    [ToolboxData("<{0}:Gravatar runat=server></{0}:Gravatar>")]
    public class Gravatar : Image
    {
        public enum RatingType { G, PG, R, X }

        private string _email;
        private short _size;
        private RatingType _maxAllowedRating;
        private string _defaultImage;

        /// <summary>
        /// The Email for the user
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Email { get { return _email; } set { _email = value.ToLower(); } }

        /// <summary>
        /// Size of Gravatar image.  Must be between 1 and 512.
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("80")]
        public short Size { get { return _size; } set { _size = value; } }

        /// <summary>
        /// An optional "rating" parameter may follow with a value of [ G | PG | R | X ] that determines the highest rating (inclusive) that will be returned.
        /// </summary>
        public RatingType MaxAllowedRating { get { return _maxAllowedRating; } set { _maxAllowedRating = value; } }
        
        /// <summary>
        /// An optional "default" parameter may follow that specifies the full, URL encoded URL, protocol included, of a GIF, JPEG, or PNG image that should be returned if either the requested email address has no associated gravatar, or that gravatar has a rating higher than is allowed by the "rating" parameter.
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string DefaultImage { get { return _defaultImage; } set { _defaultImage = value; } }
        
        protected override void Render(HtmlTextWriter output)
        {
            const string imageUrl = "http://www.gravatar.com/avatar.php?gravatar_id={0}&rating={1}&size={2}&default={3}";

            string hashedEmail = null;

            // If it's not in the allowed range, throw an exception:
            if (Size < 1 || Size > 512)
                throw new ArgumentOutOfRangeException("Size");

            if( !string.IsNullOrEmpty( Email))
            {
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                UTF8Encoding encoder = new UTF8Encoding();
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(Email));
                StringBuilder sb = new StringBuilder(hashedBytes.Length * 2);

                for (int i = 0; i < hashedBytes.Length; i++)
                    sb.Append(hashedBytes[i].ToString("X2"));

                hashedEmail = sb.ToString().ToLower();
            }

            this.ImageUrl = imageUrl.FormatString(hashedEmail, _maxAllowedRating, _size, (_defaultImage ?? ""));

            base.Render(output);
        }
    }
}
