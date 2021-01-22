using System;
using System.Linq;
using System.Collections.Generic;
using Xceed.Words.NET;
using System.Reflection;

namespace LabCMS.TestReportDomain.Shared
{
    public class BindingMapper
    {
        private readonly DocXService _docXService;
        private readonly TokenDecorator _tokenDecorator;
        public BindingMapper(
            DocXService? docXService=null,
            TokenDecorator? tokenDecorator=null)
        {
            _docXService = docXService??new(); 
            _tokenDecorator = tokenDecorator??new();
        }
        
        public IEnumerable<Binding> ParseDataContext<TDataContext>(TDataContext dataContext,string dateTimeFormat="yyyy-MM-dd",string? prefix=null)
        {
            foreach(PropertyInfo propertyInfo in typeof(TDataContext).GetProperties())
            {
                if (propertyInfo.GetCustomAttribute<IgnoreInMappingAttribute>() is null)
                {
                    dynamic? value = propertyInfo.GetValue(dataContext);
                    
                    string? stringFormat = propertyInfo.GetCustomAttribute<StringFormatAttribute>()?.Format;
                    string currentDateTimeFormat = stringFormat??dateTimeFormat;
                    string? content = value switch
                    {
                        string contentString => contentString,
                        DateTimeOffset dateTimeOffset => dateTimeOffset.ToString(currentDateTimeFormat),
                        DateTime dateTime => dateTime.ToString(currentDateTimeFormat),
                        _ when propertyInfo.PropertyType.IsPrimitive=> value?.ToString(stringFormat),
                        _ => null
                    };
                    if ((content is null) && (value is not Delegate))
                    {
                        foreach(var pair in ParseDataContext(value,dateTimeFormat,propertyInfo.Name))
                        { yield return pair;}
                    }else{
                        string bindingPath = _tokenDecorator.GetBindingPath(propertyInfo,prefix);
                        yield return new Binding(bindingPath,value);
                    }
                }
            }
        }


        public void Map<TDataContext>(DocX doc,TDataContext dataContext,string dateTimeFormat="yyyy-MM-dd",string? prefix=null)
        {
            IEnumerable<Binding> bindings = ParseDataContext(dataContext,dateTimeFormat,prefix);
            foreach(Binding binding in bindings)
            {
                _docXService.ReplaceToken(doc,_tokenDecorator.GetToken(binding.BindingPath),binding.Value);
            }
            // foreach(PropertyInfo propertyInfo in typeof(TDataContext).GetProperties())
            // {
            //     if (propertyInfo.GetCustomAttribute<IgnoreInMappingAttribute>() is null)
            //     {
            //         dynamic? value = propertyInfo.GetValue(dataContext);
            //         string? content = value switch
            //         {
            //             string contentString => contentString,
            //             DateTimeOffset dateTimeOffset => dateTimeOffset.ToString("yyyy-MM-dd"),
            //             DateTime dateTime => dateTime.ToString("yyyy-MM-dd"),
            //             _ when propertyInfo.PropertyType.IsPrimitive=> value?.ToString(),
            //             _ => null
            //         };

            //         if ((content is null) && (value is not Delegate))
            //         {
            //             Map(doc,value,propertyInfo.Name);
            //         }else{
            //             string bindingPath = _tokenDecorator.GetBindingPath(propertyInfo,prefix);
            //             _docXService.ReplaceToken(doc,_tokenDecorator.GetToken(bindingPath),content);
            //         }

            //     }
            // }
        }
    }
}