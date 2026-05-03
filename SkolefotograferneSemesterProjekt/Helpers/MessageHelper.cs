using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SkolefotograferneSemesterProjekt.Helpers
{
    /// <summary>
    /// A static class used to customise validation messages
    /// </summary>
    public static class MessageHelper
    {
        /// <summary>
        /// Given a ModelStateDictionary customise messages
        /// </summary>
        /// <param name="msDict"></param>
        public static void CustomizedMessages(this ModelStateDictionary msDict, string message)
        {
            foreach (var key in msDict.Keys)
            {
                var entry = msDict[key];
                if (entry.Errors.Count > 0)
                {
                    entry.Errors.Clear();
                    entry.Errors.Add(new ModelError(message));
                }
            }
            
        }
    }
}
