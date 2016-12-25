using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CasterCore
{
    public class TheCaster
    {

        public Dictionary<Pair<Type,Type>, Dictionary<string, string>> PropertyNameDictionary;

        private List<Pair<Type, Type>> _pairTypeList = new List<Pair<Type, Type>>();
        private Pair<Type, Type> _currentPairType;

        public TheCaster()
        {
            PropertyNameDictionary = new Dictionary<Pair<Type, Type>, Dictionary<string, string>>();
        }


        public TResult Cast<TResult, TSource>(TSource source) where TResult : class, new() where TSource : class
        {
            

            var result = new TResult();

            var findPair = new Pair<Type, Type>(typeof(TSource), typeof(TResult));
            var pairKey = _pairTypeList.Find(pair => pair.Left == findPair.Left && pair.Right == findPair.Right);
            var TSourceProperties = typeof(TSource).GetProperties();
            foreach (var sourceProperty in TSourceProperties)
            {
                if (PropertyNameDictionary[pairKey].ContainsKey(sourceProperty.Name))
                {
                    result.GetType().GetProperty(PropertyNameDictionary[pairKey][sourceProperty.Name]).SetValue(result, source.GetType().GetProperty(sourceProperty.Name).GetValue(source));
                }
            }


            return result;
        }

       
        public TheCaster AddPropertyName(string sourcePropertyName, string resultPropertyName)
        {
            if (!PropertyNameDictionary.ContainsKey(_currentPairType)) throw new ConversionNotFoundException();
            PropertyNameDictionary[_currentPairType].Add(sourcePropertyName,resultPropertyName);
            return this;
        }


        public T MapFrom<T>() where T : class, new()
        {
            var t = new T();
            return t;
        }

        public TheCaster InitializeMap(Type sourceType, Type resultType)
        {
           var pair = new Pair<Type,Type>(sourceType,resultType);
            _currentPairType = pair;
            PropertyNameDictionary[_currentPairType] = new Dictionary<string, string>();
            _pairTypeList.Add(_currentPairType);

            var sourceProperties = sourceType.GetProperties();
            var resultProperties = resultType.GetProperties();

           
            foreach (var sourceProperty in sourceProperties)
            {
                if (resultProperties.Contains(sourceProperty))
                {
                    PropertyNameDictionary[_currentPairType].Add(sourceProperty.Name,resultProperties.First(info => info == sourceProperty).Name);
                }
            }

            return this;
        }

        public string AnonymousTypeSerialize(int field1, int field2)
        {
            return JsonConvert.SerializeObject(new { intProp = field1, DataA = field2 });
        }


        public T AnonymousTypeDeserialize<T>(string target) where T : class, new()
        {
            return JsonConvert.DeserializeAnonymousType(target,new T());
        }
    }

   
}
