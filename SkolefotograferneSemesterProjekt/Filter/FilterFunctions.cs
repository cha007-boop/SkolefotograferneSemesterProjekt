using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Helpers.Filter
{
    public class FilterFunctions<T> 
    {
        #region Methods
        /// <summary>
        /// Filters a sequence of values based on a set of predicates. Only items that satisfy
        /// all provided predicates are included in the result.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input sequence.</typeparam>
        /// <param name="values">The sequence of values to filter.</param>
        /// <param name="predicates">A list of conditions that each item must satisfy.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing only the elements that match all predicates.
        /// </returns>
        /// <remarks>
        /// An item is included in the result only if every predicate in <paramref name="predicates"/>
        /// returns <c>true</c> for that item.
        /// </remarks>
        public static IEnumerable<T> Filter(IEnumerable<T> values, IEnumerable<Predicate<T>> predicates)
        {
            List<T> filteredValues = new List<T>();
            foreach (T item in values)
            {
                int count = 0;
                foreach (Predicate<T> predicate in predicates)
                {
                    if (predicate(item))
                    {
                        count++;
                    }
                }
                if(count == predicates.Count())
                {
                    filteredValues.Add(item);
                }
            }
            return filteredValues;
        }
        #endregion
    }
}