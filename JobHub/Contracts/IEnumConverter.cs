using Microsoft.AspNetCore.Mvc.Rendering;

namespace JobHub.Contracts
{
    public interface IEnumConverter
    {
        IEnumerable<SelectListItem> GetSelectListFromEnum<T>(string? selectedValue = null) where T : Enum;
    }
}
