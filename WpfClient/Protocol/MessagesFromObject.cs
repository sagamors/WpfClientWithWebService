﻿using System.Collections.ObjectModel;

namespace Protocol
{
    public class MessagesFromObject : ReportParameter
    {
        public ObservableCollection<Sensor> Sensors { set; get; }

        public MessagesFromObject() : base("Сообщения от объекта", 0x02)
        {
            Sensors = new ObservableCollection<Sensor>();
        }
    }
}