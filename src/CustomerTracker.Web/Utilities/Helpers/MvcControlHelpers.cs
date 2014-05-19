using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Script.Serialization;
using CustomerTracker.Web.Models.Attributes;

namespace CustomerTracker.Web.Utilities.Helpers
{
    public static class MvcControlHelpers
    { 
        #region Dropdown Helpers
        public static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
        {
            return EnumDropDownListFor<TModel, TEnum>(htmlHelper, expression, null);
        }

        /// <summary>
        /// Returns enum with bootstrapstyple
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            Type enumType = GetNonNullableModelType(metadata);

            Type baseEnumType = Enum.GetUnderlyingType(enumType);

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (FieldInfo field in enumType.GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public))
            {
                var hidden = field.GetCustomAttributes(true).OfType<HiddenItemAttribute>().Count();

                if (hidden > 0)
                    continue;

                string text = field.Name;

                string value = Convert.ChangeType(field.GetValue(null), baseEnumType).ToString();

                bool selected = field.GetValue(null).Equals(metadata.Model);

                foreach (var displayAttribute in field.GetCustomAttributes(true).OfType<DescriptionAttribute>())
                {

                    text = displayAttribute.Description;
                }

                items.Add(new SelectListItem()

                {

                    Text = text,

                    Value = value,

                    Selected = selected

                });

            }

            if (metadata.IsNullableValueType)
            {

                items.Insert(0, new SelectListItem { Text = "", Value = "" });

            }

            var selectTag = new TagBuilder("select");
            var inputName = GetInputName(expression);
            selectTag.Attributes.Add("id", inputName);
            selectTag.Attributes.Add("name", inputName);
            var attributes = htmlAttributes.ToDictionary();
            if (attributes != null && attributes.Count > 0)
            {
                foreach (var attribute in attributes)
                {
                    selectTag.Attributes.Add(attribute.Key, attribute.Value.ToString());
                }
            }
            foreach (var item in items)
            {
                var optionTag = new TagBuilder("option");                
                optionTag.Attributes.Add("value",item.Value);
                if (item.Selected)
                    optionTag.Attributes.Add("selected", "selected");
                optionTag.InnerHtml = item.Text;
                selectTag.InnerHtml += optionTag.ToString(TagRenderMode.Normal);
            }

            var result = selectTag.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(result);
            //return SelectExtensions.DropDownListFor(htmlHelper, expression, items, htmlAttributes);

        }

        private static string GetInputName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (expression.Body.NodeType == ExpressionType.Call)
            {
                var methodCallExpression = (MethodCallExpression) expression.Body;
                var name = GetInputName(methodCallExpression);
                return name.Substring(expression.Parameters[0].Name.Length + 1);

            }
            return expression.Body.ToString().Substring(expression.Parameters[0].Name.Length + 1);
        }

        private static string GetInputName(MethodCallExpression expression)
        {
            var methodCallExpression = expression.Object as MethodCallExpression;
            return methodCallExpression != null ? GetInputName(methodCallExpression) : expression.Object.ToString();
        }


        private static IDictionary<string, object> ToDictionary(this object data)
        {
            if (data == null) return null;
            const BindingFlags publicAttributes = BindingFlags.Public | BindingFlags.Instance;
            return data.GetType().GetProperties(publicAttributes).Where(property => property.CanRead).ToDictionary(property => property.Name, property => property.GetValue(data, null));
        }

        //public static MvcHtmlString DropdownToBootStrap()
        //{
        //    var editorLabel = new TagBuilder("div");
        //    editorLabel.AddCssClass("editor-label");
        //    editorLabel.InnerHtml += labelContent;


        //}





        #endregion

      
 
    }

    public static class ViewDataExtensions
    {
        public static IEnumerable<T> ViewData<T>(this HtmlHelper helper, string name)
        {
            if (helper.ViewData[name] != null)
            {
                return (IEnumerable<T>)helper.ViewData[name];
            }
            return new List<T>();
        }

        public static T ViewDataSingle<T>(this HtmlHelper helper, string name)
        {
            if (helper.ViewData[name] != null)
            {
                return (T)helper.ViewData[name];
            }
            return default(T);
        }

    }

    public static class UrlHelpers
    {

        public static string SiteRoot(HttpContextBase context)
        {
            return SiteRoot(context, true);
        }

        public static string SiteRoot(HttpContextBase context, bool usePort)
        {
            var Port = context.Request.ServerVariables["SERVER_PORT"];
            if (usePort)
            {
                if (Port == null || Port == "80" || Port == "443")
                    Port = "";
                else
                    Port = ":" + Port;
            }
            var Protocol = context.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            var appPath = context.Request.ApplicationPath;
            if (appPath == "/")
                appPath = "";

            var sOut = Protocol + context.Request.ServerVariables["SERVER_NAME"] + Port + appPath;
            return sOut;

        }

        public static string SiteRoot(this UrlHelper url)
        {
            return SiteRoot(url.RequestContext.HttpContext);
        }


        public static string SiteRoot(this ViewPage pg)
        {
            return SiteRoot(pg.ViewContext.HttpContext);
        }

        public static string SiteRoot(this ViewUserControl pg)
        {
            var vpage = pg.Page as ViewPage;
            return SiteRoot(vpage.ViewContext.HttpContext);
        }

        public static string SiteRoot(this ViewMasterPage pg)
        {
            return SiteRoot(pg.ViewContext.HttpContext);
        }

        public static string GetReturnUrl(HttpContextBase context)
        {
            var returnUrl = "";

            if (context.Request.QueryString["ReturnUrl"] != null)
            {
                returnUrl = context.Request.QueryString["ReturnUrl"];
            }

            return returnUrl;
        }

        public static string GetReturnUrl(this UrlHelper helper)
        {
            return GetReturnUrl(helper.RequestContext.HttpContext);
        }

        public static string GetReturnUrl(this ViewPage pg)
        {
            return GetReturnUrl(pg.ViewContext.HttpContext);
        }

        public static string GetReturnUrl(this ViewMasterPage pg)
        {
            return GetReturnUrl(pg.Page as ViewPage);
        }

        public static string GetReturnUrl(this ViewUserControl pg)
        {
            return GetReturnUrl(pg.Page as ViewPage);
        }

    }

    public static class TwitterBootstrapHelpers
    {
        public static MvcHtmlString TypeaheadFor<TModel, TValue>(
        this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TValue>> expression,
        IEnumerable<string> source,
        int items = 8)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (source == null)
                throw new ArgumentNullException("source");

            var jsonString = new JavaScriptSerializer().Serialize(source);

            return htmlHelper.TextBoxFor(
                expression,
                new
                {
                    autocomplete = "off",
                    data_provide = "typeahead",
                    data_items = items,
                    data_source = jsonString
                }
            );
        }
    }

    public static class ContollerExtensions
    {
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <returns>Result</returns>
        public static string RenderPartialViewToString(this Controller controller)
        {
            return RenderPartialViewToString(controller, null, null);
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <returns>Result</returns>
        public static string RenderPartialViewToString(this Controller controller, string viewName)
        {
            return RenderPartialViewToString(controller, viewName, null);
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        public static string RenderPartialViewToString(this Controller controller, object model)
        {
            return RenderPartialViewToString(controller, null, model);
        }
        /// <summary>
        /// Render partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>Result</returns>
        public static string RenderPartialViewToString(this Controller controller, string viewName, object model)
        {
            //Original source code: http://craftycodeblog.com/2010/05/15/asp-net-mvc-render-partial-view-to-string/
            if (string.IsNullOrEmpty(viewName))
                viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

            controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

    }

    public static class ModelStateHelper
    {
        public static IEnumerable Errors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return modelState.ToDictionary(kvp => kvp.Key,
                    kvp => kvp.Value.Errors
                                    .Select(e => e.ErrorMessage).ToArray())
                                    .Where(m => m.Value.Any());
            }
            return null;
        }
    }

}