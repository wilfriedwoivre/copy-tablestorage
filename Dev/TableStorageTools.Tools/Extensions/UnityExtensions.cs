using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Unity;

namespace TableStorageTools.Tools.Extensions
{
    public static class UnityExtensions
    {
        public static IUnityContainer GetUnityContainer(this DependencyObject obj)
        {
            var container = (IUnityContainer)obj.GetValue(UnityContainerProperty);
            if (container == null)
            {
                var parent = VisualTreeHelper.GetParent(obj);
                container = parent.GetUnityContainer();
                obj.SetUnityContainer(container);
            }
            return container;
        }

        public static void SetUnityContainer(this DependencyObject obj, IUnityContainer value)
        {
            obj.SetValue(UnityContainerProperty, value);
        }

        // Using a DependencyProperty as the backing store for UnityContainer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UnityContainerProperty =
            DependencyProperty.RegisterAttached("UnityContainer", typeof(IUnityContainer), typeof(UnityExtensions), new PropertyMetadata(null));

        /// <summary>
        /// Ensure that the view has a viewmodel of type T
        /// </summary>
        /// <typeparam name="T">type of the view model</typeparam>
        /// <param name="view">view to check</param>
        public static T EnsureViewModel<T>(this FrameworkElement view) where T : class
        {
            if (view.DataContext is T)
                return view.DataContext as T;


            object vm = null;
            Type parentType = typeof(T);
            IUnityContainer viewFindUnityContainer = view.FindUnityContainer();

            if (!parentType.IsInterface)
            {
                //Checks if the viewModel is already registered ?
                if (!viewFindUnityContainer.IsRegistered(typeof(T)))
                {
                    //register the subclass as the viewModel in the unityContainer
                    viewFindUnityContainer.RegisterType(typeof(T));
                }
                vm = viewFindUnityContainer.Resolve(typeof(T));
            }
            else
            {
                vm = viewFindUnityContainer.Resolve<T>();
            }

            view.DataContext = vm;
            return vm as T;
        }

        /// <summary>
        /// Get or set the container associated to the specified element (walk to the ancestors if null)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IUnityContainer FindUnityContainer(this DependencyObject obj)
        {
            var local = GetUnityContainer(obj);
            if (local != null)
                return local;

            return obj.GetAncestors()
                .Select(GetUnityContainer)
                .Where(c => c != null)
                .FirstOrDefault();
        }
    }
}
