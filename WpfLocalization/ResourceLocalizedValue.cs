using System;
using System.Drawing;
using System.IO;
using System.Globalization;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;

namespace WpfLocalization
{
    /// <summary>
    /// Retrieves a localized value from resources.
    /// </summary>
    public class ResourceLocalizedValue : LocalizedValue
    {
        string _resourceKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceLocalizedValue"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="resourceKey">The resource key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="property"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="resourceKey"/> is null or empty.</exception>
        public ResourceLocalizedValue(LocalizedProperty property, string resourceKey)
            : base(property)
        {
            if (string.IsNullOrEmpty(resourceKey))
            {
                throw new ArgumentNullException("resourceKey");
            }

            _resourceKey = resourceKey;
        }

        /// <summary>
        /// Retrieves the localized value from resources or by other means.
        /// </summary>
        /// <returns>
        /// The localized value.
        /// </returns>
        protected override object GetLocalizedValue()
        {
            var resourceManager = Property.GetResourceManager();

            if (resourceManager == null)
            {
                return GetFallbackValue();
            }

            var uiCulture = Property.GetUICulture();

            var value = resourceManager.GetObject(_resourceKey, uiCulture);

            if (value == null)
            {
                return GetFallbackValue();
            }
            if (Property.Converter != null)
            {
                return value;
            }
            return ChangeValueType(Property.GetValueType(), value);
        }

        /// <summary>
        /// Returns a value when a resource is not found.
        /// </summary>
        /// <returns>
        /// "[ResourceKey]" if the property is of type <see cref="String"/>. Otherwise, <c>null</c>.
        /// </returns>
        object GetFallbackValue()
        {
            if (Property.GetValueType() == typeof(string) || Property.GetValueType() == typeof(object))
            {
                return "[" + _resourceKey + "]";
            }
            return null;
        }

        /// <summary>
        /// Attempts to change the type of a loaded resource to the type of the property.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>
        /// Supports the following property types: images, icons, text, enumerations, numbers, boolean,
        /// <see cref="DateTime"/> and a <see cref="TypeConverter"/> if one is defined for the
        /// property's type.
        /// </remarks>
        object ChangeValueType(Type type, object value)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type == typeof(object))
            {
                return value;
            }
            if (type == value.GetType() || type.IsAssignableFrom(value.GetType()))
            {
                return value;
            }
            if (type == typeof(ImageSource))
            {
                BitmapSource result;

                var bitmap1 = value as Bitmap;
                if (bitmap1 != null)
                {
                    using (bitmap1)
                    {
                        result = Imaging.CreateBitmapSourceFromHBitmap(bitmap1.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }
                }
                else
                {
                    var icon = value as Icon;
                    if (icon != null)
                    {
                        using (icon)
                        {
                            using (var bitmap = icon.ToBitmap())
                            {
                                result = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                            }
                        }
                    }
                    else
                    {
                        var bytes = value as byte[];
                        if (bytes != null)
                        {
                            using (var memoryStream = new MemoryStream(bytes, false))
                            {
                                result = BitmapFrame.Create(memoryStream);
                            }
                        }
                        else
                        {
                            // Return the value as is

                            return value;
                        }
                    }
                }

                result.Freeze();

                return result;
            }
            if (type.IsEnum && value is string)
            {
                return Enum.Parse(type, (string)value);
            }
            if (value is IConvertible && (type.IsPrimitive || type == typeof(DateTime)))
            {
                return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
            }
            var converter = TypeDescriptor.GetConverter(type);

            if (converter == null || converter.GetType() == typeof(TypeConverter))
            {
                // No converter was found or the default converter was returned
                // (the default converter is unusable)

                if (Property.IsInDesignMode)
                {
                    // VS fails to load some converters in design mode

                    if (type == typeof(Brush))
                    {
                        converter = new BrushConverter();

                        return converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
                    }
                }

                return value;
            }
            return converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
        }
    }
}
