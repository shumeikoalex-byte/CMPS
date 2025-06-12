using System;
using System.Collections.Generic;

namespace EConstruction.Models
{
    /// <summary>
    /// Модель даних для пункту меню.
    /// </summary>
    public class MenuItem
    {
        public string Header { get; set; } = string.Empty; // Заголовок пункту меню
        public string IconPathData { get; set; } = string.Empty; // Path.Data для іконки
        public string EmojiIcon { get; set; } = string.Empty; // Або Emoji іконка
        public Type? TargetViewType { get; set; } // Тип View, до якого здійснюється навігація
        public List<MenuItem>? SubMenuItems { get; set; } // Підменю (1 рівень вкладеності)
    }
}
