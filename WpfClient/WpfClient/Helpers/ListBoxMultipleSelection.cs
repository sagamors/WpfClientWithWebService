using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace WpfClient.Helpers
{
    /// <summary>
    /// An attached property for supporting listbox selected items
    /// </summary>
    public class ListBoxMultipleSelection
    {
        private static ListBox list;
        public static readonly DependencyProperty SelectedItemsProperty = 
            DependencyProperty.RegisterAttached("SelectedItems", typeof(IList),typeof(ListBoxMultipleSelection),new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,OnSelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject d)
        {
            return (IList)d.GetValue(SelectedItemsProperty);
        }

        public static void SetSelectedItems(DependencyObject d, IList value)
        {
            d.SetValue(SelectedItemsProperty, value);
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = (ListBox)d;
            if (!Equals(list, listBox))
            {
                list = listBox;
                listBox.SelectionChanged -= listBox_SelectionChanged;
                listBox.SelectionChanged += listBox_SelectionChanged;
               
            }
            SelectItemsFromSource(list);
            var oldNotifyCollectionChanged = e.OldValue as INotifyCollectionChanged;
            if (oldNotifyCollectionChanged != null)
            {
                oldNotifyCollectionChanged.CollectionChanged -= NotifyCollectionChanged_CollectionChanged;
            }
            var notifyCollectionChanged = e.NewValue as INotifyCollectionChanged;
            if (notifyCollectionChanged != null)
            {
                notifyCollectionChanged.CollectionChanged -= NotifyCollectionChanged_CollectionChanged;
                notifyCollectionChanged.CollectionChanged += NotifyCollectionChanged_CollectionChanged;
            }
            list = listBox;
        }

        private static void NotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (list != null && !suppressCollectionChanged)
            {
                SelectItemsFromSource(list);
            }
        }

        private static bool suppressCollectionChanged;
        private static void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = sender as ListBox;
            var modelSelectedItems = GetSelectedItems(listBox);
            if(modelSelectedItems==null) return;
            suppressCollectionChanged = true;
            modelSelectedItems.Clear();
            if (listBox?.SelectedItems != null)
            {
                foreach (var item in listBox.SelectedItems)
                    modelSelectedItems.Add(item);
            }
            suppressCollectionChanged = false;
            SetSelectedItems(listBox, modelSelectedItems);
        }

        private static void SelectItemsFromSource(ListBox listBox)
        {
            
            if (listBox == null || listBox.ItemsSource==null) return;
            if (!listBox.IsLoaded)
            {
                listBox.Loaded += ListBox_Loaded;
                return;
            }
           var modelSelectedItems = GetSelectedItems(listBox);
            foreach (var item in listBox.ItemsSource)
            {
                ListBoxItem itemContainer =
                    listBox.ItemContainerGenerator.ContainerFromItem(item) as ListBoxItem;
                if (itemContainer != null)
                {
                    itemContainer.IsSelected = modelSelectedItems.Contains(item);
                }
            }
        }

        private static void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            var listBox = sender as ListBox;
            SelectItemsFromSource(listBox);
        }
    }
}
