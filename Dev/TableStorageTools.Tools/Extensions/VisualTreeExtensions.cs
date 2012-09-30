using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TableStorageTools.Tools.Extensions
{
    public static class VisualTreeExtensions
    {
        public static DependencyObject FindAncestor(Type ancestorType, DependencyObject dependencyObject)
        {
            while (dependencyObject != null && !ancestorType.IsInstanceOfType(dependencyObject))
            {
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
            return dependencyObject;
        }

        public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject obj)
        {
            while (obj != null)
            {
                var parent = VisualTreeHelper.GetParent(obj);
                if (parent == null)
                {
                    var asfe = obj as FrameworkElement;
                    if (asfe != null)
                        parent = asfe.Parent;
                }
                if (parent == null)
                    yield break;
                yield return parent;
                obj = parent;
            }
        }

        public static List<T> GetChildrenByType<T>(this UIElement element)
        where T : UIElement
        {
            return GetChildrenByType<T>(element, null);
        }

        public static bool HasChildrenByType<T>(this UIElement element, Func<T, bool> condition)
                where T : UIElement
        {
            return GetChildrenByType<T>(element, condition).Count != 0;
        }

        public static List<T> GetChildrenByType<T>(this UIElement element, Func<T, bool> condition)
            where T : UIElement
        {
            List<T> results = new List<T>();
            GetChildrenByType<T>(element, condition, results);
            return results;
        }

        private static void GetChildrenByType<T>(UIElement element, Func<T, bool> condition, List<T> results)
            where T : UIElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                UIElement child = VisualTreeHelper.GetChild(element, i) as UIElement;
                if (child != null)
                {
                    T t = child as T;
                    if (t != null)
                    {
                        if (condition == null)
                            results.Add(t);
                        else if (condition(t))
                            results.Add(t);
                    }
                    GetChildrenByType<T>(child, condition, results);
                }
            }
        }

    }
}
