﻿using System.Globalization;
using System.Resources;

// Register the types in the Microsoft's default namespaces
using System.Windows;
using System.Windows.Markup;

[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "WpfLocalization")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2007/xaml/presentation", "WpfLocalization")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2008/xaml/presentation", "WpfLocalization")]

namespace WpfLocalization
{
    public static class LocalizationScope
    {
        #region Attached properties

        #region Culture

        /// <summary>
        /// The <see cref="CultureInfo"/> according to which values are formatted.
        /// </summary>
        /// <remarks>
        /// CAUTION! Setting this property does NOT automatically update localized values.
        /// Call <see cref="LocalizationScope.UpdateValues"/> for that purpose.
        /// </remarks>
        public static readonly DependencyProperty CultureProperty = DependencyProperty.RegisterAttached(
            "Culture",
            typeof(CultureInfo),
            typeof(LocalizationScope),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits
                )
            );

        public static CultureInfo GetCulture(DependencyObject obj)
        {
            return (CultureInfo)obj.GetValue(CultureProperty);
        }

        public static void SetCulture(DependencyObject obj, CultureInfo value)
        {
            obj.SetValue(CultureProperty, value);
        }

        #endregion

        #region UICulture

        /// <summary>
        /// The <see cref="CultureInfo"/> used to retrieve resources from <see cref="ResourceManager"/>.
        /// </summary>
        /// <remarks>
        /// CAUTION! Setting this property does NOT automatically update localized values.
        /// Call <see cref="LocalizationScope.UpdateValues"/> for that purpose.
        /// </remarks>
        public static readonly DependencyProperty UICultureProperty = DependencyProperty.RegisterAttached(
            "UICulture",
            typeof(CultureInfo),
            typeof(LocalizationScope),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits
                )
            );

        public static CultureInfo GetUICulture(DependencyObject obj)
        {
            return (CultureInfo)obj.GetValue(UICultureProperty);
        }

        public static void SetUICulture(DependencyObject obj, CultureInfo value)
        {
            obj.SetValue(UICultureProperty, value);
        }

        #endregion

        #region ResourceManager

        /// <summary>
        /// The resource manager to use to retrieve resources.
        /// </summary>
        /// <remarks>
        /// CAUTION! Setting this property does NOT automatically update localized values.
        /// Call <see cref="LocalizationScope.UpdateValues"/> for that purpose.
        /// </remarks>
        public static readonly DependencyProperty ResourceManagerProperty = DependencyProperty.RegisterAttached(
            "ResourceManager",
            typeof(ResourceManager),
            typeof(LocalizationScope),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits
                )
            );

        public static ResourceManager GetResourceManager(DependencyObject obj)
        {
            return (ResourceManager)obj.GetValue(ResourceManagerProperty);
        }

        public static void SetResourceManager(DependencyObject obj, ResourceManager value)
        {
            obj.SetValue(ResourceManagerProperty, value);
        }

        #endregion

        #endregion
    }
}
