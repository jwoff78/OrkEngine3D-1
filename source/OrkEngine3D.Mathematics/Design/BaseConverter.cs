﻿/*
* Copyright (c) 2007-2010 SlimDX Group
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;

namespace OrkEngine3D.Mathematics.Design;

/// <summary>
/// Provides a base class for mathematical type converters.
/// </summary>
public abstract class BaseConverter : ExpandableObjectConverter
{
    /// <summary>
    /// Gets or sets the collection of exposed properties.
    /// </summary>
    /// <value>The collection of exposed properties.</value>
    protected PropertyDescriptorCollection Properties
    {
        get;
        set;
    }

    internal static string ConvertFromValues<T>(ITypeDescriptorContext context, CultureInfo culture, T[] values)
    {
        if (culture == null)
            culture = CultureInfo.CurrentCulture;

        var converter = TypeDescriptor.GetConverter(typeof(T));
        var results = Array.ConvertAll(values, t => converter.ConvertToString(context, culture, t));

        return string.Join(culture.TextInfo.ListSeparator + " ", results);
    }

    internal static T[] ConvertToValues<T>(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        var str = value as string;
        if (string.IsNullOrEmpty(str))
            return null;

        if (culture == null)
            culture = CultureInfo.CurrentCulture;

        var converter = TypeDescriptor.GetConverter(typeof(T));
        var strings = str.Trim().Split(new[] { culture.TextInfo.ListSeparator }, StringSplitOptions.RemoveEmptyEntries);

        return Array.ConvertAll(strings, s => (T)converter.ConvertFromString(context, culture, s));
    }

    /// <summary>
    /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
    /// <param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert from.</param>
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    /// <summary>
    /// Returns whether this converter can convert the object to the specified type, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
    /// <param name="destinationType">A <see cref="T:System.Type"/> that represents the type you want to convert to.</param>
    /// <returns>
    /// true if this converter can perform the conversion; otherwise, false.
    /// </returns>
    public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    {
        return destinationType == typeof(string) || destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
    }

    /// <summary>
    /// Returns whether changing a value on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"/> to create a new value, using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
    /// <returns>
    /// true if changing a property on this object requires a call to <see cref="M:System.ComponentModel.TypeConverter.CreateInstance(System.Collections.IDictionary)"/> to create a new value; otherwise, false.
    /// </returns>
    public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
    {
        return true;
    }

    /// <summary>
    /// Gets a value indicating whether this object supports properties using the specified context.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
    /// <returns>
    /// true because <see cref="M:System.ComponentModel.TypeConverter.GetProperties(System.Object)"/> should be called to find the properties of this object. This method never returns false.
    /// </returns>
    public override bool GetPropertiesSupported(ITypeDescriptorContext context)
    {
        return true;
    }

    /// <summary>
    /// Gets a collection of properties for the type of object specified by the value parameter.
    /// </summary>
    /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
    /// <param name="value">An <see cref="T:System.Object"/> that specifies the type of object to get the properties for.</param>
    /// <param name="attributes">An array of type <see cref="T:System.Attribute"/> that will be used as a filter.</param>
    /// <returns>
    /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"/> with the properties that are exposed for the component, or null if there are no properties.
    /// </returns>
    public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
    {
        return Properties;
    }
}
