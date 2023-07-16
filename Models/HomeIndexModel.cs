using Microsoft.AspNetCore.Mvc.Rendering;

namespace checkboxlist.Models
{
    public class HomeIndexModel
    {
        public List<MonsterFeatureItem> MonsterFeatureItems { get; set; }
        public List<int> ProfileIds { get; set; }
    }
    
    public class ProfileItem
    {
        public int ProfileId { get; set; }
        public string Name { get; set; }
    }

    public class CheckBoxListItem<T>
    {
        public bool Checked { get; set; }
        public T Value { get; set; }
    }

    public class MonsterFeatureItem : CheckBoxListItem<string>
    {
        public static List<MonsterFeatureItem> CreateListFromSelectListItems(IEnumerable<SelectListItem> selectListItems)
        {
            return selectListItems.Select(x => new MonsterFeatureItem()
            {
                Checked = false,
                Value = x.Value,
            }).ToList();
        }
    }
}