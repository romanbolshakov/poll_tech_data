   M S B A M L   `   `   `  ����  techdata_charting_wpf/    )techdata_charting_wpf.View.ItemsWorkspace   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      P LAvalonControlsLibrary, Version=2.0.0.0, Culture=neutral, PublicKeyToken=nullIavalon<http://schemas.AvalonControls/AvalonControlsLibrary/Controls  5      .��5      �   � RowDefinitions  5	   
   �� 5
      $
��auto �6   �� 5      $��* �6   �� 5      $
��auto �6   .+�5      �� $��5q�6   
���   �$��1q�64    �� CornerRadius$ 4��6H   $	��Top=�6Y   ._�5      ��  �� Orientation$ 
Horizontal=�6   $��5q�63   .+�5      �� $.��(Коллекция параметров:��6   $��5q�69   �� 5      $��3q�6   $	��200��5      ��5      ��  �� Path$ CustomItemCollections� �� SelectedItem 5      �� $" SelectedCustomItemCollection��� 5      $��5q�6   $��Right=�6'   �� 5       �� Command 6    �� $# UpdateCustomCollectionCommand�.�6W   .�5      �� $)��#Обновить коллекцию��6$   5      5      5      5      &   System.Windows.Controls.DataGrid  5      $��1��6   $��5q�5         AutoGenerateColumns .     AlternatingRowBackground
 �����$��Stretch=�5!      ��5      �� $ ItemsValuesCollection�   Columns 5"      0  *System.Windows.Controls.DataGridTextColumn  5#      ,  &System.Windows.Controls.DataGridColumn	   Header$	 Параметр��6%   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 67   �� $ AdvancedItemCaption�  5$      $	 Дата\Время��6%   
 69   �� $ LastValueTimestamp�  5%      $	 Значение��6%   
 67   �� $ 	LastValue�  5&      $	 Ед.изм.��6%   
 66   �� $ ItemUnit�5(      �� 5)   
   $��2��6   $��5q�6   
���   �$��1q�6=   $ 4��6Q   $��Bottom=�6b   ._�5*      �� $ Vertical=�6   $��5q�60   .+�5+      �� $ 
Horizontal=�6   .+�5,      �� $'��!Текущее состояние��6   $��5q�69   ��6D   �� $ GetCurrentStateCommand��� 5-      $$��Автообновление: ��6    $	��5,9q�68   �� 5.      $��Вкл.��6   $��5,5,0,5q�6,   $	��7,0q�6=   {�.  6K   �� 5/      $��	Выкл.��6   $��0,5,5,5q�6-   $	��2,0q�6>   {�.  6L   50      �� 51      $ 
Horizontal=�6   .+�52      �� $��
Дата: ��6    $
��5,12q�6.   (  "System.Windows.Controls.DatePicker  53      $��5q�6!   $��25��6,      SelectedDate 6;   �� $ 
CustomDate��� 54      $��Время:��6    $
��5,12q�6.   2  ,AC.AvalonControlsLibrary.Controls.TimePicker  55      -   5          Name$ customTimePicker��55   (   $��5q�6B   $��25��6M      SelectedTime 6\   �� $ 
CustomTime��� 56      $��Обновить��6   $��5q�60   $��25��6;   ��6J   �� $ RefreshCustomDataCommand�57      58      59      5:      5;      