using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuantumConcepts.Common.Utils;
using System.Web.SessionState;
using QuantumConcepts.Common.Web.WebControls;
using System.Security.Principal;
using QuantumConcepts.Common.Exceptions;
using QuantumConcepts.Common.Extensions;
using QuantumConcepts.Common.DataObjects;
using QuantumConcepts.Common.Security;

namespace QuantumConcepts.Common.Web.Utils
{
    public class SessionUtil
    {
        private const string Key_User = "User";
        private const string Session_PageAlerts = "PageAlerts";
        private const string Session_ActiveExport = "ActiveExport";
        private const string Session_AccessibleVersion = "AccessibleVersion";

        public static SessionUtil Instance { get; private set; }

        private HttpContext _context;

        protected HttpContext Context { get { return (_context ?? HttpContext.Current); } }

        static SessionUtil()
        {
            SessionUtil.Instance = new SessionUtil();
        }

        protected SessionUtil() { }

        public SessionUtil(HttpContext context)
        {
            _context = context;
        }

        public virtual IUserPrincipal UserPrincipal
        {
            get
            {
                IUserPrincipal userPrincipal = this.Get<IUserPrincipal>(Key_User);

                if (userPrincipal == null && ConfigUtil.Instance.EnableAuthenticationCookie)
                {
                    string cookie = CookieUtil.Instance.AuthenticationTicket;

                    if (cookie != null)
                    {
                        string[] userData = cookie.Split(':');

                        if (userData.Length == 2)
                        {
                            string userIDString = userData[0];
                            string passwordEncrypted = userData[1];
                            int userID;

                            if (!passwordEncrypted.IsNullOrEmpty() && int.TryParse(userIDString, out userID))
                            {
                                IUser user = GetUser(userID);

                                if (user != null && !user.PasswordEncrypted.IsNullOrEmpty() && string.Equals(Convert.ToBase64String(user.PasswordEncrypted), passwordEncrypted))
                                {
                                    try
                                    {
                                        userPrincipal = CreatePrincipal(user);
                                    }
                                    catch { }
                                }
                            }
                        }
                    }
                }

                if (userPrincipal != null)
                {
                    userPrincipal.UserIdentity.Refresh();
                    this.Set(Key_User, userPrincipal);
                }

                return userPrincipal;
            }
            set
            {
                IUserPrincipal userPrincipal = value;

                CookieUtil.Instance.AuthenticationTicket = null;

                if (ConfigUtil.Instance.EnableAuthenticationCookie && userPrincipal != null)
                    CookieUtil.Instance.AuthenticationTicket = "{0}:{1}".FormatString(userPrincipal.UserIdentity.User.ID, Convert.ToBase64String(userPrincipal.UserIdentity.User.PasswordEncrypted));

                this.Set(Key_User, value);
            }
        }

        public virtual IUser GetUser(object id)
        {
            throw new NotImplementedException();
        }

        public virtual IUserPrincipal CreatePrincipal(IUser user)
        {
            throw new NotImplementedException();
        }

        public bool AccessibleVersion { get { return Get<bool>(Session_AccessibleVersion, false); } set { Set(Session_AccessibleVersion, value); } }

        public List<PageAlert> PageAlerts
        {
            get { return (Get(Session_PageAlerts) as List<PageAlert>); }
            set { Set(Session_PageAlerts, value); }
        }

        public void AddPageAlert(PageAlert pageAlert)
        {
            List<PageAlert> pageAlerts = PageAlerts;

            if (pageAlerts == null)
                pageAlerts = new List<PageAlert>();

            pageAlerts.Add(pageAlert);

            PageAlerts = pageAlerts;
        }

        public void ClearPageAlerts()
        {
            PageAlerts = null;
        }

        protected object Get(string name)
        {
            if (this.Context.Session != null)
                return this.Context.Session[name];

            return null;
        }

        protected T Get<T>(string name) where T : class {
            return Get<T>(name, () => null);
        }

        protected T Get<T>(string name, T defaultValue) {
            return Get<T>(name, () => defaultValue);
        }

        protected T Get<T>(string name, Func<T> defaultValueSelector)
        {
            if (this.Context != null && this.Context.Session != null)
            {
                object value = Get(name);

                if (value == null || !typeof(T).IsAssignableFrom(value.GetType())) {
                    T defaultValue = defaultValueSelector();

                    Set(name, defaultValue);

                    return defaultValue;
                }

                return (T)value;
            }
            else
                return default(T);
        }

        protected void Set(string name, object value)
        {
            if (this.Context == null || this.Context.Session == null)
                throw new InvalidOperationException("Cannot alter session before it is created.");

            this.Context.Session[name] = value;
        }

        public virtual void Abandon()
        {
            this.Context.User = null;
            this.Context.Session.Clear();
            this.Context.Session.Abandon();
        }
    }
}
