var Global =
{
    Initialize: function ()
    {
        Global.ScrollHelperManager.Initialize();
        Global.JQuery.Initialize();

        //Re-initialize once an AJAX request completes.
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (sender, args)
        {
            Global.ReInitialize();
        });
    },
    ReInitialize: function ()
    {
        Global.JQuery.Initialize();
    },
    Browser:
    {
        IsFirefox: function ()
        {
            return (navigator.userAgent.toLowerCase().indexOf("firefox") >= 0);
        },
        IsInternetExplorer: function ()
        {
            return (navigator.userAgent.toLowerCase().indexOf("msie") >= 0);
        },
        Version: function ()
        {
            var match = null;

            if (this.IsInternetExplorer())
                match = /MSIE ([\d+](?:\.[\d+]))?/.exec(navigator.appVersion);
            else
                match = /([\d+](?:\.[\d+]))?/.exec(navigator.appVersion);

            return match[1];
        }
    },
    EventManager:
    {
        AddEvent: function (element, type, func)
        {
            if (element.attachEvent)
            {
                element["e" + type + func] = func;
                element[type + func] = function ()
                {
                    element["e" + type + func](window.event);
                };
                element.attachEvent("on" + type, element[type + func]);
            }
            else
                element.addEventListener(type, func, false);
        },
        RemoveEvent: function (element, type, func)
        {
            if (element.detachEvent)
            {
                if (element[type + func] == null)
                    return;

                element.detachEvent("on" + type, element[type + func]);
                element[type + func] = null;
            }
            else
                element.removeEventListener(type, func, false);
        }
    },
    PopupManager:
    {
        Popups: new Array(),
        Create: function (parentLink, popupElement, horizontalAlignment, verticalAlignment, usePositioning, onShow, onHide)
        {
            var popup = new Popup(parentLink, popupElement, horizontalAlignment, verticalAlignment, usePositioning, onShow, onHide);

            this.Popups[this.Popups.length] = popup;

            return popup;
        },
        GetByParentLink: function (parentLink)
        {
            for (var i = 0; i < this.Popups.length; i++)
            {
                if (this.Popups[i].ParentLink == parentLink)
                {
                    return this.Popups[i];
                }
            }

            return null;
        },
        HideAll: function ()
        {
            for (var i = 0; i < this.Popups.length; i++)
            {
                this.Popups[i].Hide();
            }
        }
    },
    GetLeftOffset: function (element, usePositioning)
    {
        var offset = element.offsetLeft;

        while ((element = element.offsetParent) != null && (Global.Browser.IsFirefox() || usePositioning || element.style.position == "fixed"))
            offset += element.offsetLeft;

        return offset;
    },
    GetTopOffset: function (element, usePositioning)
    {
        var offset = element.offsetTop;

        while ((element = element.offsetParent) != null && (Global.Browser.IsFirefox() || usePositioning || element.style.position == "fixed"))
            offset += element.offsetTop;

        return offset;
    },
    GetVerticalScroll: function ()
    {
        var scroll = 0;

        if (window.pageYOffset != null)
            scroll = window.pageYOffset;
        else if (document.documentElement != null && document.documentElement.scrollTop != null)
            scroll = document.documentElement.scrollTop;
        else if (document.body != null && document.body.scrollTop != null)
            scroll = document.body.scrollTop;

        return scroll;
    },
    GetHorizontalScroll: function ()
    {
        var scroll = 0;

        if (window.pageXOffset != null)
            scroll = window.pageXOffset;
        else if (document.documentElement != null && document.documentElement.scrollLeft != null)
            scroll = document.documentElement.scrollLeft;
        else if (document.body != null && document.body.scrollLeft != null)
            scroll = document.body.scrollLeft;

        return scroll;
    },
    ScrollHelperManager:
    {
        Initialize: function ()
        {
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function (sender, args)
            {
                for (var i = 0; i < Global.ScrollHelperManager.ScrollHelpers.length; i++)
                    Global.ScrollHelperManager.ScrollHelpers[i].UpdatePosition();
            });

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (sender, args)
            {
                for (var i = 0; i < Global.ScrollHelperManager.ScrollHelpers.length; i++)
                    Global.ScrollHelperManager.ScrollHelpers[i].SetPosition();
            });
        },
        ScrollHelpers: new Array(),
        Add: function (targetControlID)
        {
            var exists = false;

            for (var i = 0; i < this.ScrollHelpers.length; i++)
            {
                if (this.ScrollHelpers[i].TargetElementID == targetControlID)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
                this.ScrollHelpers[this.ScrollHelpers.length] = new ScrollHelper(targetControlID);
        }
    },
    JQuery:
    {
        Initialize: function ()
        {
            $(".AnimatedImageButton").hover(function ()
            {
                $(".Image", this).animate({ opacity: "show", right: 0, top: 0 }, "fast");
            }, function ()
            {
                $(".Image", this).animate({ opacity: "hide", right: -4, top: -4 }, "fast");
            });

            $(".MouseOverImage").hover(function ()
            {
                this.src = this.attributes["MouseOverImageSrc"].value;
            }, function ()
            {
                this.src = this.attributes["MouseOffImageSrc"].value;
            });

            $(".CollapsiblePanel img.Toggle").unbind("click");
            $(".CollapsiblePanel img.Toggle").click(function ()
            {
                var panel = document.getElementById(this.attributes["TargetControlID"].value);
                var expandedHidden = document.getElementById(this.attributes["ExpandedHiddenControlID"].value);

                if (Boolean.parse(expandedHidden.value.toString()))
                {
                    panel.style.display = "none";
                    this.src = this.attributes["ExpandImageUrl"].value;
                    expandedHidden.value = false;
                }
                else
                {
                    panel.style.display = "";
                    this.src = this.attributes["CollapseImageUrl"].value;
                    expandedHidden.value = true;

                    if (Boolean.parse(this.attributes["LazyLoad"].value))
                    {
                        var loadedHidden = document.getElementById(this.attributes["LoadedHiddenControlID"].value);
                        var clientID = this.attributes["ClientID"].value;

                        loadedHidden.value = true;
                        __doPostBack(clientID, "Click");
                    }
                }
            });

            $("input[type='image'][disabled=true]").each(function ()
            {
                this.style.opacity = .50;
                this.style.filter = "alpha(opacity=50)";
                this.style.cursor = "default";
            });

            SetDropDownExpanders();

            $("iframe.AutoSize").load(function ()
            {
                Global.JQuery.AutoSizeIFrames();
            });

            if (Global.JQuery.Initialize_Custom != null)
                Global.JQuery.Initialize_Custom();
        },
        AutoSizeIFrames: function ()
        {
            $("iframe.AutoSize").height(function ()
            {
                var iframe = $(this);

                iframe.height($(iframe[0].contentDocument).height());
            });
        }
    },
    Validation:
    {
        FixCalloutExtender: function (obj)
        {
            alert(typeof (obj));
        }
    }
};

function Popup(parentLink, popupElement, horizontalAlignment, verticalAlignment, usePositioning, onShow, onHide)
{
    this.ParentLink = parentLink;
    this.PopupElement = popupElement;
    this.HorizontalAlignment = horizontalAlignment;
    this.VerticalAlignment = verticalAlignment;
    this.UsePositioning = usePositioning;
    this.AutoHideTimeout = 500;
    this.AutoHideTimer = null;
    this.OnShow = onShow;
    this.OnHide = onHide;

    this.Show = function()
    {
        //Default to "left-bottom" alignment.
        var left = 0;
        var top = 0;

        this.CancelAutoHide();
        Global.PopupManager.HideAll();

        //Show the element first otherwise offsetWidth is 0.
        this.PopupElement.style.display = "";

        switch (this.HorizontalAlignment)
        {
            case "Right":
                {
                    left = (Global.GetLeftOffset(this.ParentLink, usePositioning) - this.PopupElement.offsetWidth + this.ParentLink.offsetWidth);
                    break;
                }
            default:
                left = Global.GetLeftOffset(this.ParentLink, usePositioning);
        }

        switch (this.VerticalAlignment)
        {
            case "Top":
                {
                    top = (Global.GetTopOffset(this.ParentLink, usePositioning) - this.PopupElement.offsetHeight);
                    break;
                }
            default:
                top = (Global.GetTopOffset(this.ParentLink, usePositioning) + this.ParentLink.offsetHeight);
        }
        
        this.PopupElement.style.left = (left + "px");
        this.PopupElement.style.top = (top + "px");

        if (this.OnShow != null)
            this.OnShow();
    };
    this.Hide = function()
    {
        this.CancelAutoHide();
        this.PopupElement.style.display = "none";

        if (this.OnHide != null)
            this.OnHide();
    };
    this.Toggle = function()
    {
        if (this.PopupElement.style.display == "none")
            this.Show();
        else
            this.Hide();
    };
    this.AutoHide = function()
    {
        this.AutoHideTimer = setTimeout("document.getElementById(\"" + this.PopupElement.id + "\").Popup.Hide();", this.AutoHideTimeout);
    };
    this.CancelAutoHide = function()
    {
        if (this.AutoHideTimer != null)
            clearTimeout(this.AutoHideTimer);
    };
    this.Initialize = function()
    {
        var linkMouseoverFunction = function(e) { this.Popup.Show(); };
        var linkMouseoutFunction = function(e) { this.Popup.AutoHide(); };
        var popupMouseoverFunction = function(e) { this.Popup.CancelAutoHide(); };
        var popupMouseoutFunction = linkMouseoutFunction;

        //Initialize the sub menu.
        this.ParentLink.Popup = this;
        this.PopupElement.Popup = this;

        //Remove events first.
        Global.EventManager.RemoveEvent(this.ParentLink, "mouseover", linkMouseoverFunction);
        Global.EventManager.RemoveEvent(this.ParentLink, "mouseout", linkMouseoutFunction);
        Global.EventManager.RemoveEvent(this.PopupElement, "mouseover", popupMouseoverFunction);
        Global.EventManager.RemoveEvent(this.PopupElement, "mouseout", popupMouseoutFunction);

        //Add events.
        Global.EventManager.AddEvent(this.ParentLink, "mouseover", linkMouseoverFunction);
        Global.EventManager.AddEvent(this.ParentLink, "mouseout", linkMouseoutFunction);
        Global.EventManager.AddEvent(this.PopupElement, "mouseover", popupMouseoverFunction);
        Global.EventManager.AddEvent(this.PopupElement, "mouseout", popupMouseoutFunction);
    };
    
    this.Initialize();
    return true;
}

function SelectButton(elementID, selectButtonID) 
{
    this.Element = document.getElementById(elementID);
    this.Button = document.getElementById(selectButtonID);
    this.RegisterEvents = function () 
    {
        Global.EventManager.AddEvent(this.Element, "mouseover", function () 
        {
            this.SelectButton.Button.style.display = "";
        });

        Global.EventManager.AddEvent(this.Element, "mouseout", function () 
        {
            this.SelectButton.Button.style.display = "none";
        });
    };

    this.Initialize = function () 
    {
        //Initialization.
        if (this.Element == null)
            throw "Element with ID \"" + elementID + "\" not found.";

        if (this.Button == null)
            throw "Element with ID \"" + selectButtonID + "\" not found.";

        //Store this object on the element.
        this.Element.SelectButton = this;

        //Configure the events.
        this.RegisterEvents();
    };

    this.Initialize();
    return true;
}

function SelectOrDeselectButton(elementID, selectButtonID, deselectButtonID, selectable, selected)
{
    this.Element = document.getElementById(elementID);
    this.SelectButton = document.getElementById(selectButtonID);
    this.DeselectButton = document.getElementById(deselectButtonID);
    this.Selectable = selectable;
    this.Selected = selected;
    this.RegisterEvents = function ()
    {
        if (!this.Selectable)
            return;

        Global.EventManager.AddEvent(this.Element, "mouseover", function ()
        {
            var elementToShow = (this.SelectOrDeselectButton.Selected ? this.SelectOrDeselectButton.DeselectButton : this.SelectOrDeselectButton.SelectButton);

            if (elementToShow != this.SelectOrDeselectButton.DeselectButton && this.SelectOrDeselectButton.DeselectButton != null)
                this.SelectOrDeselectButton.DeselectButton.style.display = "none";

            if (elementToShow != this.SelectOrDeselectButton.SelectButton && this.SelectOrDeselectButton.SelectButton != null)
                this.SelectOrDeselectButton.SelectButton.style.display = "none";

            if (elementToShow != null)
                elementToShow.style.display = "";
        });
        Global.EventManager.AddEvent(this.Element, "mouseout", function ()
        {
            if (this.SelectOrDeselectButton.DeselectButton != null)
                this.SelectOrDeselectButton.DeselectButton.style.display = "none";

            if (this.SelectOrDeselectButton.SelectButton != null)
            {
                if (this.SelectOrDeselectButton.Selected)
                    this.SelectOrDeselectButton.SelectButton.style.display = "";
                else
                    this.SelectOrDeselectButton.SelectButton.style.display = "none";
            }
        });
    };
    this.Initialize = function ()
    {
        //Initialization.
        if (this.Element == null)
            throw "Element with ID \"" + elementID + "\" not found.";

        if (this.SelectButton == null && this.Selectable && !this.Selected)
            throw "Element with ID \"" + selectButtonID + "\" not found.";

        if (this.DeselectButton == null && this.Selectable && this.Selected)
            throw "Element with ID \"" + deselectButtonID + "\" not found.";

        //Store this object on the element.
        this.Element.SelectOrDeselectButton = this;

        //Configure the events.
        this.RegisterEvents();

        if (this.Selected)
            this.SelectButton.style.display = "";
    };

    this.Initialize();
    return true;
}

function ScrollHelper(targetControlID)
{
    this.TargetElementID = targetControlID;
    this.ScrollPositionX = 0;
    this.ScrollPositionY = 0;
    this.UpdatePosition = function ()
    {
        var element = document.getElementById(this.TargetElementID);

        if (element == null)
            return;

        this.ScrollPositionX = element.scrollLeft;
        this.ScrollPositionY = element.scrollTop;
    };
    this.SetPosition = function ()
    {
        var element = document.getElementById(this.TargetElementID);

        if (element == null)
            return;

        element.scrollLeft = this.ScrollPositionX;
        element.scrollTop = this.ScrollPositionY;
    };

    return true;
}

function ToggleDisplay(element, makeVisible, display)
{
    if (element == null)
        return;

    if (makeVisible == null)
        makeVisible = (element.style.display == "none");

    if (display == null)
        display = "";

    if (makeVisible)
        element.style.display = display;
    else
        element.style.display = "none";
}

function ClientRedirect(url)
{
    document.location = url;
}

function SetDropDownExpanders()
{
    //"Unbind" the handlers before setting them.
    $('.StandardDropDownButton').unbind();
    $('.StandardDropDownButton').click(function ()
    {
        DropDownExpand(this);
    });

    $('.StandardDropDownButton a').not('li a').removeAttr("href");
}

function DropDownExpand(object)
{
    if ($(object).is('.Expanded'))
        HideButtonExpanders();
    else
    {
        HideButtonExpanders();

        $(object).toggleClass('Expanded');
    }
}

function HideButtonExpanders()
{
    $('.StandardDropDownButton').removeClass('Expanded');
}