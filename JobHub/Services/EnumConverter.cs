using JobHub.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobHub.Services
{
    public class EnumConverter : IEnumConverter
    {
        public IEnumerable<SelectListItem> GetSelectListFromEnum<T>(string? selectedValue = null) where T : Enum
        {
            List<SelectListItem> selectList = new List<SelectListItem>();

            var elements = typeof(T).GetMembers(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Select(e => (T)Enum.Parse(typeof(T), e.Name));

            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem(element.GetDisplayValueName(), element.ToString()));
            }

            if (selectedValue != null)
            {
                selectList.Where(e => e.Value == selectedValue).First().Selected = true;
            }

            return selectList;
        }

    }
}
