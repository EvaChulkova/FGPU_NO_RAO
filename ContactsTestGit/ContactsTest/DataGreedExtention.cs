﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ContactsTest
{
	public static class ExtendedEvents
	{
		public static readonly DependencyProperty DataGridDoubleClickProperty =
		  DependencyProperty.RegisterAttached("DataGridDoubleClickCommand", typeof(ICommand), typeof(ExtendedEvents),
							new PropertyMetadata(new PropertyChangedCallback(AttachOrRemoveDataGridDoubleClickEvent)));

		public static ICommand GetDataGridDoubleClickCommand(DependencyObject obj)
		{
			return (ICommand)obj.GetValue(DataGridDoubleClickProperty);
		}

		public static void SetDataGridDoubleClickCommand(DependencyObject obj, ICommand value)
		{
			obj.SetValue(DataGridDoubleClickProperty, value);
		}

		public static void AttachOrRemoveDataGridDoubleClickEvent(DependencyObject obj, DependencyPropertyChangedEventArgs args)
		{
			DataGrid dataGrid = obj as DataGrid;
			if (dataGrid != null)
			{
				ICommand cmd = (ICommand)args.NewValue;

				if (args.OldValue == null && args.NewValue != null)
				{
					dataGrid.MouseDoubleClick += ExecuteDataGridDoubleClick;
				}
				else if (args.OldValue != null && args.NewValue == null)
				{
					dataGrid.MouseDoubleClick -= ExecuteDataGridDoubleClick;
				}
			}
		}

		private static void ExecuteDataGridDoubleClick(object sender, MouseButtonEventArgs args)
		{
			DependencyObject obj = sender as DependencyObject;
			ICommand cmd = (ICommand)obj.GetValue(DataGridDoubleClickProperty);
			if (cmd != null)
			{
				if (cmd.CanExecute(obj))
				{
					cmd.Execute(obj);
				}
			}
		}
	}
}
